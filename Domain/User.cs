namespace Domain
{
    public class User
    {
        public string FIO { get; set; } = string.Empty;
        public string Phone_Number { get; set; } = string.Empty;


        public User()
        {
        }

        public User(string fio, string phone_number)
        {
            FIO = fio;
            Phone_Number = phone_number;
        }
    }
}