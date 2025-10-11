using System.Collections.Generic;
using System.Linq;
using Domain;
using Data.Interfaces;

namespace Data.InMemory
{
    public class InMemoryRequestRepository : IRequestRepository
    {
       
        private static readonly List<Request> _requests = new List<Request>();

        
        private static int _nextId = 1;

        public InMemoryRequestRepository()
        {
            
            if (!_requests.Any())
            {
                _requests.Add(new Request(
                    id: _nextId++,
                    date: System.DateTime.Now.AddDays(-5),
                    tipe: "Компьютер",
                    model: "Dell OptiPlex 7010",
                    description: "Не включается, горит оранжевый индикатор",
                    status: "В процессе ремонта",
                    clientfullname: "Иванов Иван Иванович",
                    clientphone: "+7 (903) 123-45-67",
                    engineer: "Петров С.А.",
                    comments: "Требуется замена блока питания."
                ));
                _requests.Add(new Request(
                    id: _nextId++,
                    date: System.DateTime.Now.AddDays(-1),
                    tipe: "Принтер",
                    model: "HP LaserJet Pro M404dn",
                    description: "Заминает бумагу при печати",
                    status: "Новая заявка",
                    clientfullname: "Сидорова Елена Петровна",
                    clientphone: "+7 (926) 987-65-43",
                    engineer: "",
                    comments: ""
                ));
            }
        }

        public int Add(Request request)
        {
            
            request.Id = _nextId++;
            request.Date = System.DateTime.Now;
            _requests.Add(request);
            return request.Id;
        }

        public Request GetById(int id)
        {
            return _requests.FirstOrDefault(r => r.Id == id);
        }

        public List<Request> GetAll()
        {
            
            return _requests.OrderByDescending(r => r.Date).ToList();
        }

        public bool Update(Request updatedRequest)
        {
            var existingRequest = _requests.FirstOrDefault(r => r.Id == updatedRequest.Id);

            if (existingRequest == null)
                return false;

            
            existingRequest.Date = updatedRequest.Date;
            existingRequest.Tipe = updatedRequest.Tipe;
            existingRequest.Model = updatedRequest.Model;
            existingRequest.Description = updatedRequest.Description;
            existingRequest.Status = updatedRequest.Status;
            existingRequest.ClientFullName = updatedRequest.ClientFullName;
            existingRequest.ClientPhone = updatedRequest.ClientPhone;
            existingRequest.Engineer = updatedRequest.Engineer;
            existingRequest.Comments = updatedRequest.Comments;

            return true;
        }

        public bool Delete(int id)
        {
            var requestToDelete = _requests.FirstOrDefault(r => r.Id == id);
            if (requestToDelete != null)
            {
                _requests.Remove(requestToDelete);
                return true;
            }
            return false;
        }
    }
}