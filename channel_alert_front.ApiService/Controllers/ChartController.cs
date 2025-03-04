using channel_alert_front.ApiService.Attributes;
using channel_alert_front.ApiService.Services;
using channel_alert_front.Shared.DataTransferObject;
using channel_alert_front.Shared.HttpModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Channels;

namespace channel_alert_front.ApiService.Controllers;

public class Tasks
{
    public int id { get; set; }
}

[Auth, Route("[controller]"), ApiController]
public class ChartController : ControllerBase
{
    private readonly IServiceManager _service;

    public ChartController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Welcome, Chart!");
    }

    [HttpGet("histories")]
    public IActionResult GetAllHistories()
    {
        IEnumerable<AlertHistoryDto> histories = _service.ChartService.GetAllHistories();

        ResType<List<AlertHistoryDto>> res = new() { Data = histories.ToList() };

        return Ok(res);
    }

    IEnumerable<int> Generator()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return i;
        }
    }

    [HttpGet("sse"), AllowAnonymous]
    public async Task InjectStreamHistory(CancellationToken cancellationToken)
    {
        HttpContext.Response.Headers.Append(HeaderNames.ContentType, "text/event-stream");
        int count = Int32.MaxValue;

        await HttpContext.Response.WriteAsync($"event: custom\r");
        await HttpContext.Response.WriteAsync($"data: custom event data\r\r");
        await HttpContext.Response.Body.FlushAsync();

        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1000);
            var message = await chan.Reader.ReadAsync();
            Console.WriteLine($"Consumer: {message.id}");
            //Console.WriteLine($"sending message: {message.id}");
            await HttpContext.Response.WriteAsync($"data: {count}/id => {message.id}\n\n");
            await HttpContext.Response.Body.FlushAsync();
            count--;
        }

        await HttpContext.Response.CompleteAsync();
    }
    static Channel<Tasks> chan { get; set; } = Channel.CreateUnbounded<Tasks>();
    static int global { get; set; } = 0;

    [HttpPut("sse"), AllowAnonymous]
    public IActionResult AddItem()
    {
        global += 1;
        Tasks newItem = new() { id = global };
        //Console.WriteLine(newItem.id);
        bool wrote = chan.Writer.TryWrite(newItem);
        //bool error = chan.Writer.TryComplete();
        //chan.Writer.Complete();
        Console.WriteLine($"Producer: {wrote} / {chan.Reader.Count} / {newItem.id}");
        
        return Accepted();
    }
}