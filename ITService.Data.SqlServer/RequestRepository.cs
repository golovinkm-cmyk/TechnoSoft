using Data.Interfaces;
using Data.Interfaces;
using Domain;

namespace RepairService.Data.SqlServer;

public class RequestRepository : IRequestRepository
{
    private readonly RepairServiceDbContext _context;

    public RequestRepository(RepairServiceDbContext context)
    {
        _context = context;
    }

    public int Add(Request request)
    {
        _context.Requests.Add(request);
        _context.SaveChanges();
        return request.Id;
    }

    public Request GetById(int id)
    {
        return _context.Requests.Find(id);
    }

    public List<Request> GetAll()
    {
        return GetAll(RequestFilter.Empty);
    }

    public List<Request> GetAll(RequestFilter filter)
    {
        var query = _context.Requests.AsQueryable();

        if (filter.StartDate.HasValue)
        {
            query = query.Where(x => x.Date >= filter.StartDate.Value);
        }
        if (filter.EndDate.HasValue)
        {
            query = query.Where(x => x.Date <= filter.EndDate.Value);
        }

        return query.OrderByDescending(r => r.Date).ToList();
    }

    public bool Update(Request request)
    {
        var existing = _context.Requests.Find(request.Id);
        if (existing == null)
            return false;

        // Копируем свойства
        existing.Date = request.Date;
        existing.Tipe = request.Tipe;
        existing.Model = request.Model;
        existing.Description = request.Description;
        existing.Status = request.Status;
        existing.ClientFullName = request.ClientFullName;
        existing.ClientPhone = request.ClientPhone;
        existing.Engineer = request.Engineer;
        existing.Comments = request.Comments;

        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var request = _context.Requests.Find(id);
        if (request == null)
            return false;

        _context.Requests.Remove(request);
        _context.SaveChanges();
        return true;
    }
}