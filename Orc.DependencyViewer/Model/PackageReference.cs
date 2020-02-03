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
    }
}
