using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDBI.Server.Models;

namespace MongoDBI.Server.Services
{
    public interface IShiftsService
    {
        Task<List<Employee>> GetAsync();
        Task<Employee> GetSingleAsync(int dono);

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


    }
}
