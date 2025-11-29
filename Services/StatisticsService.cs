using Data.Interfaces;
using Domain.Statistics;
using System.Linq;

namespace Services
{

    public class StatusStatisticItem
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class MonthStatisticItem
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Count { get; set; }

        public string GetMonthName()
        {
            return new DateTime(Year, Month, 1).ToString("MMMM yyyy");
        }
    }

    public class EngineerStatisticItem
    {
        public string EngineerName { get; set; } = string.Empty;
        public int Count { get; set; }
    }
    public class StatisticsService
    {
        private readonly IRequestRepository _requestRepository;

        public StatisticsService(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        //
        public List<StatusStatisticItem> GetRequestsByStatus(RequestFilter filter)
        {
            var requests = _requestRepository.GetAll(filter);
            return requests
                .GroupBy(r => r.Status) 
                .Select(g => new StatusStatisticItem
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .OrderBy(s => s.Status)
                .ToList();
        }

        //
        public List<MonthStatisticItem> GetRequestsByMonth(RequestFilter filter)
        {
            var requests = _requestRepository.GetAll(filter);
            return requests
                //
                .GroupBy(r => new { r.Date.Year, r.Date.Month })
                .Select(g => new MonthStatisticItem
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(m => m.Year)
                .ThenBy(m => m.Month)
                .ToList();
        }

        //
        public List<EngineerStatisticItem> GetRequestsByEngineer(RequestFilter filter)
        {
            var requests = _requestRepository.GetAll(filter);
            return requests
                //
                .Where(r => !string.IsNullOrWhiteSpace(r.Engineer))
                .GroupBy(r => r.Engineer) //
                .Select(g => new EngineerStatisticItem
                {
                    EngineerName = g.Key, //
                    Count = g.Count()
                })
                .OrderByDescending(m => m.Count) //
                .ToList();
        }
    }
}