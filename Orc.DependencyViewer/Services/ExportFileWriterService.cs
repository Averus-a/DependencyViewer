namespace Orc.DependencyViewer.Services
{
    using Orc.Csv;
    using Orc.DependencyViewer.Maps;
    using Orc.DependencyViewer.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class ExportFileWriterService : IFileWriterService
    {
        private readonly ICsvWriterService _csvWriterService;

        public ExportFileWriterService(ICsvWriterService csvWriterService)
        {
            _csvWriterService = csvWriterService;
        }

        public async Task WriteAsync(string path, IEnumerable<IEntity> records)
        {
            var csvContext = new CsvContext<PackageReference>();
            csvContext.ClassMap = new PackageReferenceMap();
            var typedRecords = records.Cast<PackageReference>().ToList();
            await _csvWriterService.WriteRecordsAsync<PackageReference, PackageReferenceMap>(typedRecords, path);
        }
    }
}
