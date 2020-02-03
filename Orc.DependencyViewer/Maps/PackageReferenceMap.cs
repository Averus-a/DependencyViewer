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
            Map(entity => entity.To).TypeConverter<PackageIdentityTypeConverter>();
            Map(entity => entity.From).TypeConverter<PackageIdentityTypeConverter>();
        }

        private class PackageIdentityTypeConverter : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                return PackageIdentityParser.Parse(text);
            }

            public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {
                return value.ToString();
            }
        }
    }
}
