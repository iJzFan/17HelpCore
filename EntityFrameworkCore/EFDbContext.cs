using HELP.BLL.Entity;
using Microsoft.EntityFrameworkCore;

namespace HELP.BLL.EntityFrameworkCore
{
    public class EFDbContext:DbContext
    {
        public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Credit> Credit { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(e =>
            {
                e.HasIndex(x => x.Name);
                e.Property(x => x.CreateTime);
               e.Property(x => x.AuthCode).ValueGeneratedOnAdd();
            });

            builder.Entity<Problem>(e =>
            {
                e.HasIndex(x => x.CreateTime);
            });

            builder.Entity<Comment>(e =>
            {
                e.HasIndex(x => x.ProblemId);
                e.HasIndex(x => x.CreateTime);
            });

            builder.Entity<Credit>(e =>
            {
                e.HasIndex(x => x.CreateTime);
                e.Property(x => x.Balance);
            });

        }
    }
}
