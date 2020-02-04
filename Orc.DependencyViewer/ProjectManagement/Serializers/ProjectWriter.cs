namespace Orc.DependencyViewer.ProjectManagement
{
    using Orc.DependencyViewer.Services;
    using Orc.ProjectManagement;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class ProjectWriter : ProjectWriterBase<PackageListProject>
    {
        private readonly IFileWriterService _fileWriterService;

        public ProjectWriter(IFileWriterService fileWriterService)
        {
            _fileWriterService = fileWriterService;
        }

        protected override async Task<bool> WriteToLocationAsync(PackageListProject project, string location)
        {
            try
            {
                await _fileWriterService.WriteAsync(location, project.DependencyCollection);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
