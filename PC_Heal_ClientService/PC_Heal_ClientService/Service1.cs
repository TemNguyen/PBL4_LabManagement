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
        Timer _timer = null;
        private static int _activeTime;
        public Service1()
        {
            InitializeComponent();
            CanShutdown = true;
        }

        protected override void OnStart(string[] args)
        {
            _activeTime = 0;
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
            
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _activeTime++;
            try
            {
                CI computer = GetComputerInformation();
                computer.ActiveTime = _activeTime;
                computer.IsOnline = true;
                SendToServer.Send(computer);
            }
            catch (Exception)
            {

            }
        }

        protected override void OnStop()
        {
            try
            {
                CI computer = GetComputerInformation();
                computer.IsOnline = false;
                SendToServer.Send(computer);
            }
            catch (Exception)
            {

            }
        }

        protected override void OnShutdown()
        {
            try
            {
                CI computer = GetComputerInformation();
                computer.IsOnline = false;
                SendToServer.Send(computer);
            }
            catch (Exception)
            {

            }
        }
        static CI GetComputerInformation()
        {
            var computerInformation = new CI();
            using (var computer = new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                try
                {
                    foreach (var item in computer)
                    {
                        computerInformation.ComputerName = item["Name"].ToString();
                        computerInformation.NumberOfLogicalProcessors = item["NumberOfLogicalProcessors"].ToString();
                    }
                }
                catch (Exception)
                {
                    
                }

            }

            using (var processor = new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                try
                {
                    foreach (var item in processor)
                    {
                        computerInformation.ChipSet = item["Name"].ToString();
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
                    string gpu_Name = "";
                    foreach (var item in gpu)
                    {
                        gpu_Name += item["Name"].ToString() + ",";
                    }
                    computerInformation.GpuName = gpu_Name.Substring(0, gpu_Name.Length - 1);
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
                    computerInformation.IpAddress = IP;
                    computerInformation.MacAddress = MAC;
                }
                catch (Exception)
                {

                }
            }

            using(var disk = new ManagementObjectSearcher("SELECT * FROM Win32_Volume").Get())
            {
                try
                {
                    foreach (var item in disk)
                    {
                        var usage = double.Parse(item["FreeSpace"].ToString()) / double.Parse(item["Capacity"].ToString());
                        computerInformation.DiskUsage = (usage * 100).ToString("00.00");
                    }
                }
                catch (Exception)
                {
                    
                }
            }

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

                    computerInformation.CpuUsage = String.Format("{0:0.0}", total / n);
                }
                catch (Exception)
                {
                    //
                }
            }

            int process = 0;
            int threads = 0;

            using(var thread = new ManagementObjectSearcher("select * from Win32_Process").Get())
            {
                try
                {
                    foreach (var item in thread)
                    {
                        threads += Convert.ToInt32(item["ThreadCount"].ToString());
                        process++;
                    }
                }
                catch (Exception)
                {

                    
                }

                computerInformation.NumThread = threads.ToString();
                computerInformation.NumProcess = process.ToString();
            }

            using (var ram = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem").Get())
            {
                try
                {
                    foreach (var item in ram)
                    {
                        computerInformation.RamSize = (double.Parse(item["TotalVisibleMemorySize"].ToString()) / (1024*1024)).ToString("00.");
                        computerInformation.RamUsage = (int.Parse(computerInformation.RamSize) - double.Parse(item["FreePhysicalMemory"].ToString()) / (1024*1024)).ToString("00.0");
                    }
                }
                catch (Exception)
                {
                    //
                }
            }
            //GPU usage
            try
            {
                var category = new PerformanceCounterCategory("GPU Engine");
                var gpuCounter = category.GetCounters("Utilization Percentage");
                foreach (var item in gpuCounter)
                {
                    computerInformation.GpuUsage = item.NextValue().ToString("00.00");
                }
            }
            catch (Exception)
            {

            }

            return computerInformation;
        }
    }
}
