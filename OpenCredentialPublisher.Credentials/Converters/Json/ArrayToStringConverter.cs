using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Credentials.Converters.Json
{
    public class ArrayToStringConverter : JsonConverter<String>
    {

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(String));
        }

        public override String Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var item = string.Empty;
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                if (jsonDoc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    item = String.Join(',', jsonDoc.RootElement.EnumerateArray().Select(a => a.GetRawText()).ToList());
                }
                else 
                {
                    item = jsonDoc.RootElement.GetRawText();
                }
            }
            return item;
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            if (value != null) 
            {
                if (value.Contains(","))
                {
                    writer.WriteStartArray();
                    foreach (var item in value.Split(","))
                    {
                        var jsonDocument = JsonDocument.Parse(JsonSerializer.Serialize(item, options));
                        jsonDocument.WriteTo(writer);
                    }
                    writer.WriteEndArray();
                }
                else
                {
                    var jsonDocument = JsonDocument.Parse(JsonSerializer.Serialize(value, options));
                    jsonDocument.WriteTo(writer);
                }
            }
        }
    }
}
