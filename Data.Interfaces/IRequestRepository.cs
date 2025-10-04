
using System.Collections.Generic;
using Domain;

namespace Data.Interfaces
{
    // Интерфейс репозитория для работы с сущностью Request
    public interface IRequestRepository
    {
        // Добавляет новую заявку и возвращает ее сгенерированный ID
        int Add(Request request);

        // Получает заявку по ID
        Request GetById(int id);

        // Получает весь список заявок
        List<Request> GetAll();

        // Обновляет существующую заявку. Возвращает true, если обновление прошло успешно.
        bool Update(Request request);

        // Удаляет заявку по ID. Возвращает true, если удаление прошло успешно.
        bool Delete(int id);
    }
}