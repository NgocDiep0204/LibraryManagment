using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using api_librarymanagment.Models.Library;
using Microsoft.AspNetCore.Identity;
using static System.Reflection.Metadata.BlobBuilder;
using api_librarymanagment.Models.Login;

namespace api_librarymanagment.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options){ }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<PenaltyDetail> PenaltyDetails { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Publicator> Publicators { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<RentDetail> RentDetails { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedRoles(modelBuilder);

            modelBuilder.Entity<RentDetail>()
                .HasKey(rd => new { rd.RentId, rd.BookId });
            modelBuilder.Entity<RentDetail>()
               .HasOne(rd => rd.RentIdNavigation)
               .WithMany(r => r.RentDetails)
               .HasForeignKey(rd => rd.RentId);
            modelBuilder.Entity<PenaltyDetail>()
              .HasKey(rd => new { rd.PenaltyId, rd.BookId });
            modelBuilder.Entity<PenaltyDetail>()
                .HasOne(pd => pd.PenaltyIdNavigation)
                .WithMany(p => p.PenaltyDetails)
                .HasForeignKey(pd => pd.PenaltyId);

            modelBuilder.Entity<Penalty>()
                .HasOne(p => p.UserIdNavigation)  // Định nghĩa mối quan hệ
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<RefreshToken>()
            .HasKey(rt => rt.Id);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .IsRequired();
        }
        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
                );
        }
    }
}
