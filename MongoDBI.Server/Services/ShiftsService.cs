using Microsoft.Extensions.Options;
using MongoDBI.Server.Models;

namespace MongoDBI.Server.Services
{
    public class ShiftsService(IOptions<MongoSettings> mongoSettings)
    {
    }
}
