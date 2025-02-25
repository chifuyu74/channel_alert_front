using channel_alert_front.ApiService.Attributes;
using channel_alert_front.ApiService.Services;
using channel_alert_front.Shared.DataTransferObject;
using channel_alert_front.Shared.HttpModels;
using Microsoft.AspNetCore.Mvc;

namespace channel_alert_front.ApiService.Controllers;

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
}