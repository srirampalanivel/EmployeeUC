using EmployeeUC.WebAPI.Domain.Models;
using EmployeeUC.WebAPI.Context;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EmployeeUC.WebAPI.Utilities;

namespace EmployeeUC.WebAPI.DAL.Commands
{
    public class CreateEmployeeCommand : IRequest<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public int DepartmentId { get; set; }
        public int ManagerId { get; set; }
        public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
        {
            private readonly IApplicationContext _context;
            public CreateEmployeeCommandHandler(IApplicationContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
            {
                var employee = new Employee();
                employee.FirstName = command.FirstName;
                employee.LastName = command.LastName;
                employee.EmailId = command.EmailId;
                employee.DepartmentId = command.DepartmentId;
                employee.ManagerId = command.ManagerId;

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                SendEmailNotificationHelper eMail = new SendEmailNotificationHelper();
                eMail.SendEmailNotificationtoCustomer();
                return employee.Id;
            }
        }
    }
}
