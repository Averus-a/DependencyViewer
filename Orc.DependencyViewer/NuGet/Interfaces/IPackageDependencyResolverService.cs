namespace Orc.DependencyViewer
{
    using NuGet.Frameworks;
    using NuGet.Packaging.Core;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPackageDependencyResolverService
    {
        Task<IEnumerable<PackageIdentity>> ResolveAsync(PackageIdentity package, NuGetFramework targetFramework, bool includePrelease, bool ignoreMissingPackages = true);
    }
}
