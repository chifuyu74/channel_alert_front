namespace channel_alert_front.Web.Defines;

// Add Scoped Service
public class HttpRequests
{
    // TODO : when Instantiate, Authmatically Insert URL with HTTP Method (e.g GET/POST)
    public static Dictionary<ERequestUrl, string> RequestUrl = new()
    {
        { ERequestUrl.Auth_Login, "auth/login" },
        { ERequestUrl.Auth_Refresh, "auth/refresh" },
    };

    public static string GenerateBaseUrl(ERequestUrl type)
    {
        string baseUrl = Constant.Url["Development"];
        string resourceBaseUrl = RequestUrl[type];
        string requestBaseUrl = string.Join("/", baseUrl, resourceBaseUrl);

        return requestBaseUrl;
    }

    // TODO: Create HTTP Client Request Util Function
}
