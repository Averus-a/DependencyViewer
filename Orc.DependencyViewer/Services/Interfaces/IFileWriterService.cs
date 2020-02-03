namespace Orc.DependencyViewer.Services
{
    using Orc.DependencyViewer.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    interface IFileWriterService
    {
        Task WriteAsync(string path, IEnumerable<IEntity> records);
    }
}
