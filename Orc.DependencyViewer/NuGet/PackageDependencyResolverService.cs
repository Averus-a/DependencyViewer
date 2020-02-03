namespace Orc.DependencyViewer
{
    using Catel;
    using Catel.Logging;
    using NuGet.Common;
    using NuGet.Frameworks;
    using NuGet.Packaging.Core;
    using NuGet.Protocol.Core.Types;
    using NuGet.Resolver;
    using Orc.NuGetExplorer;
    using Orc.NuGetExplorer.Management.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class PackageDependencyResolverService : IPackageDependencyResolverService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private static readonly ILogger NuGetLogger = NullLogger.Instance;

        private readonly IFrameworkNameProvider _frameworkNameProvider;
        private readonly IEnumerable<SourceRepository> repositories;

        public PackageDependencyResolverService(IFrameworkNameProvider frameworkNameProvider)
        {
            _frameworkNameProvider = frameworkNameProvider;
        }

        public async Task<IEnumerable<PackageIdentity>> ResolveAsync(PackageIdentity package, NuGetFramework targetFramework, bool ignoreMissingPackages = true)
        {
            DependencyBehavior dependencyBehavior = DependencyBehavior.Lowest;

            //gathered dependencies
            var availabePackageStorage = new HashSet<SourcePackageDependencyInfo>(PackageIdentityComparer.Default);

            //gathering resource from available sources
            var getDependencyResourcesTasks = repositories.Select(repo => repo.GetResourceAsync<DependencyInfoResource>());

            var dependencyResources = (await getDependencyResourcesTasks.WhenAllOrException()).Where(x => x.IsSuccess && x.Result != null)
                .Select(x => x.Result).ToArray();

            var dependencyInfoResources = new DependencyInfoResourceCollection(dependencyResources);

            //recursively collect all dependencies
            dependencyBehavior = await ResolveDependenciesRecursivelyAsync(package, targetFramework, dependencyInfoResources, NullSourceCacheContext.Instance,
                availabePackageStorage, ignoreMissingPackages, default);

            return availabePackageStorage.ToList();
        }

        private async Task<DependencyBehavior> ResolveDependenciesRecursivelyAsync(PackageIdentity identity, NuGetFramework targetFramework,
           DependencyInfoResourceCollection dependencyInfoResource,
           SourceCacheContext cacheContext,
           HashSet<SourcePackageDependencyInfo> storage,
           bool ignoreMissingPackages = false,
           CancellationToken cancellationToken = default)
        {
            Argument.IsNotNull(() => storage);

            HashSet<SourcePackageDependencyInfo> packageStore = storage;

            Stack<SourcePackageDependencyInfo> downloadStack = new Stack<SourcePackageDependencyInfo>();

            var resolvedBehavior = DependencyBehavior.Lowest;

            //get top dependency
            var dependencyInfo = await dependencyInfoResource.ResolvePackage(
                            identity, targetFramework, cacheContext, NuGetLogger, cancellationToken);

            if (dependencyInfo == null)
            {
                Log.Error($"Cannot resolve {identity} package for target framework {targetFramework}");
                return resolvedBehavior;
            }

            downloadStack.Push(dependencyInfo); //and add it to package store

            while (downloadStack.Count > 0)
            {
                var rootDependency = downloadStack.Pop();

                //store all new packges
                if (!packageStore.Contains(rootDependency))
                {
                    packageStore.Add(rootDependency);
                }
                else
                {
                    continue;
                }

                foreach (var dependency in rootDependency.Dependencies)
                {
                    var relatedIdentity = new PackageIdentity(dependency.Id, dependency.VersionRange.MinVersion);

                    var relatedDepInfo = await dependencyInfoResource.ResolvePackage(relatedIdentity, targetFramework, cacheContext, NuGetLogger, cancellationToken);

                    if (relatedDepInfo != null)
                    {
                        downloadStack.Push(relatedDepInfo);
                    }

                    if (ignoreMissingPackages)
                    {
                        resolvedBehavior = DependencyBehavior.Ignore;
                        Log.Warning($"Available sources doesn't contain package {relatedIdentity}. Package {relatedIdentity} is missing");
                    }
                    else
                    {
                        throw new MissingPackageException($"Cannot find package {relatedIdentity}");
                    }
                }
            }

            return resolvedBehavior;
        }
    }
}
