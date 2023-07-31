using System.Text.Json;

namespace OpenCredentialPublisher.ClrLibrary.Extensions
{
    public class TWJson
    {
        public static T Deserialize<T>(string value) where T : class
        {
            if (string.IsNullOrEmpty(value))
            {
                return (T)null;
            }
            else
            {
                return JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions { IgnoreNullValues = true });
            }
        }
        public static JsonSerializerOptions IgnoreNulls
        {
            get
            {
                return new JsonSerializerOptions { IgnoreNullValues = true };
            }
        }
    }
}
