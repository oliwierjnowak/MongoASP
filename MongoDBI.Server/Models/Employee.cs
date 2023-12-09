using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDBI.Server.Models
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("do_no")]
        public int do_no { get; set; }
        [BsonElement("do_name")]
        public string do_name { get; set;}
        [BsonElement("shifts")]
        public IEnumerable<Shift> shifts { get; set; }
    }

    public class Shift
    {
        [BsonElement("ISOweek")]
        public int ISOweek { get; set; }
        [BsonElement("year")]
        public int year { get; set; }
        [BsonElement("monday")]
        public Workday monday { get; set; }
        [BsonElement("tuesday")]
        public Workday tuesday { get; set; }
        [BsonElement("wednesday")]
        public Workday wednesday { get; set; }
        [BsonElement("thursday")]
        public Workday thursday { get; set; }
        [BsonElement("friday")]
        public Workday friday { get; set; }
        [BsonElement("saturday")]
        public Workday saturday { get; set; }
        [BsonElement("sunday")]
        public Workday sunday { get; set; }
    }

    public class Workday
    {
        [BsonElement("shiftname")]
        public string shiftname{ get; set; }
        [BsonElement("shifthours")]
        public int shifthours { get; set; }
    }
}
