using Json.Schema;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;

var options = new JsonSerializerOptions();
options.Converters.Add(new ListJsonSchemaConverter());
options.Converters.Add(new DictionaryJsonSchemaConverter());
options.TypeInfoResolver = JsonTypeInfoResolver.Combine(new DefaultJsonTypeInfoResolver());

var schemaText = File.ReadAllText("schema.json");
var jsonSchema = JsonSchema.FromText(schemaText, options);

var schemaEvaluationOptions = EvaluationOptions.Default;
schemaEvaluationOptions.OutputFormat = OutputFormat.List;

var jsonData = File.ReadAllText("document.json");
var evaluationResult = jsonSchema.Evaluate(JsonNode.Parse(jsonData), schemaEvaluationOptions);

var errors = new List<SchemaValidationError>();
if (evaluationResult.HasDetails)
{
    foreach (var details in evaluationResult.Details)
    {
        if (details.Errors == null) continue;
        foreach (var error in details.Errors)
        {
            errors.Add(new SchemaValidationError
            {
                Name = error.Key,
                Value = error.Value,
                ErrorPath = details.InstanceLocation.ToString()
            });
        }
    }
}

Console.WriteLine($"{evaluationResult.IsValid} - {string.Join(", ", errors.Select(e => e.ToErrorMessage()))}");