using Microsoft.EntityFrameworkCore;

namespace Messages
{
    public class MessagesDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public MessagesDbContext(DbContextOptions<MessagesDbContext> configuration)
            : base(configuration)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasNoKey()
                .Property(m => m.Value)
                .IsRequired();
        }
    }
}