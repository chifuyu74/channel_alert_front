using AutoMapper;
using channel_alert_front.ApiService.Entities;
using channel_alert_front.ApiService.Repository;
using channel_alert_front.Shared.DataTransferObject;

namespace channel_alert_front.ApiService.Services;

public interface IChartService
{
    IEnumerable<AlertHistoryDto> GetAllHistories();
}

public class ChartService : IChartService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public ChartService(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public IEnumerable<AlertHistoryDto> GetAllHistories()
    {
        IEnumerable<AlertHistory> histories = _repository.Chart.GetAllHistories();
        var dtos = _mapper.Map<IEnumerable<AlertHistoryDto>>(histories);
        return dtos;
    }
}
