using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace channel_alert_front.Web.Defines;

public enum AppEnums
{
    MESSAGES_NOTIFICATION_CENTER,
    MESSAGES_UNUSED,
}

public enum ETokenType
{
    Access,
    Refresh,
}

public enum EStorageType
{
    Local,
    Session,
}

public enum EStorageOperationType
{
    Get,
    Set,
    Remove,
}

public enum EPageType
{
    Index,
    Login,
    Home,
    Dashboard,
}

public enum ERequestUrl
{
    Auth_Login,
    Auth_Refresh,
    Auth_Revoke,
    Chart,
}

public static class ExtensionObject
{
    public const string StorageObjectName = "storage";

    public static async Task<string?> StorageAsync(this IJSRuntime js, EStorageType type, EStorageOperationType opType, string key, params object?[]? args)
    {
        string funcName = $"{StorageObjectName}.{type.ToString().ToLower()}.{opType.ToString().ToLower()}Storage";

        return await js.InvokeAsync<string?>(funcName, key, args);
    }

    public static void NavigatePage(this NavigationManager navigationManager, EPageType type)
    {
        string razorPage = $"/{type.ToString().ToLower()}";
        navigationManager.NavigateTo(razorPage);
    }
}