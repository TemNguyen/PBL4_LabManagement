using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace PC_Heal_ClientService
{
    public partial class Service1 : ServiceBase
    {
        Timer _timer = null;
        private static int _activeTime;
        private static TcpClient _client = new TcpClient();
        public Service1()
        {
            InitializeComponent();
            CanShutdown = true;
        }

        protected override void OnStart(string[] args)
        {
            _activeTime = 0;
            _timer = new Timer();
            _timer.Interval = 1500;
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
            
        }

        private static async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _activeTime++;
            try
            {
                CI computer = GetComputerInformation();
                computer.ActiveTime = _activeTime;
                computer.IsOnline = true;
                await SendToServer.Send(computer);
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
                        computerInformation.DiskUsage = Convert.ToInt32(usage * 100);
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

                    computerInformation.CpuUsage = Convert.ToInt32(total / n);
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

                computerInformation.NumThread = threads;
                computerInformation.NumProcess = process;
            }

            using (var ram = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem").Get())
            {
                try
                {
                    foreach (var item in ram)
                    {
                        computerInformation.RamSize = Convert.ToInt32(double.Parse(item["TotalVisibleMemorySize"].ToString()) / (1024 * 1024));
                        var ramUsage = Convert.ToDouble(computerInformation.RamSize - (double.Parse(item["FreePhysicalMemory"].ToString()) / (1024 * 1024)));
                        computerInformation.RamUsage = Math.Round(ramUsage, 1);
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
                var counterNames = category.GetInstanceNames();
                var gpuCounters = new List<PerformanceCounter>();
                var result = 0f;

                foreach (string counterName in counterNames)
                {
                    if (counterName.EndsWith("engtype_3D"))
                    {
                        foreach (PerformanceCounter counter in category.GetCounters(counterName))
                        {
                            if (counter.CounterName == "Utilization Percentage")
                            {
                                gpuCounters.Add(counter);
                            }
                        }
                    }
                }

                gpuCounters.ForEach(x =>
                {
                    _ = x.NextValue();
                });

                gpuCounters.ForEach(x =>
                {
                    result += x.NextValue();
                });

                computerInformation.GpuUsage = Convert.ToInt32(result);
            }
            catch(Exception)
            {

            }

            return computerInformation;
        }
    }
}
