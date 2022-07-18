namespace BeBank.Models
{
    public class Deposit
    {
        public int Id { get; set; }
        public string DepositedBalance { get; set; }
        public DateTime Date { get; set; }

        public int userId { get; set; }
        public User user { get; set; }
    }
}
