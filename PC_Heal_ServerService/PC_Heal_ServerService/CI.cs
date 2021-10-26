using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Heal_ServerService
{
    [Table("CI")]
    public class CI
    {
        [Key]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string ComputerName { get; set; }
        [BsonElement]
        public string GPUName { get; set; }
        [BsonElement]
        public string NumberOfLogicalProcessors { get; set; }
        [BsonElement]
        public string Chipset { get; set; }
        [BsonElement]
        public string RAM_Size { get; set; }
        [BsonElement]
        public string IPAddress { get; set; }
        [BsonElement]
        public string MACAddress { get; set; }
        [BsonElement]
        public string CPU_Usage { get; set; }
        [BsonElement]
        public string Max_Clock_Speed { get; set; }
        [BsonElement]
        public string Num_Thread { get; set; }
        [BsonElement]
        public string Num_Process { get; set; }
        [BsonElement]
        public string Disk_Usage { get; set; }
        [BsonElement]
        public string RAM_Usage { get; set; }
        [BsonElement]
        public string GPU_Usage { get; set; }
        [BsonElement]
        public int ActiveTime { get; set; }
    }
}
