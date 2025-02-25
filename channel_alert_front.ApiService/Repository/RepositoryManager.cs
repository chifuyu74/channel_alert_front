using channel_alert_front.ApiService.DB;

namespace channel_alert_front.ApiService.Repository;

public interface IRepositoryManager
{
    IUserRepository User { get; }
    IChartRepository Chart { get; }

    public void Save();
}

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IChartRepository> _chartRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
        _chartRepository = new Lazy<IChartRepository>(() => new ChartRepository(repositoryContext));
    }

    public IUserRepository User => _userRepository.Value;
    public IChartRepository Chart => _chartRepository.Value;

    public void Save()
    {
        _repositoryContext.SaveChanges();
    }
}
