using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDBI.Server.Models;
using MongoDBI.Server.Services;

namespace MongoDBI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IShiftsService _service) : ControllerBase
    {

        [HttpGet]
        public async Task<List<Employee>> Get() =>
          await _service.GetAsync();

        [Route("{dono:int}")]
        public async Task<Employee> Get( int dono) =>
          await _service.GetSingleAsync(dono);
        
    }
}
