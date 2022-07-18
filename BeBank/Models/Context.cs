using Microsoft.EntityFrameworkCore;

namespace BeBank.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Deposit> deposits { get; set; }
        public DbSet<Withdraw> withdraws { get; set; }
        public DbSet<OTP> oTPs { get; set; }
        public DbSet<Contact> contacts { get; set; }

    }
    
}
