using System.Text.Json;

namespace Movies.Client.Helpers;

public class JsonSerializerOptionsWrapper
{
    public JsonSerializerOptions options { get; }

    public JsonSerializerOptionsWrapper()
    {
        options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }
}
