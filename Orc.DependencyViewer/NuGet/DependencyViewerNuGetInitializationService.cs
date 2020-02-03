namespace Orc.DependencyViewer
{
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.NuGetExplorer;
    using Orc.NuGetExplorer.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DependencyViewerNuGetInitializationService : NuGetExplorerInitializationService
    {
        public DependencyViewerNuGetInitializationService(ILanguageService languageService, ICredentialProviderLoaderService credentialProviderLoaderService, 
            INuGetProjectUpgradeService nuGetProjectUpgradeService, IViewModelLocator vmLocator, ITypeFactory typeFactory) 
            : base(languageService, credentialProviderLoaderService, nuGetProjectUpgradeService, vmLocator, typeFactory)
        {
        }
    }
}
