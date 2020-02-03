using Catel.IoC;
using Orc.DependencyViewer.Services;

namespace Orc.DependencyViewer
{
    public static class ModuleInitializer
    {
        public static void Initialize()
        {
            var serviceLocator = ServiceLocator.Default;
            serviceLocator.RegisterType<IFileWriterService, ExportFileWriterService>();
            serviceLocator.RegisterType<IFileReaderService, ImportFileReaderService>();
        }
    }
}

