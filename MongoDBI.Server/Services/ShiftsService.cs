using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDBI.Server.Models;
using System.Diagnostics;
using static MongoDB.Driver.WriteConcern;

namespace MongoDBI.Server.Services
{
    public interface IShiftsService
    {
        Task<List<Employee>> GetAsync();
        Task<Employee> GetSingleAsync(int dono);
        Task<Employee> CreateAsync(Employee newEmp);
        Task<long> CreatePerfAsync(int howoften);
        Task<Employee> UpdateAsync(int dono, Employee updatedEmployee);
        Task RemoveAsync(int dono);
        Task CreateMany(IEnumerable<Employee> Emps);
        Task RemoveAllAsync();
        Task<List<Employee>> IsoWeekDayAndShiftName(int isoWeek, DayOfWeek day, int shiftname);
        Task<(long,List<EmpDTO>)> GetAggregateAsync();
        Task<(long,dynamic)> GetWorkdays(byte sort);

    }

    public class ShiftsService : IShiftsService
    {
        private readonly IMongoCollection<Employee> _collection;
        private readonly IOptions<MongoSettings> _databaseSettings;
        public ShiftsService(IOptions<MongoSettings> DatabaseSettings)
        {
            _databaseSettings = DatabaseSettings;
            var mongoClient = new MongoClient(
                DatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                DatabaseSettings.Value.DatabaseName);

            _collection = mongoDatabase.GetCollection<Employee>(
                DatabaseSettings.Value.CollectionName);
        }

        public async Task<List<Employee>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<Employee> GetSingleAsync(int dono) =>
           await _collection.Find(x => x.do_no == dono).FirstAsync();


        public async Task<Employee> CreateAsync(Employee newEmp)
        {
            await _collection.InsertOneAsync(newEmp);
            return await _collection.Find(x => x.do_no == newEmp.do_no).FirstAsync();
        }

        public async Task<Employee> UpdateAsync(int dono, Employee updatedEmployee)
        {
            //extremly fast
             await _collection.ReplaceOneAsync(x => x.do_no == dono, updatedEmployee);
           /** var filter = Builders<Employee>.Filter.Eq(s => s.do_no, dono) ;
            var update = Builders<Employee>.Update
            .Set(restaurant => restaurant, updatedEmployee);
            await _collection.UpdateManyAsync(filter, update);**/

            return await _collection.Find(x => x.do_no == updatedEmployee.do_no).FirstAsync();
        }
            

        public async Task RemoveAsync(int dono) =>
            await _collection.DeleteOneAsync(x => x.do_no == dono);

        public async Task CreateMany(IEnumerable<Employee> Emps) =>
            await _collection.InsertManyAsync(Emps);

        public async Task RemoveAllAsync() =>
             await _collection.DeleteManyAsync(_ => true);


        public async Task<List<Employee>> IsoWeekDayAndShiftName(int isoWeek, DayOfWeek day, int shiftname)
        {
           // var employeesWithShift = Enumerable.Empty<Employee>();
            switch ((int)day)
            {
                case 1:
                    var employeesWithShift = await _collection.Find(x =>
                    x.shifts.Any(s => s.ISOweek == isoWeek && s.monday.workdays_id == shiftname)).ToListAsync();
                    return employeesWithShift;
                case 2:
                    var employeesWithShift2 = await _collection.Find(x =>
                    x.shifts.Any(s => s.ISOweek == isoWeek && s.tuesday.workdays_id == shiftname)).ToListAsync();
                    return employeesWithShift2;
                case 3:
                    var employeesWithShift3 = await _collection.Find(x =>
                    x.shifts.Any(s => s.ISOweek == isoWeek && s.wednesday.workdays_id == shiftname)).ToListAsync();
                    return employeesWithShift3;
                case 4:
                    var employeesWithShift4 = await _collection.Find(x =>
                    x.shifts.Any(s => s.ISOweek == isoWeek && s.thursday.workdays_id == shiftname)).ToListAsync();
                    return employeesWithShift4;
                case 5:
                    var employeesWithShift5 = await _collection.Find(x =>
                    x.shifts.Any(s => s.ISOweek == isoWeek && s.friday.workdays_id == shiftname)).ToListAsync();
                    return employeesWithShift5;
                case 6:
                    var employeesWithShift6 = await _collection.Find(x =>
                    x.shifts.Any(s => s.ISOweek == isoWeek && s.saturday.workdays_id == shiftname)).ToListAsync();
                    return employeesWithShift6;
                case 7:
                    var employeesWithShift7 = await _collection.Find(x =>
                    x.shifts.Any(s => s.ISOweek == isoWeek && s.sunday.workdays_id == shiftname)).ToListAsync();
                    return employeesWithShift7;
                default: throw new NotImplementedException();
            }
            

            
        }

        public async Task<(long,List<EmpDTO>)> GetAggregateAsync()
        {
           

            var agendoCollection = _collection;


            var mongoClient = new MongoClient(
             _databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                _databaseSettings.Value.DatabaseName);

            var shiftsCollection = mongoDatabase.GetCollection<BsonDocument>("workdays");


            var pipeline = new BsonDocument[]
            {
        
            new BsonDocument("$unwind", "$shifts"),
            new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "workdays" },
                    { "localField", "shifts.monday.workdays_id" },
                    { "foreignField", "_id" },
                    { "as", "shifts.monday" }
                }
            ),
             new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "workdays" },
                    { "localField", "shifts.tuesday.workdays_id" },
                    { "foreignField", "_id" },
                    { "as", "shifts.tuesday" }
                }
            ),
              new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "workdays" },
                    { "localField", "shifts.wednesday.workdays_id" },
                    { "foreignField", "_id" },
                    { "as", "shifts.wednesday" }
                }
            ),
            new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "workdays" },
                    { "localField", "shifts.thursday.workdays_id" },
                    { "foreignField", "_id" },
                    { "as", "shifts.thursday" }
                }
            ),
               new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "workdays" },
                    { "localField", "shifts.friday.workdays_id" },
                    { "foreignField", "_id" },
                    { "as", "shifts.friday" }
                }
            ),
            new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "workdays" },
                    { "localField", "shifts.saturday.workdays_id" },
                    { "foreignField", "_id" },
                    { "as", "shifts.saturday" }
                }
            ),
             new BsonDocument("$lookup",
                new BsonDocument
                {
                    { "from", "workdays" },
                    { "localField", "shifts.sunday.workdays_id" },
                    { "foreignField", "_id" },
                    { "as", "shifts.sunday" }
                }
            ),
            new BsonDocument("$project",
                new BsonDocument
                {
                    { "do_no", 1 },
                    { "do_name", 1 },
                    { "shifts", 1 },
                    { "_id",(BsonValue)1}
                }
            )
            };
            Stopwatch sw3 = Stopwatch.StartNew();
            var result = await agendoCollection.Aggregate<EmployeeAggregate>(pipeline).ToListAsync();
            sw3.Stop();
            var mongotime3 = sw3.ElapsedMilliseconds;
            var x = result.GroupBy(c => new { c.do_no, c.Id, c.do_name });



            var empDTOs = new List<EmpDTO>();
            foreach (var item in x)
            {
                var c = item;

                var shifts = new List<EmpDTO.ShiftDTO>();
                foreach (var item2 in item)
                {
                    var shift1 = new EmpDTO.ShiftDTO
                    {
                        ISOweek = item2.shifts.ISOweek,
                        year = item2.shifts.year,
                        monday = item2.shifts.monday.FirstOrDefault(),
                        tuesday = item2.shifts.tuesday.FirstOrDefault(),
                        friday = item2.shifts.friday.FirstOrDefault(),
                        saturday = item2.shifts.saturday.FirstOrDefault(),
                        sunday= item2.shifts.sunday.FirstOrDefault(),
                        thursday = item2.shifts.thursday.FirstOrDefault(),
                        wednesday = item2.shifts.wednesday.FirstOrDefault()
                        
                    };
                    shifts.Add(shift1);
                }
                var emp = new EmpDTO
                {
                    shifts = shifts,
                    do_name = item.Key.do_name,
                    do_no = item.Key.do_no,
                    Id = item.Key.Id
                };

                empDTOs.Add(emp);
            }


            return (mongotime3,empDTOs);

        }
        //doesnt return shift with 0 hours
        public async Task<(long,dynamic)> GetWorkdays(byte sortASC)
        {
            var sort = sortASC == 1 ? 1 : -1;
            var mongoClient = new MongoClient(
            _databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                _databaseSettings.Value.DatabaseName);

            var shiftsCollection = mongoDatabase.GetCollection<WorkdayObject>("workdays");


            var pipeline = new BsonDocument[]
            {
           new BsonDocument("$sort",
                new BsonDocument
                {


                    {"shifthours",sort }
                }
            ),
             new BsonDocument("$match",
                new BsonDocument
                {


                    {"shifthours",new BsonDocument("$gt" , 0 ) }
                }
            ),
            // Project to reshape the document
            new BsonDocument("$project",
                new BsonDocument
                {
        
                   
                    {"shiftname",1 },
                     { "_id",(BsonValue)0},
                      {"shifthours",1 }
                }
            )
            };
            Stopwatch sw4 = Stopwatch.StartNew();
            // returning dynamic because there is no extra need for debugging at this point or converting it into some object
            var x =  (await shiftsCollection.AggregateAsync<dynamic>(pipeline)).ToList();
            sw4.Stop();
            var mongotime4 = sw4.ElapsedMilliseconds;
            return (mongotime4,x);
        }

        public async Task<long> CreatePerfAsync(int howoften)
        {

            var newEmp = new Employee
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
            };
            Stopwatch sw = Stopwatch.StartNew();
            int i = 0;
            while(i < howoften)
            {
                await _collection.InsertOneAsync(newEmp.Clone());
                i++;
            }
     

            sw.Stop();
            var time = sw.ElapsedMilliseconds;
            return time;
        }
    }
}
