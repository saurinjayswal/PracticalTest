using Microsoft.EntityFrameworkCore;
using PracticalTest.Models;

namespace PracticalTest.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        
        }
        public DbSet<ApplicationForm> applicationForms { get; set; }
        public DbSet<BasicDetails> basicDetails { get; set; }
        public DbSet<Education> educations { get; set; }
        public DbSet<Experience> experiences { get; set; }
        public DbSet<TechExpertise> techExpertises { get; set; }
        public DbSet<User> Users { get; set; }

        /*  protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
              modelBuilder.Entity<ApplicationForm>().ToTable("ApplicationForm");
          }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationForm>()
                .HasOne(a => a.BasicDetails)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade); // Example of cascading delete

            // Configure other relationships similarly
        }

    }

}
