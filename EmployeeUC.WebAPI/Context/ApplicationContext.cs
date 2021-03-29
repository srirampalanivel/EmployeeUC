using EmployeeUC.WebAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeUC.WebAPI.Context
{
    public class ApplicationContext : DbContext, IApplicationContext
    {

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            //modelBuilder.Entity<Employee>(a =>
            //{
            //    a.HasIndex(x => x.ManagerId).IsUnique(true);
            //});

            modelBuilder.Entity<Employee>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Employee>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd();


            modelBuilder.Entity<Employee>()
            .HasOne<Employee>(a => a.Manager)
            .WithOne()
            .HasForeignKey<Employee>(a => a.ManagerId);

            //Seed Data
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, DepartmentName = "HealthCare" },
                new Department { DepartmentId = 2, DepartmentName = "DMV" },
                new Department { DepartmentId = 3, DepartmentName = "Finance" }
            );

        }

    }
}
