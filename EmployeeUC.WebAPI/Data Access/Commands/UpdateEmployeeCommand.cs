using EmployeeUC.WebAPI.Context;
using MediatR;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeUC.WebAPI.DAL.Commands
{
    public class UpdateEmployeeCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public int DepartmentId { get; set; }
        public int ManagerId { get; set; }
        public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, int>
        {
            private readonly IApplicationContext _context;
            public UpdateEmployeeCommandHandler(IApplicationContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
            {
                var employee = _context.Employees.Where(a => a.Id == command.Id).FirstOrDefault();

                if (employee == null)
                {
                    return default;
                }
                else
                {
                    employee.FirstName = command.FirstName;
                    employee.LastName = command.LastName;
                    employee.EmailId = command.EmailId;
                    employee.DepartmentId = command.DepartmentId;
                    employee.ManagerId = command.ManagerId;
                    await _context.SaveChangesAsync();
                    return employee.Id;
                }
            }
        }
    }
}
