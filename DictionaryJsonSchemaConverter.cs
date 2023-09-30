using Json.More;
using Json.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DictionaryJsonSchemaConverter : JsonConverter<Dictionary<string, JsonSchema>>
{
    public override bool HandleNull => true;

    public override Dictionary<string, JsonSchema> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected object");

        var dictionary = new Dictionary<string, JsonSchema>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return dictionary;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("JsonTokenType was not PropertyName");
            }

            var propertyName = reader.GetString();
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new JsonException("Failed to get property name");
            }

            reader.Read();
            var schema = options.Read<JsonSchema>(ref reader)!;

            dictionary.Add(propertyName!, schema!);
        }

        return dictionary;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, JsonSchema> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}