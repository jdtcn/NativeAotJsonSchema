using Json.More;
using Json.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

public class ListJsonSchemaConverter : JsonConverter<List<JsonSchema>>
{
    public override bool HandleNull => true;

    public override List<JsonSchema> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Expected array");

        var list = new List<JsonSchema>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return list;
            }

            var schema = options.Read<JsonSchema>(ref reader)!;

            list.Add(schema!);
        }

        return list;
    }

    public override void Write(Utf8JsonWriter writer, List<JsonSchema> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}