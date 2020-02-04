namespace Orc.DependencyViewer.Services
{
    using Catel.Logging;
    using NuGet.Protocol.Core.Types;
    using Orc.DependencyViewer.Model;
    using Orc.DependencyViewer.ProjectManagement;
    using Orc.NuGetExplorer;
    using Orc.ProjectManagement;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OnProjectDependenciesGatherService : IOnProjectOperationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly IPackageDependencyResolverService _packageDependencyResolverService;
        private readonly IDefaultNuGetFramework _defaultNuGetFramework;

        public OnProjectDependenciesGatherService(IPackageDependencyResolverService packageDependencyResolverService, IDefaultNuGetFramework defaultNuGetFramework)
        {
            _packageDependencyResolverService = packageDependencyResolverService;
            _defaultNuGetFramework = defaultNuGetFramework;
        }

        public async Task<bool> ExecuteAsync(IProject project, IOperationOptions options)
        {
            if(!(project is PackageListProject activeProject))
            {
                Log.Error("Project type is unsupported for this operation");
                return false;
            }

            if(!(options is DependenciesGatherOptions operationOptions))
            {
                Log.Error("Options type is unsupported for this operation");
                return false;
            }


            var updatedPackageCollection = new List<PackageReference>(activeProject.PackageCollection.Cast<PackageReference>());

            foreach(var reference in activeProject.PackageCollection)
            {
                await ResolvePackageReferenceDependencyAsync((PackageReference)reference, updatedPackageCollection, operationOptions.UseStableDependencies);
            }

            //setup new references collection
            updatedPackageCollection = updatedPackageCollection.Where(reference => reference.To != null).Distinct().ToList();

            if(operationOptions.BuildDependenciesOnlyFromList)
            {
                var identityFilterList = activeProject.PackageCollection.Cast<PackageReference>().Select(reference => reference.From).ToHashSet();
                updatedPackageCollection = updatedPackageCollection.Where(
                        reference => identityFilterList.Contains(reference.From) 
                        && identityFilterList.Contains(reference.To))
                    .ToList();
            }

            activeProject.DependencyCollection = updatedPackageCollection.Cast<IEntity>();
            activeProject.IsFinished = true;

            return true;
        }

        private async Task ResolvePackageReferenceDependencyAsync(PackageReference packageReference, List<PackageReference> gatheredReferences, bool useStableVersions)
        {
            var chosenFramework = _defaultNuGetFramework.GetLowest().LastOrDefault();

            var result = await _packageDependencyResolverService.ResolveAsync(packageReference.From, chosenFramework, useStableVersions);

            //transform to distinct list of package references
            var transformedCollection = result.SelectMany(package => (package as SourcePackageDependencyInfo)?.ToSeparateReferences()).Distinct().ToList();

            gatheredReferences.AddRange(transformedCollection);
        }
    }

    public class DependenciesGatherOptions : IOperationOptions
    {
        /// <summary>
        /// Use nearest suitable stable version from dependency versions range.
        /// </summary>
        public bool UseStableDependencies { get; set; }
        /// <summary>
        /// Throw away dependencies which aren't listed in initial package collection
        /// </summary>
        public bool BuildDependenciesOnlyFromList { get; set; }
    }
}
