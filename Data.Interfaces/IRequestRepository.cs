
using System.Collections.Generic;
using Domain;

namespace Data.Interfaces
{
    
    public interface IRequestRepository
    {

        int Add(Request request);
        Request GetById(int id);

        List<Request> GetAll();
        List<Request> GetAll(RequestFilter filter); 

        bool Update(Request request);
        bool Delete(int id);
    }
}