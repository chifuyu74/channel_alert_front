using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Mime;

namespace channel_alert_front.ApiService.Formatter;

public class ByteArrayInputFormatter : InputFormatter
{
    public ByteArrayInputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeNames.Application.Octet);
    }

    protected override bool CanReadType(Type type)
    {
        return typeof(byte[]).Equals(type);
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
    {
        MemoryStream stream = new();
        await context.HttpContext.Request.Body.CopyToAsync(stream);
        InputFormatterResult result = await InputFormatterResult.SuccessAsync(stream.ToArray());
        return result;
    }
}
