using EmployeeUC.WebAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EmployeeUC.WebAPI.Context
{
    public interface IApplicationContext
    {
        DbSet<Employee> Employees { get; set; }
        DbSet<Department> Departments { get; set; }

        Task<int> SaveChangesAsync();
    }
}