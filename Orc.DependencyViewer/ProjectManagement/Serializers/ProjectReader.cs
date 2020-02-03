namespace Orc.DependencyViewer.ProjectManagement
{
    using Orc.DependencyViewer.Model;
    using Orc.DependencyViewer.Services;
    using Orc.FileSystem;
    using Orc.ProjectManagement;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ProjectReader : ProjectReaderBase
    {
        private readonly IFileReaderService _fileReaderService;
        private readonly IFileService _rawFileService;
        private readonly IProjectManager _projectManager;

        public ProjectReader(IFileReaderService fileReaderService, IProjectManager projectManager, IFileService fileService)
        {
            _fileReaderService = fileReaderService;
            _projectManager = projectManager;
            _rawFileService = fileService;
        }

        protected override async Task<IProject> ReadFromLocationAsync(string location)
        {
            var entityRecords = await _fileReaderService.ReadAsync(location);
            var text = await _rawFileService.ReadAllTextAsync(location);

            var project = new PackageListProject(location);
            project.RawContent = text;
            
            InitializeProjectContent(project, entityRecords);

            await _projectManager.SetActiveProjectAsync(project);

            return project;
        }

        private IProject InitializeProjectContent(PackageListProject project, IEnumerable<IEntity> records)
        {
            project.PackageCollection = records;
            return project;
        }
    }
}
