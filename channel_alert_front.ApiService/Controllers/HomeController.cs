using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using channel_alert_front.ApiService.Attributes;

namespace channel_alert_front.ApiService.Controllers;

[Auth, ApiController, Route("[controller]")]
public class HomeController : Controller
{
    [AllowAnonymous, HttpGet]
    public IActionResult Index()
    {
        return Ok("Welcome! I am Alive!");
    }
}
