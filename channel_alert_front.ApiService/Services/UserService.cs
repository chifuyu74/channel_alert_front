using AutoMapper;
using channel_alert_front.ApiService.Entities;
using channel_alert_front.ApiService.Repository;
using channel_alert_front.Shared.DataTransferObject;
using channel_alert_front.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace channel_alert_front.ApiService.Services;

public interface IUserService
{
    public IEnumerable<UserDto> GetAllUsers(bool trackChanges = true);
    public Task<bool> Create(string email, string password);
    public Task<bool> DeleteAsync(string email);
    public Task<int> UpdateToken(string email);
    public LoginResponseModel? Login(LoginRequestModel request);
    public Task<bool> RevokeToken(string token);
    public Task<LoginResponseModel?> RefreshToken(string accessToken, string refreshToken);
}

public class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    private readonly JwtService _jwtService;

    public UserService(IRepositoryManager repository, IMapper mapper, ILoggerManager logger, JwtService jwtService)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _jwtService = jwtService;
    }

    public IEnumerable<UserDto> GetAllUsers(bool trackChanges = true)
    {
        var users = _repository.User.GetAllUsers();
        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
        return userDtos;
    }

    public async Task<bool> Create(string email, string password)
    {
        User user = new() { Email = email };

        PasswordHasher<User> hasher = new();
        string hash = hasher.HashPassword(user, password);
        user.PasswordHash = hash;

        return await _repository.User.CreateAsync(user);
    }

    public async Task<bool> DeleteAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        int deleted = await _repository.User.DeleteAsync(email);
        return deleted > 0;
    }

    public async Task<int> UpdateToken(string email)
    {
        if (string.IsNullOrEmpty(email))
            return 0;

        User? user = _repository.User.FindByCondition((u) => u.Email == email).FirstOrDefault();
        if (user == null)
            return 0;

        //string token = _jwtService.GenerateToken();

        int updated = await _repository.User.UpdateToken(email, "");
        return updated;
    }

    public LoginResponseModel? Login(LoginRequestModel request)
    {
        User? foundUser = _repository.User.FindByCondition((u) => u.Email == request.Email).FirstOrDefault();
        if (foundUser == null || string.IsNullOrEmpty(foundUser.PasswordHash))
            return null;

        PasswordHasher<User> hasher = new();
        PasswordVerificationResult result = hasher.VerifyHashedPassword(foundUser, foundUser.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            return null;

        List<Claim> claims = _jwtService.GenerateDefaultClaims(request.Email);
        string token = _jwtService.GenerateToken(claims);
        string refreshToken = _jwtService.GenerateRefreshToken();

        Claim? roleClaim = claims.Find((claim) => claim.Type == ClaimTypes.Role);
        if (roleClaim == null)
            return null;

        if (!foundUser.Roles.Contains(roleClaim.Value))
            foundUser.Roles.Add(roleClaim.Value);

        foundUser.LastLoggedIn = DateTime.Now;
        foundUser.RefreshToken = refreshToken;
        _repository.User.Save();

        return new() { Access = token, Refresh = refreshToken };
    }

    public async Task<bool> RevokeToken(string token)
    {
        ClaimsPrincipal principal = _jwtService.GetPrincipalFromExpiredToken(token);
        Claim? claim = principal.Claims.ToList().Find((claim) =>
        {
            return claim.Type == ClaimTypes.Name;
        });
        if (claim == null)
            return false;

        int updated = await _repository.User.UpdateToken(claim.Value, null);
        return updated > 0;
    }

    public async Task<LoginResponseModel?> RefreshToken(string accessToken, string refreshToken)
    {
        var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);

        string newAccessToken = _jwtService.GenerateToken(principal.Claims);
        string newRefreshToken = _jwtService.GenerateRefreshToken();

        Claim? emailClaim = principal.Claims.ToList().Find((claim) =>
        {
            return claim.Type == ClaimTypes.Email;
        });

        if (emailClaim == null)
            return null;

        int updated = await _repository.User.UpdateToken(emailClaim.Value, newRefreshToken);
        if (updated <= 0)
            return null;

        LoginResponseModel response = new() { Access = newAccessToken, Refresh = newRefreshToken };

        return response;
    }
}
