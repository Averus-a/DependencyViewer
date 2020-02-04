namespace Orc.DependencyViewer.ViewModels
{
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.DependencyViewer.ProjectManagement;
    using Orc.DependencyViewer.Services;
    using Orc.FileSystem;
    using Orc.NuGetExplorer;
    using Orc.ProjectManagement;
    using System;
    using System.Threading.Tasks;

    public class MainViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IOpenFileService _openFileService;
        private readonly ISaveFileService _saveFileService;
        private readonly IFileService _fileService;
        private readonly IPleaseWaitService _pleaseWaitService;
        private readonly IProjectManager _projectManager;
        private readonly IPackagesUIService _packagesUIService; 
        private readonly IOnProjectOperationService _projectOperationService;

        public MainViewModel(IOpenFileService openFileService, ISaveFileService saveFileService, IFileService fileService, IPleaseWaitService pleaseWaitService, 
            IProjectManager projectManager, IOnProjectOperationService projectOperationService, IPackagesUIService packagesUIService)
        {
            Argument.IsNotNull(() => openFileService);
            Argument.IsNotNull(() => saveFileService);
            Argument.IsNotNull(() => fileService);
            Argument.IsNotNull(() => pleaseWaitService);
            Argument.IsNotNull(() => projectManager);
            Argument.IsNotNull(() => projectOperationService);
            Argument.IsNotNull(() => packagesUIService);

            _openFileService = openFileService;
            _saveFileService = saveFileService;
            _fileService = fileService;
            _pleaseWaitService = pleaseWaitService;
            _projectManager = projectManager;
            _projectOperationService = projectOperationService;
            _packagesUIService = packagesUIService;
        }

        public IProject ActiveProject { get; set; }

        public string RawText { get; set; }

        public object Scope { get; set; } = "Orc.DependencyViewer";

        public bool UseStableDependencies { get; set; } = true;

        public bool BuildDependenciesOnlyFromList { get; set; } = true;

        protected override Task InitializeAsync()
        {
            OpenFile = new TaskCommand(OpenFileExecuteAsync);
            SaveFile = new TaskCommand(SaveFileExecuteAsync, SaveFileCanExecute);
            GatherDependenciesOnProject = new TaskCommand(GatherDependenciesOnProjectExecuteAsync, GatherDependenciesOnProjectCanExecute);
            OpenPackageSourceSettings = new TaskCommand(OpenPackageSourceSettingsExecuteAsync);

            _projectManager.ProjectActivatedAsync += OnProjectActivatedAsync;

            return base.InitializeAsync();
        }

        private async Task OnProjectActivatedAsync(object sender, ProjectUpdatedEventArgs e)
        {
            ActiveProject = _projectManager.ActiveProject;
            RawText = (ActiveProject as PackageListProject)?.RawContent;
        }

        #region commands

        public TaskCommand OpenFile { get; set; }

        private async Task OpenFileExecuteAsync()
        {
            await OpenCsvFileAsync();
        }

        public TaskCommand SaveFile { get; set; }

        private bool SaveFileCanExecute()
        {
            return ActiveProject != null;
        }

        private async Task SaveFileExecuteAsync()
        {
            await ExportProjectAsCsvAsync();
        }

        public TaskCommand GatherDependenciesOnProject { get; set; }

        private bool GatherDependenciesOnProjectCanExecute()
        {
            return ActiveProject != null;
        }

        private async Task GatherDependenciesOnProjectExecuteAsync()
        {
            using (_pleaseWaitService.PushInScope())
            {
                var options = new DependenciesGatherOptions()
                { 
                    UseStableDependencies = UseStableDependencies,
                    BuildDependenciesOnlyFromList = BuildDependenciesOnlyFromList
                };

                await _projectOperationService.ExecuteAsync(_projectManager.ActiveProject, options);
            }
        }

        public TaskCommand OpenPackageSourceSettings { get; set; }


        private async Task OpenPackageSourceSettingsExecuteAsync()
        {
            await _packagesUIService.ShowPackagesSourceSettingsAsync();
        }

        #endregion

        private async Task OpenCsvFileAsync()
        {
            try
            {
                string location = string.Empty;

                _openFileService.Filter = "Text Files (*.csv)|*csv";
                _openFileService.IsMultiSelect = false;

                if (await _openFileService.DetermineFileAsync())
                {
                    location = _openFileService.FileName;
                }

                if (!string.IsNullOrWhiteSpace(location))
                {
                    using (_pleaseWaitService.PushInScope())
                    {
                        await _projectManager.LoadAsync(location);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to open file");
            }
        }

        private async Task ExportProjectAsCsvAsync()
        {
            try
            {
                _saveFileService.InitialDirectory = ActiveProject.Location;

                if (await _saveFileService.DetermineFileAsync())
                {
                    var location = _saveFileService.FileName;
                    await _projectManager.SaveAsync(location);
                    Log.Info($"Wrote file on path {location}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save file");
            }
        }
    }
}