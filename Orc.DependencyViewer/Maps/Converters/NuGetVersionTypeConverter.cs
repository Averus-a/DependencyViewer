namespace Orc.DependencyViewer.Maps
{
    using Catel.Logging;
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.TypeConversion;
    using NuGet.Versioning;
    using Orc.NuGetExplorer;

    public class NuGetVersionTypeConverter : DefaultTypeConverter
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (!NuGetVersion.TryParse(text, out NuGetVersion version))
            {
                Log.Warning($"{text} {Constants.Messages.PackageParserInvalidVersion}");
                return null;
            }

            return version;
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return value?.ToString() ?? string.Empty;
        }
    }
}
