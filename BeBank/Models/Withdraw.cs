namespace BeBank.Models
{
    public class Withdraw
    {
        public int Id { get; set; }
        public int WithdrawnBalance { get; set; }
        public DateTime Date { get; set; }

        public int userId { get; set; }
        public User user { get; set; }
    }
}
