public class SchemaValidationError
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? ErrorPath { get; set; }

    public string ToErrorMessage()
    {
        return string.IsNullOrEmpty(ErrorPath) ? Value : $"{ErrorPath}: {Value}";
    }
}
