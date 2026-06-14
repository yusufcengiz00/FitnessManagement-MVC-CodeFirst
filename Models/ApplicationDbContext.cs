using Microsoft.EntityFrameworkCore;

namespace Project1.Models
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>
            dbcontext) : base(dbcontext)
        {

        }

        public DbSet<Uye> uyes { get; set; }
        public DbSet<Antrenor> antrenors { get; set; }

        public DbSet<Salon> salons { get; set; }

        public DbSet<Supplement> supplements { get; set; }
    }
}
