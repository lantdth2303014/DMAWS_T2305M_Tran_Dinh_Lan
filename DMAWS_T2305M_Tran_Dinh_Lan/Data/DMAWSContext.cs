using DMAWS_T2305M_Tran_Dinh_Lan.Models;
using Microsoft.EntityFrameworkCore;

namespace DMAWS_T2305M_Tran_Dinh_Lan.Data
{
    using Microsoft.EntityFrameworkCore;

    public class DMAWSContext : DbContext
    {
        public DMAWSContext(DbContextOptions<DMAWSContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite Key for ProjectEmployee
            modelBuilder.Entity<ProjectEmployee>()
                .HasKey(pe => new { pe.EmployeeId, pe.ProjectId });

            modelBuilder.Entity<Project>()
                .HasMany(p => p.ProjectEmployees)
                .WithOne(pe => pe.Projects);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.ProjectEmployees)
                .WithOne(pe => pe.Employees);
        }
    }

}