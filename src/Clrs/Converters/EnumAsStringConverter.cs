using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenCredentialsPublisher.Credentials.Clrs.Converters
{
    public class EnumAsStringConverter<T> : JsonConverter where T : Enum
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            var enumType = typeof(T);
            if (enumType.IsEnum)
            {
                if (token.Type == JTokenType.String)
                {
                    var value = token.Value<string>();
                    if (Enum.TryParse(typeof(T), value, out var result))
                        return result;
                }
            }
            return "InvalidValue";
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var names = Enum.GetNames(typeof(T));
            var stringValue = value.ToString();
            if (names.Contains(stringValue))
            {
                serializer.Serialize(writer, stringValue);
            }
        }
    }
}
