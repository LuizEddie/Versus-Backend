using System.Text.Json;
using System.Text.Json.Serialization;

namespace Versus.Helpers
{
    public class TimeSpanConverter : JsonConverter<TimeSpan?>
    {
        public override TimeSpan? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String) {
                var value = reader.GetString();

                if(TimeSpan.TryParse(value, out var ts)) return ts;
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan? value, JsonSerializerOptions options)
        {
            if (value.HasValue) { 
                writer.WriteStringValue(value.Value.ToString(@"hh\:mm\:ss"));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
