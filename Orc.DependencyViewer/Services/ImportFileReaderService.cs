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

    public class ImportFileReaderService : IFileReaderService
    {
        private readonly ICsvReaderService _csvReaderService;

        public ImportFileReaderService(ICsvReaderService csvReaderService)
        {
            _csvReaderService = csvReaderService;
        }

        public async Task<IEnumerable<IEntity>> ReadAsync(string path)
        {
            var csvContext = new CsvContext<PackageReference>();
            csvContext.ClassMap = new PackageReferenceReadMap();
            var records = await _csvReaderService.ReadRecordsAsync<PackageReference>(path, csvContext);

            return records.Cast<IEntity>();
        }
    }
}
