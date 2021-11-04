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
        public int RamSize { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public int CpuUsage { get; set; }
        public int NumThread { get; set; }
        public int NumProcess { get; set; }
        public int DiskUsage { get; set; }
        public double RamUsage { get; set; }
        public int GpuUsage { get; set; }
        public int ActiveTime{ get; set; }
        public bool IsOnline { get; set; }
    }
}
