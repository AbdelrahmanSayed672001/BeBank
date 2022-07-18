namespace BeBank.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public int? Balance { get; set; }
        public int? WithDrawn { get; set; }
        public int? Deposited { get; set; }
    }
}
