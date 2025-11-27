using System;

namespace Domain
{
    public class Request
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Tipe { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ClientFullName { get; set; } = string.Empty;
        public string ClientPhone { get; set; } = string.Empty;
        public string Engineer { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;

        public Request()
        {
        }

        public Request(int id, DateTime date, string tipe, string model, string description,
                      string status, string clientfullname, string clientphone,
                      string engineer, string comments)
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