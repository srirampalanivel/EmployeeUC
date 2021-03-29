using EmployeeUC.WebAPI.Domain.Models;
using EmployeeUC.WebAPI.Context;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmployeeUC.WebAPI.DAL.Queries
{
    public class GetEmployeeByQuery : IRequest<IEnumerable<Employee>>
    {
        public int Id { get; set; }
        public int? Department { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByQuery, IEnumerable<Employee>>
        {
            private readonly IApplicationContext _context;
            public GetEmployeeByIdQueryHandler(IApplicationContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Employee>> Handle(GetEmployeeByQuery query, CancellationToken cancellationToken)
            {
                var employeesList =  _context.Employees.Where(a => ((query.Id != null ? a.Id == query.Id : true) &&
                (query.Department.Value != null ? a.DepartmentId == query.Department.Value : true) && (query.FirstName != null ? a.FirstName == query.FirstName : true) &&
                (query.LastName != null ? a.LastName == query.LastName : true)));
                if (employeesList == null)
                {
                    return null;
                }
                return employeesList.OrderBy(a => a.FirstName);
            }
        }
    }
}
