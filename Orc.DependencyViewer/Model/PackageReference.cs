using NuGet.Packaging.Core;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orc.DependencyViewer.Model
{
    public struct PackageReference : IEntity
    {
        public PackageReference(PackageIdentity from, PackageIdentity to)
        {
            FromVersion = from.Version;
            FromId = from.Id;

            ToVersion = to.Version;
            ToId = to.Id;
        }

        public string FromId { get; set; }
        public NuGetVersion FromVersion { get; set;}
        public PackageIdentity From => FromId is null ? null : new PackageIdentity(FromId, FromVersion);

        public string ToId { get; set; }
        public NuGetVersion ToVersion { get; set; }
        public PackageIdentity To => ToId is null ? null : new PackageIdentity(ToId, ToVersion);


        public override bool Equals(object obj)
        {
            if (obj is PackageReference objPackage)
            {
                return From.Equals(objPackage.From) && To.Equals(objPackage.To);
                //return PackageIdentityEquals(From, objPackage.From) && PackageIdentityEquals(To, objPackage.To);
            }

            throw new InvalidOperationException();
        }

        private bool PackageIdentityEquals(PackageIdentity x, PackageIdentity y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null)
                || ReferenceEquals(y, null))
            {
                return false;
            }

            var versionComparer = new VersionComparer(VersionComparison.Default);
            return versionComparer.Equals(x.Version, y.Version)
                   && StringComparer.OrdinalIgnoreCase.Equals(x.Id, y.Id);
        }

        public override int GetHashCode()
        {
            return From?.GetHashCode() ?? 0 ^ To?.GetHashCode() ?? 0;
        }
    }
}
