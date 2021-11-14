using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Timers;
using Timer = System.Timers.Timer;

namespace PC_Heal_ClientService
{
    public partial class Service1 : ServiceBase
    {
        Timer _timer = null;
        private static int _activeTime;
        public Service1()
        {
            InitializeComponent();
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
                //ignore
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
                    //ignore
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
                    string gpuName = "";
                    foreach (var item in gpu)
                    {
                        gpuName += item["Name"].ToString() + ", ";
                    }
                    computerInformation.GpuName = gpuName.Substring(0, gpuName.Length - 1);
                }
                catch (Exception)
                {
                    //ignore
                }
            }

            string ip = String.Empty;
            string mac = String.Empty;
            using (var network = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                try
                {
                    foreach (var item in network.GetInstances())
                    {
                        if (mac == String.Empty)
                        {
                            if ((bool)item["IPEnabled"] == true)
                            {
                                mac = item["MACAddress"].ToString();
                                String[] ips = (String[]) item["IPAddress"];
                                ip = ips[0];
                            }
                        }
                    }
                    computerInformation.IpAddress = ip;
                    computerInformation.MacAddress = mac;
                }
                catch (Exception)
                {
                    //ignore
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
                    //ignore
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
                    //ignore
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
                    //ignore
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
                    //ignore
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
                //ignore
            }

            return computerInformation;
        }
    }
}
