using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Heal_ClientService
{
    public class CI
    {
        public string ComputerName { get; set; }
        public string GpuName { get; set; }
        public string NumberOfLogicalProcessors { get; set; }
        public string ChipSet { get; set; }
        public string RamSize { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public string CpuUsage { get; set; }
        public string NumThread { get; set; }
        public string NumProcess { get; set; }
        public string DiskUsage { get; set; }
        public string RamUsage { get; set; }
        public string GpuUsage { get; set; }
        public int ActiveTime{ get; set; }
        public bool IsOnline { get; set; }
    }
}
