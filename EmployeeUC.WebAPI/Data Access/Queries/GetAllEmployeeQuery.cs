using EmployeeUC.WebAPI.Domain.Models;
using EmployeeUC.WebAPI.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace EmployeeUC.WebAPI.DAL.Queries
{
    public class GetAllEmployeesQuery : IRequest<IEnumerable<Employee>>
    {

        public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery,IEnumerable<Employee>>
        {
            private readonly IApplicationContext _context;
            public GetAllEmployeesQueryHandler(IApplicationContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Employee>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
            {
                var employeesList = await _context.Employees.ToListAsync();
                if (employeesList == null)
                {
                    return null;
                }
                return employeesList.OrderBy(a => a.FirstName);
            }
        }
    }
}
