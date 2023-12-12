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

        public Employee Clone()
        {
            var clone = this.MemberwiseClone();
            return (Employee)clone;
        }
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
        [BsonRepresentation(BsonType.Int64)]
        [BsonElement("workdays_id")]
        public int workdays_id { get; set; }
        /*
        [BsonElement("shiftname")]
        public string shiftname{ get; set; }
        [BsonElement("shifthours")]
        public int shifthours { get; set; }*/
    }
    public class WorkdayObject
    {
        [BsonRepresentation(BsonType.Int64)]
        [BsonElement("_id")]
        public int workdays_id { get; set; }
        
        [BsonElement("shiftname")]
        public string shiftname{ get; set; }
        [BsonElement("shifthours")]
        public int shifthours { get; set; }
    }


    public class EmployeeAggregate
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("do_no")]
        public int do_no { get; set; }
        [BsonElement("do_name")]
        public string do_name { get; set; }
        [BsonElement("shifts")]
        public ShiftAggregate shifts { get; set; }

        public Employee Clone()
        {
            var clone = this.MemberwiseClone();
            return (Employee)clone;
        }
    }
    public class ShiftAggregate
    {
        [BsonElement("ISOweek")]
        public int ISOweek { get; set; }
        [BsonElement("year")]
        public int year { get; set; }
        [BsonElement("monday")]
        public IEnumerable<WorkdayObject> monday { get; set; }
        [BsonElement("tuesday")]
        public IEnumerable<WorkdayObject> tuesday { get; set; }
        [BsonElement("wednesday")]
        public IEnumerable<WorkdayObject> wednesday { get; set; }
        [BsonElement("thursday")]
        public IEnumerable<WorkdayObject> thursday { get; set; }
        [BsonElement("friday")]
        public IEnumerable<WorkdayObject> friday { get; set; }
        [BsonElement("saturday")]
        public IEnumerable<WorkdayObject> saturday { get; set; }
        [BsonElement("sunday")]
        public IEnumerable<WorkdayObject> sunday { get; set; }
    }

    public class EmpDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("do_no")]
        public int do_no { get; set; }
        [BsonElement("do_name")]
        public string do_name { get; set; }
        [BsonElement("shifts")]
        public IEnumerable<ShiftDTO> shifts { get; set; }

        public Employee Clone()
        {
            var clone = this.MemberwiseClone();
            return (Employee)clone;
        }


        public class ShiftDTO
        {
            [BsonElement("ISOweek")]
            public int ISOweek { get; set; }
            [BsonElement("year")]
            public int year { get; set; }
            [BsonElement("monday")]
            public WorkdayObject monday { get; set; }
            [BsonElement("tuesday")]
            public WorkdayObject tuesday { get; set; }
            [BsonElement("wednesday")]
            public WorkdayObject wednesday { get; set; }
            [BsonElement("thursday")]
            public WorkdayObject thursday { get; set; }
            [BsonElement("friday")]
            public WorkdayObject friday { get; set; }
            [BsonElement("saturday")]
            public WorkdayObject saturday { get; set; }
            [BsonElement("sunday")]
            public WorkdayObject sunday { get; set; }
        }
    }
}
