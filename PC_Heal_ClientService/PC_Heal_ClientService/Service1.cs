using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PC_Heal_ClientService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer();
            timer.Interval = 2000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                CI computer = GetCI();
                SendToServer.Send(computer);
            }
            catch (Exception)
            {

            }
        }

        protected override void OnStop()
        {

        }
        static CI GetCI()
        {
            var computerInfor = new CI();
            using (var computer_System = new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                try
                {
                    foreach (var item in computer_System)
                    {
                        computerInfor.ComputerName = item["Name"].ToString();
                        computerInfor.NumberOfProcessors = item["NumberOfProcessors"].ToString();
                        computerInfor.NumberOfLogicalProcessors = item["NumberOfLogicalProcessors"].ToString();
                    }
                }
                catch (Exception)
                {
                    //ignore
                }

            }

            using (var processor = new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                try
                {
                    foreach (var item in processor)
                    {
                        computerInfor.ProcessorName = item["Name"].ToString();
                        break;
                    }
                }
                catch (Exception)
                {
                    //ignore
                }
            }

            using (var gpu = new System.Management.ManagementObjectSearcher("select * from Win32_VideoController").Get())
            {
                try
                {
                    foreach(var item in gpu)
                    {
                        computerInfor.GPUName = item["Name"].ToString();
                        break;
                    }
                }
                catch (Exception)
                {

                    
                }
            }

            string IP = String.Empty;
            string MAC = String.Empty;

            using (var network = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                try
                {
                    foreach (var item in network.GetInstances())
                    {
                        if (MAC == String.Empty)
                        {
                            if ((bool)item["IPEnabled"] == true)
                            {
                                MAC = item["MACAddress"].ToString();
                                String[] IPs = (String[])item["IPAddress"];
                                IP = IPs[0];
                            }
                        }
                    }
                    computerInfor.IPAddress = IP;
                    computerInfor.MACAddress = MAC;
                }
                catch (Exception)
                {

                }
            }


            DriveInfo dDisk = new DriveInfo("D");
            DriveInfo cDisk = new DriveInfo("C");
            computerInfor.C_DiskFree = ((cDisk.AvailableFreeSpace / (float)cDisk.TotalSize) * 100).ToString("00.00");
            computerInfor.D_DiskFree = ((dDisk.AvailableFreeSpace / (float)dDisk.TotalSize) * 100).ToString("00.00");

            using (var processor = new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor").Get())
            {
                double total = 0;
                int n = 0;
                try
                {
                    foreach (var item in processor)
                    {
                        total += Convert.ToDouble(item["PercentProcessorTime"]);
                        n++;
                    }

                    computerInfor.CPU_Usage = String.Format("{0:0.0}", total / n);
                }
                catch (Exception)
                {
                    //
                }
            }

            using (var operating = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem").Get())
            {
                try
                {
                    foreach (var item in operating)
                    {
                        computerInfor.RAM_Usage = ((int.Parse(item["FreePhysicalMemory"].ToString()) / (float)int.Parse(item["TotalVisibleMemorySize"].ToString())) * 100).ToString("00.00");
                    }
                }
                catch (Exception)
                {
                    //
                }
            }

            return computerInfor;
        }
    }
}
