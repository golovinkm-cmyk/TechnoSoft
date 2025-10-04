
using System;

namespace Domain
{
    public class Request
    {
        // »зменено: Id теперь int дл€ автогенерации и поиска
        public int Id { get; set; }
        public DateTime Date { get; set; }
        // ќставлено Tipe, чтобы соответствовать XAML, хот€ Type было бы более идиоматично
        public string Tipe { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string ClientFullName { get; set; }
        public string ClientPhone { get; set; }
        public string Engineer { get; set; }
        public string Comments { get; set; }

        //  онструктор по умолчанию (об€зателен дл€ WPF)
        public Request()
        {
        }

        //  онструктор с параметрами (Id теперь int)
        public Request(int id, DateTime date, string tipe, string model, string description, string status, string clientfullname,
            string clientphone, string engineer, string comments)
        {
            Id = id;
            Date = date;
            Tipe = tipe;
            Model = model;
            Description = description;
            Status = status;
            ClientFullName = clientfullname;
            ClientPhone = clientphone;
            Engineer = engineer;
            Comments = comments;
        }
    }
}