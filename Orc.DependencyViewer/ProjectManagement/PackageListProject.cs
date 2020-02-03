namespace Orc.DependencyViewer.ProjectManagement
{
    using Orc.DependencyViewer.Model;
    using Orc.ProjectManagement;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class PackageListProject : ProjectBase
    {
        public PackageListProject(string location) : base(location)
        {
            Location = location;
        }

        public IEnumerable<IEntity> PackageCollection { get; set; }
    }
}
