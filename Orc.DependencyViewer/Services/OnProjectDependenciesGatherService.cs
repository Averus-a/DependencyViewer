﻿namespace Orc.DependencyViewer.Services
{
    using Catel.Logging;
    using Orc.DependencyViewer.Model;
    using Orc.DependencyViewer.ProjectManagement;
    using Orc.NuGetExplorer;
    using Orc.ProjectManagement;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OnProjectDependenciesGatherService : IOnProjectOperationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly IPackageDependencyResolverService _packageDependencyResolverService;
        private readonly IDefaultNuGetFramework _defaultNuGetFramework;

        public OnProjectDependenciesGatherService(IPackageDependencyResolverService packageDependencyResolverService, IDefaultNuGetFramework defaultNuGetFramework)
        {
            _packageDependencyResolverService = packageDependencyResolverService;
            _defaultNuGetFramework = defaultNuGetFramework;
        }

        public async Task<bool> ExecuteAsync(IProject project)
        {
            if(!(project is PackageListProject activeProject))
            {
                Log.Error("Project type is unsupported for this operation");
                return false;
            }

            var updatedPackageCollection = new List<PackageReference>(activeProject.PackageCollection.Cast<PackageReference>());

            foreach(var reference in activeProject.PackageCollection)
            {
                await ResolvePackageReferenceDependencyAsync(reference as PackageReference, updatedPackageCollection);
            }

            //setup new references collection
            activeProject.PackageCollection = updatedPackageCollection.Distinct();

            return true;
        }

        private async Task ResolvePackageReferenceDependencyAsync(PackageReference packageReference, List<PackageReference> gatheredReferences)
        {
            var chosenFramework = _defaultNuGetFramework.GetLowest().LastOrDefault();

            var result = await _packageDependencyResolverService.ResolveAsync(packageReference.From, chosenFramework);

            gatheredReferences.AddRange(result.Select(x => new PackageReference(packageReference.From, x)));
        }
    }
}