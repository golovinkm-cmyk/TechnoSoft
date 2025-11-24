using Data.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ITService.Data.SqlServer
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ITServiceDbContext _context;

        public RequestRepository(ITServiceDbContext context)
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
            return _context.Requests
                .OrderByDescending(r => r.Date)
                .ToList();
        }

        public List<Request> GetAll(RequestFilter filter)
        {
            var query = _context.Requests.AsQueryable();

            if (filter.StartDate.HasValue)
            {
                query = query.Where(x => x.Date.Date >= filter.StartDate.Value.Date);
            }
            if (filter.EndDate.HasValue)
            {
                query = query.Where(x => x.Date.Date <= filter.EndDate.Value.Date);
            }

            return query.OrderByDescending(r => r.Date).ToList();
        }

        public bool Update(Request updatedRequest)
        {
            var existingRequest = _context.Requests.Find(updatedRequest.Id);
            if (existingRequest == null)
                return false;

            // Обновляем свойства
            existingRequest.Date = updatedRequest.Date;
            existingRequest.Tipe = updatedRequest.Tipe;
            existingRequest.Model = updatedRequest.Model;
            existingRequest.Description = updatedRequest.Description;
            existingRequest.Status = updatedRequest.Status;
            existingRequest.ClientFullName = updatedRequest.ClientFullName;
            existingRequest.ClientPhone = updatedRequest.ClientPhone;
            existingRequest.Engineer = updatedRequest.Engineer;
            existingRequest.Comments = updatedRequest.Comments;

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
}
