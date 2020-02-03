namespace Orc.DependencyViewer
{
    using NuGet.Packaging.Core;
    using NuGet.Protocol.Core.Types;
    using NuGet.Resolver;
    using NuGet.Versioning;
    using Orc.DependencyViewer.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class SourcePackageDependencyInfoExtensions
    {
        public static IEnumerable<PackageReference> ToSeparateReferences(this SourcePackageDependencyInfo packageDependencyInfo)
        {
            return packageDependencyInfo.Dependencies.Select(d => new PackageReference(
                new PackageIdentity(packageDependencyInfo.Id, packageDependencyInfo.Version), 
                new PackageIdentity(d.Id, d.VersionRange.MinVersion)));
        }
    }
}
