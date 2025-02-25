using System.Net.Mime;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using channel_alert_front.ApiService.Services;
using channel_alert_front.Shared.Models;
using channel_alert_front.Shared.HttpModels;
using channel_alert_front.ApiService.Attributes;
using Microsoft.Extensions.Primitives;

namespace channel_alert_front.ApiService.Controllers;

/// <summary>
/// Authentication Router
/// </summary>
[Auth, Route("[controller]"), ApiController, Consumes(MediaTypeNames.Application.Json)]
public class AuthController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly IServiceManager _service;

    public AuthController(JwtService jwtService, ILoggerManager logger, IServiceManager service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromQuery(Name = "email")] string email, [FromQuery(Name = "password")] string password)
    {
        _logger.LogInfo($"email: {email} / password: {password}");

        bool created = await _service.UserService.Create(email, password);

        ResType<bool> res = new() { Data = created };

        return Ok(res);
    }

    [HttpPost("users")]
    public IActionResult FindAll()
    {
        var users = _service.UserService.GetAllUsers().ToList();

        return new JsonResult(users);
    }

    [HttpDelete("user")]
    async Task<IActionResult> Delete([FromQuery(Name = "email")] string email)
    {
        bool deleted = await _service.UserService.DeleteAsync(email);
        ResType<bool> res = new() { Data = deleted };
        if (deleted)
        {
            return Ok(res);
        }

        return NotFound();
    }

    [AllowAnonymous, HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromQuery(Name = "refresh")] string refreshToken)
    {
        string? authorization = HttpContext.Request.Headers.Authorization;
        if (string.IsNullOrEmpty(authorization))
            return BadRequest();

        string[] splitHeader = authorization.Split(" ");
        if (splitHeader.Length <= 1)
            return BadRequest();

        string accessToken = splitHeader[^1];
        if (string.IsNullOrEmpty(accessToken))
            return BadRequest();

        LoginResponseModel? model = await _service.UserService.RefreshToken(accessToken, refreshToken);
        if (model == null)
            return BadRequest();
        
        ResType<LoginResponseModel> res = new() { Data = model };

        return Ok(res);
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeToken()
    {
        bool success = false;
        ResType<bool> res = new() { Data = success };
        StringValues header = HttpContext.Request.Headers.Authorization;
        if (string.IsNullOrEmpty(header))
            return BadRequest();


        string? token = header.ToString().Split(" ")[^1];
        if (string.IsNullOrEmpty(token))
            return BadRequest();
        
        success = await _service.UserService.RevokeToken(token);

        return Ok(res);
    }

    [AllowAnonymous, HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestModel request)
    {
        LoginResponseModel? tokens = _service.UserService.Login(request);

        ResType<LoginResponseModel> response = new() { Data = tokens };
        if (tokens == null)
            return Unauthorized(response);

        return Ok(response);
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        _logger.LogInfo("Here is info message from our values controller.");
        _logger.LogDebug("Here is debug message from our values controller.");
        _logger.LogWarn("Here is warn message from our values controller.");
        _logger.LogError("Here is an error message from our values controller.");

        return Ok(new { a = 1 });
    }
}
