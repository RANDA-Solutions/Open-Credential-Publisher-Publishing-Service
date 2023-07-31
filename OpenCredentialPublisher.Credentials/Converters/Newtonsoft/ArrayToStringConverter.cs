using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCredentialPublisher.Credentials.Converters.Newtonsoft
{
    public class ArrayToStringConverter : JsonConverter<string>
    {

        public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
            {
                var list = token.ToObject<List<string>>();
                return String.Join(",", list);
            }

            return token.ToString();
        }

        public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
        {
            if (value != null)
            {
                if (value.Contains(","))
                {
                    var list = value.Split(",").ToList();
                    serializer.Serialize(writer, list);
                }
                else
                {
                    serializer.Serialize(writer, value);
                }
            }
        }
    }
}
