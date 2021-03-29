using EmployeeUC.WebAPI.Domain.Models;
using EmployeeUC.WebAPI.Context;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmployeeUC.WebAPI.DAL.Queries
{
    public class GetEmployeeStatusQuery : IRequest<string>
    {
        public int Id { get; set; }

        public class GetEmployeeStatusQueryHandler : IRequestHandler<GetEmployeeStatusQuery, string>
        {
            private readonly IApplicationContext _context;
            public GetEmployeeStatusQueryHandler(IApplicationContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(GetEmployeeStatusQuery query, CancellationToken cancellationToken)
            {
                string status = string.Empty;
                var employeesList = _context.Employees.Where(a => a.ManagerId == query.Id );
                if (employeesList == null)
                {
                    status = "Associate";
                    return status;
                }
                else
                {
                    List<int> empIDs = employeesList.Select(o => o.Id).ToList();
                    var managerList = (from employee in _context.Employees
                     where (empIDs.Contains(employee.ManagerId.Value))
                     select employee).ToList();
                    if (managerList == null)
                    {
                        status = "Manager";
                        return status;
                    }
                    else 
                    {
                        status = "Head";
                        return status;
                    }
                }
                return status;
            }
        }
    }
}
