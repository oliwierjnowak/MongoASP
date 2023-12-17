using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDBI.Server.Models;
using MongoDBI.Server.Services;
using System.Diagnostics;

namespace MongoDBI.Server.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]

    [ApiController]
    public class EmployeeController(IShiftsService _service) : ControllerBase
    {

        [HttpGet]
        public async Task<List<Employee>> Get() =>
          await _service.GetAsync();

        [HttpGet("weekdayshiftname")]
        public async Task<List<Employee>> IsoWeekDayAndShiftName(int isoWeek, int day, int shiftname)
        {
            DayOfWeek dayOfWeek = (DayOfWeek)day;
            return await _service.IsoWeekDayAndShiftName(isoWeek, dayOfWeek, shiftname);
        }
        [HttpGet]
        [Route("{dono:int}")]
        public async Task<Employee> Get( int dono) =>
          await _service.GetSingleAsync(dono);

        [HttpDelete]
        [Route("{dono:int}")]
        public async Task Remove(int dono) =>
            await _service.RemoveAsync(dono);

        [HttpDelete]
        [Route("RemoveAll")]
        public async Task RemoveAll() =>
           await _service.RemoveAllAsync();


        [HttpPut]
        [Route("{dono:int}")]
        public async Task<Employee> UpdateEmp(int dono, [FromBody] Employee UpdateEmp) =>
            await _service.UpdateAsync(dono, UpdateEmp);

        [HttpPost]
        public async Task<Employee> CreateEmp([FromBody] Employee CreateEmp) =>
            await _service.CreateAsync(CreateEmp);
        /**
 [HttpPost("{numberOfInserts:int}/InsertMany")]
 public async Task<long> Create100(int numberOfInserts)
 {
     List<Employee> list = [
         new Employee
         {
             do_name = "create100",
             do_no = 10,
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
         }
     ];

     foreach (int value in Enumerable.Range(1, numberOfInserts))
     {
         var x = list[0].Clone();
         x.do_no = x.do_no + value;

         list.Add(x);
     }
     Stopwatch sw = new Stopwatch();

     sw.Start();

     await _service.CreateMany(list);

     sw.Stop();
    

     return sw.ElapsedMilliseconds;
 }

 [HttpPost("{numberOfInserts:int}/InsertOne")]
 public async Task<long> Create1002(int numberOfInserts)
 {
     List<Employee> list = [
         new Employee
         {
             do_name = "create100",
             do_no = 10,
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
         }
     ];
     Stopwatch sw = new Stopwatch();

     sw.Start();
     foreach (int value in Enumerable.Range(1, numberOfInserts))
     {
         var x = list[0].Clone();
         x.do_no = x.do_no + value;
         await CreateEmp(x);
         list.Add(x);
     }
    

    // await _service.CreateMany(list);

     sw.Stop();


     return sw.ElapsedMilliseconds;
 }
 **/
        // // komplizierte aggregate funktion die die schichten tage joint
        [HttpGet("aggregate")]
        public async Task<List<EmpDTO>> GetAggregateAsync()
        {
            return _service.GetAggregateAsync().Result.Item2;
        }


        // Gibt die liste der schicht tagen zurück und sortiert deswegen bool
        [HttpGet]
        [Route("workdays/{sort:bool}")]
        public async Task<dynamic> GetWorkdays(bool sort)
        {
            byte sortval = sort ? (byte)1 : (byte)0;

            return  _service.GetWorkdays(sortval).Result.Item2;
        }
    }
}
