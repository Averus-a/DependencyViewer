using NuGet.Packaging.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orc.DependencyViewer.Model
{
    public class PackageReference : IEntity
    {
        public PackageReference()
        {

        }

        public PackageReference(PackageIdentity from, PackageIdentity to)
        {
            From = from;
            To = to;
        }

        public PackageIdentity From { get; set; }

        public PackageIdentity To { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is PackageReference objPackage)
            {
                return (From is null && objPackage.From is null || From.Equals(objPackage.From)
                   && (To is null && objPackage.To is null || To.Equals(objPackage.To)));
            }

            throw new InvalidOperationException();
        }

        public override int GetHashCode()
        {
            return From?.GetHashCode() ?? 0 ^ To?.GetHashCode() ?? 0;
        }
    }
}
