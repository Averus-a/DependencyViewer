namespace Orc.DependencyViewer.Maps
{
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.TypeConversion;
    using Orc.Csv;
    using Orc.DependencyViewer.Model;
    using Orc.NuGetExplorer;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PackageReferenceMap : ClassMapBase<PackageReference>
    {
        public PackageReferenceMap()
        {
            Map(entity => entity.FromId);
            Map(entity => entity.FromVersion).TypeConverter<NuGetVersionTypeConverter>();
            Map(entity => entity.ToId);
            Map(entity => entity.ToVersion).TypeConverter<NuGetVersionTypeConverter>();
        }
    }

    public class PackageReferenceReadMap : ClassMapBase<PackageReference>
    {
        public PackageReferenceReadMap()
        {
            Map(entity => entity.FromId);
            Map(entity => entity.FromVersion).TypeConverter<NuGetVersionTypeConverter>();
        }
    }
}
