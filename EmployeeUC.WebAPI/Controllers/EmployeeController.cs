using EmployeeUC.WebAPI.DAL.Commands;
using EmployeeUC.WebAPI.DAL.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace EmployeeUC.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        /// <summary>
        /// Creates a New Employee.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Updates the Employee Entity based on Id.   
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateEmployeeCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Deletes Employee based on Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteEmployeeByIdCommand { Id = id }));
        }

        /// <summary>
        /// Gets all Employees.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllEmployeesQuery()));
        }

        /// <summary>
        /// Gets Employee Entity by Id/ Department/ First Name/ Last Name
        /// </summary>
        /// <param name="id">Emp ID</param>
        /// <param name="department">Department</param>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        /// <returns></returns>
        [HttpGet("{id}/{department}/{firstName}/{lastName}")]
        public async Task<IActionResult> GetBy(int id, int department, string firstName, string lastName)
        {
            return Ok(await Mediator.Send(new GetEmployeeByQuery { Id = id , Department = department, FirstName = firstName, LastName = lastName })) ;
        }

        /// <summary>
        /// Gets Employee Status by Id
        /// </summary>
        /// <param name="id">Emp ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBy(int id)
        {
            return Ok(await Mediator.Send(new GetEmployeeStatusQuery { Id = id}));
        }
    }
}