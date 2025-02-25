using AutoMapper;
using channel_alert_front.ApiService.Repository;

namespace channel_alert_front.ApiService.Services;

public interface IServiceManager
{
    IUserService UserService { get; }
    IChartService ChartService { get; }
}


public class ServiceManager : IServiceManager
{
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<IChartService> _chartService;


    public ServiceManager(IRepositoryManager repository, IMapper mapper, ILoggerManager logger, JwtService jwtService)
    {
        _userService = new(() => new UserService(repository, mapper, logger, jwtService));
        _chartService = new(() => new ChartService(repository, mapper, logger));
    }

    public IUserService UserService => _userService.Value;

    public IChartService ChartService => _chartService.Value;
}
