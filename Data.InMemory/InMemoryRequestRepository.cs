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
                //
                SeedData(); //
            }
        }

        //
        private void SeedData()
        {
            //
            var statuses = new[] { "Новая заявка", "В процессе ремонта", "Ожидание запчастей", "Готово к выдаче", "Завершена" };
            var engineers = new[] { "Петров С.А.", "Сидоров В.Н.", "Иванова Е.П.", "Кузнецов Д.И." };
            var clients = new[]
            {
                new { Name = "Иванов Иван Иванович", Phone = "+7 (903) 123-45-67" },
                new { Name = "Сидорова Елена Петровна", Phone = "+7 (926) 987-65-43" },
                new { Name = "Михайлов Артем Олегович", Phone = "+7 (916) 555-44-33" }
            };
            var models = new[]
            {
                ("Компьютер", "Dell OptiPlex 7010"),
                ("Принтер", "HP LaserJet Pro M404dn"),
                ("Ноутбук", "Lenovo ThinkPad X1"),
                ("Монитор", "Samsung Odyssey G7"),
                ("МФУ", "Kyocera Ecosys M2040dn")
            };

            var random = new Random();

            for (int i = 0; i < 50; i++) //
            {
                var client = clients[random.Next(clients.Length)];
                var model = models[random.Next(models.Length)];
                var status = statuses[random.Next(statuses.Length)];

                _requests.Add(new Request(
                    id: _nextId++,
                    date: DateTime.Now.AddDays(-random.Next(0, 180)).AddHours(random.Next(-12, 12)), //
                    tipe: model.Item1,
                    model: model.Item2,
                    description: $"Тестовая проблема {i + 1}",
                    status: status,
                    clientfullname: client.Name,
                    clientphone: client.Phone,
                    //
                    engineer: status == "Новая заявка" ? "" : engineers[random.Next(engineers.Length)],
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
            //
            return GetAll(RequestFilter.Empty);
        }

        public List<Request> GetAll(RequestFilter filter)
        {
            var result = _requests.AsEnumerable(); //

            if (filter.StartDate.HasValue)
            {
                //
                result = result.Where(x => x.Date.Date >= filter.StartDate.Value.Date);
            }
            if (filter.EndDate.HasValue)
            {
                //
                result = result.Where(x => x.Date.Date <= filter.EndDate.Value.Date);
            }

            return result.OrderByDescending(r => r.Date).ToList();
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