using channel_alert_front.ApiService.Entities;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Mime;
using System.Text;

namespace channel_alert_front.ApiService.Formatter;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeNames.Text.Csv);
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        if (typeof(IEnumerable<object>).IsAssignableFrom(type))
            return base.CanWriteType(type);

        return true;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        HttpResponse response = context.HttpContext.Response;
        StringBuilder builder = new();

        Type? type = context.ObjectType;
        if (type == null)
            await response.WriteAsync(string.Empty);

        if (context.Object is IEnumerable<object> list)
        {
            foreach (var item in list)
            {
                if (item is null)
                    continue;

                FormatCsv(builder, item.GetType());
            }
        }
        else
        {
            FormatCsv(builder, context.ObjectType!);
        }

        await response.WriteAsync(builder.ToString());
    }

    private static void FormatCsv(StringBuilder builder, Type type)
    {
        builder.AppendLine($"{type.ToString()}");
    }
}
