using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Globalization;

namespace PMO.API.Converter
{
    [ExcludeFromCodeCoverage]
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var cultureInfo = new CultureInfo("");
            cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            var strDate = reader.GetString();
            if (string.IsNullOrWhiteSpace(strDate))
                return DateTime.MinValue;
            else
                return DateTime.Parse(strDate, cultureInfo.DateTimeFormat);

            //if (reader.TokenType

                //    return DateTime.Parse(reader.GetString());
                //else
                //    return DateTime.MinValue;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
        }
    }
}
