using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Heal_ServerService
{
    [Table("Computer Information")]
    public class CI
    {
        [Key]
        public string ComputerName { get; set; }
        public string GPUName { get; set; }
        public string NumberOfLogicalProcessors { get; set; }
        public string Chipset { get; set; }
        public string RAM_Size { get; set; }
        public string IPAddress { get; set; }
        public string MACAddress { get; set; }
        public string CPU_Usage { get; set; }
        public string Max_Clock_Speed { get; set; }
        public string Num_Thread { get; set; }
        public string Num_Process { get; set; }
        public string Disk_Usage { get; set; }
        public string RAM_Usage { get; set; }
        public string GPU_Usage { get; set; }
    }
}
