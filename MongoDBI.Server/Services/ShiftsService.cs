using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDBI.Server.Models;

namespace MongoDBI.Server.Services
{
    public interface IShiftsService
    {
        Task<List<Employee>> GetAsync();
        Task<Employee> GetSingleAsync(int dono);
        Task<Employee> CreateAsync(Employee newEmp);
        Task<Employee> UpdateAsync(int dono, Employee updatedEmployee);
        Task RemoveAsync(int dono);
        Task CreateMany(IEnumerable<Employee> Emps);
        Task RemoveAllAsync();
        Task<List<Employee>> IsoWeekDayAndShiftName(int isoWeek, DayOfWeek day, string shiftname);

    }

    public class ShiftsService : IShiftsService
    {
        private readonly IMongoCollection<Employee> _collection;

        public ShiftsService(IOptions<MongoSettings> DatabaseSettings)
        {
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
            await _collection.ReplaceOneAsync(x => x.do_no == dono, updatedEmployee);
            return await _collection.Find(x => x.do_no == updatedEmployee.do_no).FirstAsync();
        }
            

        public async Task RemoveAsync(int dono) =>
            await _collection.DeleteOneAsync(x => x.do_no == dono);

        public async Task CreateMany(IEnumerable<Employee> Emps) =>
            await _collection.InsertManyAsync(Emps);

        public async Task RemoveAllAsync() =>
             await _collection.DeleteOneAsync(_ => true);


        public async Task<List<Employee>> IsoWeekDayAndShiftName(int isoWeek, DayOfWeek day, string shiftname)
        {
           // var employeesWithShift = Enumerable.Empty<Employee>();
            switch ((int)day)
            {
                case 1:
                    var employeesWithShift = await _collection.Find(x =>
                    x.shifts.Any(s => s.ISOweek == isoWeek && s.monday.shiftname == shiftname)).ToListAsync();
                    return employeesWithShift;
                case 2:
                    var employeesWithShift2 = await _collection.Find(x =>
                    x.shifts.Any(s => s.ISOweek == isoWeek && s.tuesday.shiftname == shiftname)).ToListAsync();
                    return employeesWithShift2;
                case 3:
                    var employeesWithShift3 = await _collection.Find(x =>
                    x.shifts.Any(s => s.ISOweek == isoWeek && s.wednesday.shiftname == shiftname)).ToListAsync();
                    return employeesWithShift3;
                case 4:
                    var employeesWithShift4 = await _collection.Find(x =>
                    x.shifts.Any(s => s.ISOweek == isoWeek && s.thursday.shiftname == shiftname)).ToListAsync();
                    return employeesWithShift4;
                case 5:
                    var employeesWithShift5 = await _collection.Find(x =>
                    x.shifts.Any(s => s.ISOweek == isoWeek && s.friday.shiftname == shiftname)).ToListAsync();
                    return employeesWithShift5;
                case 6:
                    var employeesWithShift6 = await _collection.Find(x =>
                    x.shifts.Any(s => s.ISOweek == isoWeek && s.saturday.shiftname == shiftname)).ToListAsync();
                    return employeesWithShift6;
                case 7:
                    var employeesWithShift7 = await _collection.Find(x =>
                    x.shifts.Any(s => s.ISOweek == isoWeek && s.sunday.shiftname == shiftname)).ToListAsync();
                    return employeesWithShift7;
                default: throw new NotImplementedException();
            }
            

            
        }

    }
}
