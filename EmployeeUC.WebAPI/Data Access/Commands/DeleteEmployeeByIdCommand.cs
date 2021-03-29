using EmployeeUC.WebAPI.Context;
using EmployeeUC.WebAPI.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeUC.WebAPI.DAL.Commands
{
    public class DeleteEmployeeByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
        public class DeleteEmployeeByIdCommandHandler : IRequestHandler<DeleteEmployeeByIdCommand, int>
        {
            private readonly IApplicationContext _context;
            public DeleteEmployeeByIdCommandHandler(IApplicationContext context)
            {
                _context = context;
            }
            public async Task<int> Handle(DeleteEmployeeByIdCommand command, CancellationToken cancellationToken)
            {
                var employee = await _context.Employees.Where(a => a.Id == command.Id).FirstOrDefaultAsync();
                if (employee == null) return default;
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                SendEmailNotificationHelper eMail = new SendEmailNotificationHelper();
                eMail.SendEmailNotificationtoCustomer();
                return employee.Id;
            }
        }
    }
}
