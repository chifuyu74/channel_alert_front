using channel_alert_front.ApiService.DB;
using channel_alert_front.ApiService.Entities;

namespace channel_alert_front.ApiService.Repository;

public interface IChartRepository
{
    IEnumerable<AlertHistory> GetAllHistories();
}

public class ChartRepository : RepositoryBase<AlertHistory>, IChartRepository
{
    public ChartRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public IEnumerable<AlertHistory> GetAllHistories()
    {
        return FindAll().OrderByDescending(history => history.OnAlerted);
    }
}
