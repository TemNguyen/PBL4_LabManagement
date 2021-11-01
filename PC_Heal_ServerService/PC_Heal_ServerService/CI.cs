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
        [BsonElement("computerName")]
        public string ComputerName { get; set; }
        [BsonElement("GPUName")]
        public string GpuName { get; set; }
        [BsonElement("numberOfLogicalProcessors")]
        public string NumberOfLogicalProcessors { get; set; }
        [BsonElement("chipset")]
        public string ChipSet { get; set; }
        [BsonElement("RAMSize")]
        public string RamSize { get; set; }
        [BsonElement("IPAddress")]
        public string IpAddress { get; set; }
        [BsonElement("MACAddress")]
        public string MacAddress { get; set; }
        [BsonElement("CPUUsage")]
        public string CpuUsage { get; set; }
        [BsonElement("numThread")]
        public string NumThread { get; set; }
        [BsonElement("numProcess")]
        public string NumProcess { get; set; }
        [BsonElement("diskUsage")]
        public string DiskUsage { get; set; }
        [BsonElement("RAMUsage")]
        public string RamUsage { get; set; }
        [BsonElement("GPUUsage")]
        public string GpuUsage { get; set; }
        [BsonElement("activeTime")]
        public int ActiveTime { get; set; }
        [BsonElement("isOnline")]
        public bool IsOnline { get; set; }
    }
}
