﻿namespace Orc.DependencyViewer.Services
{
    using Orc.DependencyViewer.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IFileReaderService
    {
        Task<IEnumerable<IEntity>> ReadAsync(string path);
    }
}
