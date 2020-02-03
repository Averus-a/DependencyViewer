namespace Orc.DependencyViewer.Services
{
    using Orc.ProjectManagement;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IOnProjectOperationService
    {
        Task<bool> ExecuteAsync(IProject project);
    }
}
