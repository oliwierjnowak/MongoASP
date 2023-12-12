using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDBI.Server.Services;

namespace MongoDBI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceController(IShiftsService _service) : ControllerBase
    {
        RelationalService _relService = new RelationalService();

        [HttpGet("inserts")]
        public async Task<string> InsertsAsync()
        {
            Console.WriteLine("Inserts \n");
            var f100 =  _relService.CreateShift(100).Result.Item1;
            Console.WriteLine("MSSQL  100: " + f100);
            var f1000 = _relService.CreateShift(1000).Result.Item1;
            Console.WriteLine("MSSQL  1000: " + (decimal)f1000/1000 + " sec");
            var f10000 = _relService.CreateShift(10000).Result.Item1;
            Console.WriteLine("MSSQL  10000: " + (decimal)f10000/60000 + " min");

            Console.WriteLine(" \n");
            EmployeeController con = new EmployeeController(_service);
            var f100Mongo = await con.Create1002(100);
            Console.WriteLine("MONGO  100: " + f100Mongo);
            var f1000Mongo = await con.Create1002(1000);
            Console.WriteLine("MONGO  1000: " + (decimal)f1000Mongo/1000 + " sec");
            var f10000Mongo = await con.Create1002(10000);
            Console.WriteLine("MONGO  1000: " + (decimal)f10000Mongo/60000 + " min");

            return @$"
INSERT
        100      1000     1000
MSSQL|  {f100} /  {(decimal)f1000 / 1000} / {(decimal) f10000 / 60000} min
INSERT
        100      1000     1000
MSSQL|  {f100Mongo} / {(decimal)f1000Mongo/1000} sec / {(decimal)f10000Mongo / 60000 } min


";
        
        }



    }
}
