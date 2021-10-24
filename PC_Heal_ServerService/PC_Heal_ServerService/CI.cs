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
        public string NumberOfProcessors { get; set; }
        public string NumberOfLogicalProcessors { get; set; }
        public string ProcessorName { get; set; }
        public string IPAddress { get; set; }
        public string MACAddress { get; set; }
        public string C_DiskFree { get; set; }
        public string D_DiskFree { get; set; }
        public string CPU_Usage { get; set; }
        public string RAM_Usage { get; set; }

        public override string ToString()
        {
            return "Computer's Name: " + ComputerName +
                "\n NumberOfProcessors: " + NumberOfProcessors
                + "\n NumberOfLogicalProcessors: " + NumberOfLogicalProcessors
                + "\n ProcessorName: " + ProcessorName
                + "\n IPAddress: " + IPAddress
                + "\n MACAddress: " + MACAddress
                + "\n C_DiskFree: " + C_DiskFree
                + "\n D_DiskFree: " + D_DiskFree
                + "\n CPU_Usage: " + CPU_Usage
                + "\n RAM_Usage: " + RAM_Usage + "\n";
        }
    }
}
