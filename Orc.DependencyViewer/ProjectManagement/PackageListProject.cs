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

        public IEnumerable<IEntity> DependencyCollection { get; set; }

        public string RawContent { get; set; }

        public bool IsFinished { get; set; }
    }
}
