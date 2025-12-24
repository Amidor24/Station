using System.Text.Json.Serialization;

namespace Api.Dtos
{
    public class GrafanaResponse
    {
        [JsonPropertyName("results")]
        public Results Results { get; set; }
    }

    public class Results
    {
        [JsonPropertyName("A")]
        public ResultA A { get; set; }
    }

    public class ResultA
    {
        [JsonPropertyName("frames")]
        public List<Frame> Frames { get; set; }
    }

    public class Frame
    {
        [JsonPropertyName("schema")]
        public Schema Schema { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class Schema
    {
        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }

        [JsonPropertyName("fields")]
        public List<Field> Fields { get; set; }
    }

    public class Meta
    {
        [JsonPropertyName("executedQueryString")]
        public string ExecutedQueryString { get; set; }
    }

    public class Field
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("typeInfo")]
        public TypeInfo TypeInfo { get; set; }
    }

    public class TypeInfo
    {
        [JsonPropertyName("frame")]
        public string Frame { get; set; }

        [JsonPropertyName("nullable")]
        public bool Nullable { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("values")]
        public List<List<object>> Values { get; set; }
    }
}
