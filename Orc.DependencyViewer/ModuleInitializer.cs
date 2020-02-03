namespace Orc.DependencyViewer
{
    using Catel.IoC;
    using NuGet.Frameworks;
    using Orc.DependencyViewer.ProjectManagement;
    using Orc.DependencyViewer.Services;
    using Orc.NuGetExplorer;
    using Orc.NuGetExplorer.Services;
    using Orc.ProjectManagement;

    public static class ModuleInitializer
    {
        public static void Initialize()
        {
            var serviceLocator = ServiceLocator.Default;
            serviceLocator.RegisterType<IFileWriterService, ExportFileWriterService>();
            serviceLocator.RegisterType<IFileReaderService, ImportFileReaderService>();
            serviceLocator.RegisterType<IProject, PackageListProject>();

            serviceLocator.RegisterType<IProjectReader, ProjectReader>();
            serviceLocator.RegisterType<IProjectWriter, ProjectWriter>();
            serviceLocator.RegisterType<IOnProjectOperationService, OnProjectDependenciesGatherService>();
            serviceLocator.RegisterType<IPackageDependencyResolverService, PackageDependencyResolverService>();
            serviceLocator.RegisterType<IFrameworkNameProvider, DefaultFrameworkNameProvider>();
            serviceLocator.RegisterType<IDefaultNuGetFramework, DefaultNuGetFramework>();
            serviceLocator.RegisterType<INuGetExplorerInitializationService, DependencyViewerNuGetInitializationService>();

            //serviceLocator.RegisterType<IProjectWriter>();
        }
    }
}

