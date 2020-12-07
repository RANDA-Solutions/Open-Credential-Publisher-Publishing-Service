using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenCredentialsPublisher.Credentials.Clrs.Clr
{
    public class EnumExtStringConverter<T> : JsonConverter where T: Enum
    {
        private const string Pattern = "(ext:)[a-z|A-Z|0-9|.|-|_]+";
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(String));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            var enumType = typeof(T);
            if (enumType.IsEnum)
            {
                if (token.Type == JTokenType.String)
                {
                    var enumValues = Enum.GetNames(enumType);
                    var value = token.Value<string>();
                    if (enumValues.Contains(value) || Regex.IsMatch(value, Pattern))
                    {
                        return value;
                    }
                }
            }

            return "InvalidValue";
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var enumType = typeof(T);
            var enumValues = Enum.GetNames(enumType);

            var valueString = (String)value;
            if (enumValues.Contains(valueString) || Regex.IsMatch(valueString, Pattern))
            {
                serializer.Serialize(writer, value);
            }
            else
            {
                serializer.Serialize(writer, "InvalidValue");
            }
        }
    }
}
