using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MongoDBI.Server.Models;
using MongoDBI.Server.Services;
using System.Diagnostics;

namespace MongoDBI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IShiftsService _service) : ControllerBase
    {

        [HttpGet]
        public async Task<List<Employee>> Get() =>
          await _service.GetAsync();

        [HttpGet("weekdayshiftname")]
        public async Task<List<Employee>> IsoWeekDayAndShiftName(int isoWeek, int day, string shiftname)
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

        [HttpPost("{numberOfInserts:int}/InsertMany")]
        public async Task<long> Create100(int numberOfInserts)
        {
            List<Employee> list = [
                new Employee
                {
                    do_name = "create 100",
                    do_no = 10,
                    shifts = [
                        new Shift
                        {
                            ISOweek= 1,
                            year= 2100,
                            monday = new Workday
                            {
                                shifthours= 1,
                                shiftname= "100"
                            },
                            friday = new Workday
                            {
                                shifthours = 1,
                                shiftname = "100"
                            },
                            saturday = new Workday
                            {
                                shifthours = 1,
                                shiftname = "100"
                            },
                            sunday = new Workday
                            {
                                shifthours = 1,
                                shiftname = "100"
                            },
                            thursday = new Workday
                            {
                                shifthours = 1,
                                shiftname = "100"
                            },
                            tuesday = new Workday
                            {
                                shifthours = 1,
                                shiftname = "100"
                            },
                            wednesday = new Workday
                            {
                                shifthours = 1,
                                shiftname = "100"
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
                    do_name = "create 100",
                    do_no = 10,
                    shifts = [
                        new Shift
                        {
                            ISOweek = 1,
                            year = 2100,
                            monday = new Workday
                            {
                                shifthours = 1,
                                shiftname = "100"
                            },
                            friday = new Workday
                            {
                                shifthours = 1,
                                shiftname = "100"
                            },
                            saturday = new Workday
                            {
                                shifthours = 1,
                                shiftname = "100"
                            },
                            sunday = new Workday
                            {
                                shifthours = 1,
                                shiftname = "100"
                            },
                            thursday = new Workday
                            {
                                shifthours = 1,
                                shiftname = "100"
                            },
                            tuesday = new Workday
                            {
                                shifthours = 1,
                                shiftname = "100"
                            },
                            wednesday = new Workday
                            {
                                shifthours = 1,
                                shiftname = "100"
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
                CreateEmp(x);
                list.Add(x);
            }
           

           // await _service.CreateMany(list);

            sw.Stop();


            return sw.ElapsedMilliseconds;
        }

        
    }
}
