using System.Text.Json.Serialization;
using System.Text.Json;

namespace channel_alert_front.Shared.HttpModels;

public interface IResType
{
    public string ToString();
}

[Serializable]
public class ResType<T> : IResType
{
    [JsonPropertyName("data")]
    public T? Data { get; set; } = default;

    public override string ToString()
    {
        return JsonSerializer.Serialize(Data);
    }
}
