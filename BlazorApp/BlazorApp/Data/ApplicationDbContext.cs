using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Data
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
	{
		public DbSet<UserAddress> UserAddresses { get; set; }
		public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserCourses> UserCourses { get; set; }
        public DbSet<UserMessages> UserMessages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCourses>()
                .HasKey(uc => new { uc.UserId, uc.CourseId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
