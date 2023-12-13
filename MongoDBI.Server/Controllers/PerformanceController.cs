using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDBI.Server.Models;
using MongoDBI.Server.Services;
using System.Diagnostics;

namespace MongoDBI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceController(IShiftsService _service) : ControllerBase
    {
        /**
        RelationalService _relService = new RelationalService();
        EmployeeController con = new EmployeeController(_service);
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

        [HttpGet("delations")]
        public async Task<string> DeleteAsync()
        {
            var x= _relService.Delete().Result.Item1;

            Stopwatch sw = Stopwatch.StartNew();
            var mongo = con.Remove(1);
            sw.Stop();
            var mongotime = sw.ElapsedMilliseconds;
            return @$"
DELETE

MSSQL   {(decimal)x/1000} sec
MONGO   {(decimal)mongotime / 1000} sec
";
        }



        [HttpGet("updates")]
        public async Task<string> UpdateAsync()
        {
            var x = _relService.UpdateShift().Result.Item1;

            var update = new Models.Employee
            {
                do_name = "update",
                 do_no = 99,
                shifts = [
                       new Shift
                       {
                           ISOweek = 1,
                           year = 2100,
                           monday = new Workday
                           {
                               workdays_id = 1
                           },
                           friday = new Workday
                           {
                               workdays_id = 1
                           },
                           saturday = new Workday
                           {
                               workdays_id = 1
                           },
                           sunday = new Workday
                           {
                               workdays_id = 1
                           },
                           thursday = new Workday
                           {
                               workdays_id = 1
                           },
                           tuesday = new Workday
                           {
                               workdays_id = 1
                           },
                           wednesday = new Workday
                           {
                               workdays_id = 1
                           }

                       }
                        ]
            };

            Stopwatch sw = Stopwatch.StartNew();
            var mongo = con.UpdateEmp(2,update);
            sw.Stop();

            var mongotime = sw.ElapsedMilliseconds;
            
            return @$"
UPDATE

MSSQL   {(decimal)x / 1000} sec
MONGO   {(decimal)mongotime / 1000} sec
";
        }
        

        [HttpGet("reads")]
        public async Task<string> REadsAsync()
        {
           var mssqlfind1 = _relService.Find1().Result.Item1;
            var mssqlfind2 = _relService.Find2(2).Result.Item1;
            var mssqlfind3 = _relService.Find3Aggregation(2).Result.Item1;
            var mssqlfind4 = _relService.Find4().Result.Item1;

            Stopwatch sw1 = Stopwatch.StartNew();
            var x = await _service.GetAsync();
            sw1.Stop();
            var mongotime = sw1.ElapsedMilliseconds;

            Stopwatch sw2 = Stopwatch.StartNew();
            var x2 = await _service.GetSingleAsync(4);
            sw2.Stop();
            var mongotime2 = sw2.ElapsedMilliseconds;

       
            var x3 = await _service.GetAggregateAsync();

            var mongotime3 = x3.Item1;

            var x4 = await _service.GetWorkdays(1);
     
            var mongotime4 = x4.Item1;

           

            return @$"
FINDS

1) ohne Filter
MSSQL   {(decimal)mssqlfind1 / 1000} sec
MONGO   {(decimal)mongotime / 1000} sec


2) mit Filter
MSSQL   {(decimal)mssqlfind2 / 1000} sec
MONGO   {(decimal)mongotime2 / 1000} sec

3)  mit Filter und Projektion (Aggregate und reference)
MSSQL   {(decimal)mssqlfind3 / 1000} sec
MONGO   {(decimal)mongotime3 / 1000} sec


4)  Projektion und Sortierung
MSSQL   {(decimal)mssqlfind4 / 1000} sec
MONGO   {(decimal)mongotime4 / 1000} sec
";
        }


        [HttpGet("final")]
        public async Task<string> Final(){

            return $@" 

{await InsertsAsync()}

{await DeleteAsync()}

{await UpdateAsync()}

{await REadsAsync()}
";
        }
        **/
    }
}
