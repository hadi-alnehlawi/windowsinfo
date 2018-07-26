using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Management;
using System.Runtime.Caching;
using System.Diagnostics;
using System.Timers;
using Microsoft.Web.Administration;

namespace WindowsInfo
{
    public static class Systeminfo
    {
        public static int intendSpace = 30;
        public static string OS_Name
        {
            get
            {
                Console.Write("OS Name: ");
                Console.Write(String.Concat(Enumerable.Repeat(" ", intendSpace - ("OS Name: ").Length)));
                var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                                   select x.GetPropertyValue("Caption")).FirstOrDefault();
                return name != null ? name.ToString() : "Unknown";
            }
            set
            {
       
            }
        }
        public static string Free_Space_OS_Drive
        {
            get
            {
                string result = "";
                Console.Write("Free Space OS Drive: ");
                Console.Write(String.Concat(Enumerable.Repeat(" ", intendSpace - ("Free Space OS Drive: ").Length)));
                string SystemDrive = "";

                //Get SystemDrive
                string key = "Win32_OperatingSystem";
                List<string> DeviceIDs = new List<string>();
                ManagementObjectSearcher searcherOSDrive = new ManagementObjectSearcher("select * from " + key);
                foreach (ManagementObject share in searcherOSDrive.Get())
                {
                    var prop = share.Properties["SystemDrive"];
                    SystemDrive = prop.Value.ToString();
                }

                // Do Calculate Free Size 
                key = "Win32_logicaldisk";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key);
                foreach (ManagementObject share in searcher.Get())
                {
                    if (share["Caption"].ToString() == SystemDrive)
                    {
                        foreach (var prop in share.Properties)
                        {
                            if (prop.Name == "FreeSpace")
                            {
                                result = Math.Round(((Convert.ToDouble(prop.Value)) / 1024 / 1024 / 1024), 2).ToString() + " GB";
                            }
                        }
                    }
                }
                //Console.WriteLine("Ok");
                return result;
            }
            set
            {
            }
        }
        public static string Disk_Write_Time
        {
            get
            {
                string result = String.Concat(Enumerable.Repeat(" ", 52 - ("Disk [C:] Write Time: ").Length));
                string key = "Win32_logicaldisk";
                List<string> DeviceIDs = new List<string>();
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key);
                foreach (ManagementObject share in searcher.Get())
                {
                    foreach (var prop in share.Properties)
                    {
                        if (prop.Name == "DeviceID")
                        {
                            DeviceIDs.Add(prop.Value.ToString());
                        }
                    }
                }
                PerformanceCounterCategory cat = new PerformanceCounterCategory("PhysicalDisk");
                string[] instances = cat.GetInstanceNames();
                foreach (string deviceid in DeviceIDs)
                {
                    foreach (string instance in instances)
                    {
                        if (instance.Contains(deviceid))
                        {
                            PerformanceCounter pc1 = new PerformanceCounter("PhysicalDisk", "% Disk Write Time", instance);
                            pc1.NextValue();
                            Console.WriteLine("Disk [" + deviceid + "] Write Time: ");
                            System.Threading.Thread.Sleep(2000);
                            //Console.WriteLine("Ok");
                            result += "(" + deviceid + " " + Math.Round(pc1.NextValue(), 2).ToString() + "% )";
                        }
                    }
                }
                
                return result;
            }
            set { }
        }
        public static string Processors
        {
            get
            {
                Console.Write("Processes: ");
                Console.Write(String.Concat(Enumerable.Repeat(" ", intendSpace - ("Processes: ").Length)));
                string result = "";
                string key = "Win32_Processor";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key);
                foreach (ManagementObject share in searcher.Get())
                {
                    foreach (var prop in share.Properties)
                    {
                        if (prop.Name == "Name")
                        {
                            result += prop.Value;
                        }
                    }
                }
                //Console.WriteLine("Ok");
                return result;
            }
            set { }
        }
        public static string Processor_Usage
        {
            get
            {
                string result = "";
                PerformanceCounter pc1 = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                pc1.NextValue();
                Console.Write("CPU Utalization: ");
                Console.Write(String.Concat(Enumerable.Repeat(" ", intendSpace - ("CPU Utalization: ").Length)));
                System.Threading.Thread.Sleep(2000);
                //Console.WriteLine("Ok");
                result = (Math.Round(pc1.NextValue(), 0)).ToString() + " %";
                return result;
                
               
            }
            set { }
        }
        public static string Number_Of_Cores
        {
            get
            {
                Console.Write("No Of CPU Cores: ");
                Console.Write(String.Concat(Enumerable.Repeat(" ", intendSpace - ("No Of CPU Cores: ").Length)));
                string result = "";
                string key = "Win32_Processor";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key);
                foreach (ManagementObject share in searcher.Get())
                {
                    foreach (var prop in share.Properties)
                    {
                        if (prop.Name == "NumberOfCores")
                        {
                            result += prop.Value;
                        }
                    }
                }
                //Console.WriteLine("Ok");
                return result;
            }
            set { }
        }
        public static string Number_Of_Logical_Processors
        {
            get
            {
                Console.Write("No of Logical Processors: ");
                Console.Write(String.Concat(Enumerable.Repeat(" ", intendSpace - ("No of Logical Processors: ").Length)));
                string result = "";
                string key = "Win32_Processor";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key);
                foreach (ManagementObject share in searcher.Get())
                {
                    foreach (var prop in share.Properties)
                    {
                        if (prop.Name == "NumberOfLogicalProcessors")
                        {
                            result += prop.Value;
                        }
                    }
                }
                //Console.WriteLine("Ok");
                return result;
            }
            set
            {

            }
        }
        public static string Total_Physical_Memory
        {
            get
            {
                Console.Write("Total Physical Memory :");
                Console.Write(String.Concat(Enumerable.Repeat(" ", intendSpace - ("Total Physical Memory :").Length)));
                string key = "Win32_PhysicalMemory";
                long returnValue = 0;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key);
                foreach (ManagementObject share in searcher.Get())
                {
                    foreach (var prop in share.Properties)
                    {
                        if (prop.Name == "Capacity")
                        {

                            returnValue += Convert.ToInt64(prop.Value) / 1024 / 1024 / 1024;
                        }
                    }
                }
                //Console.WriteLine("Ok");
                return returnValue.ToString() + " GB";
            }
            set { }
        }
        public static string Available_Physical_Memory
        {

           get
            {
                Console.Write("Avaialble Physical Memory:");
                Console.Write(String.Concat(Enumerable.Repeat(" ", intendSpace - ("Avaialble Physical Memory:").Length)));
                PerformanceCounter pc = new PerformanceCounter("Memory", "Available MBytes");
                //, "% Committed Bytes In Use");
                //,"System Cache Resident Bytes"); //, "Available MBytes");
                //Console.WriteLine("Ok");
                return (Math.Round((pc.NextValue() / 1024), 2)).ToString() + " GB";
            }
            set { }
        }
        public static string Cache_Memory
        {
            get
            {
                Console.Write("Cache Memory:");
                Console.Write(String.Concat(Enumerable.Repeat(" ", intendSpace - ("Cache Memory:").Length)));
                string result = "";
                string key = "Win32_PerfFormattedData_PerfOS_Memory";
                List<string> DeviceIDs = new List<string>();
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key);
                foreach (ManagementObject share in searcher.Get())
                {
                    foreach (var prop in share.Properties)
                    {
                        if (prop.Name == "CacheBytes")
                            result += Math.Round(((Convert.ToDouble(prop.Value)) / 1024 / 1024), 2).ToString() + " MB";
                    }
                }
                //Console.WriteLine("Ok");
                return result;
            }
            set { }
        }
        public static string Processes
        {
            get
            {
                Console.Write("No of Windows Processes: ");
                Console.Write(String.Concat(Enumerable.Repeat(" ", intendSpace - ("No of Windows Processes: ").Length)));
                PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");
                string[] instances = cat.GetInstanceNames();
                //Console.WriteLine("Ok");
                return instances.Length.ToString();

            }
            set
            {

            }
        }
        public static string Handles
        {
            get
            {
                Console.Write("No of Handles:");
                Console.Write(String.Concat(Enumerable.Repeat(" ", intendSpace - ("No of Handles:").Length)));
                int result = 0;
                string key = "Win32_Process";
                List<string> DeviceIDs = new List<string>();
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key);
                foreach (ManagementObject share in searcher.Get())
                {
                    foreach (var prop in share.Properties)
                    {
                        if (prop.Name == "HandleCount")
                        {
                            result += Convert.ToInt32(prop.Value);
                            //Console.WriteLine(prop.Value);
                            //Console.WriteLine(Math.Round(((Convert.ToDouble(prop.Value)) / 1024 / 1024), 2).ToString() + " GB");
                        }
                    }
                }
                //Console.WriteLine("Ok");

                return result.ToString();
            }
            set { }
        }
        public static string Threads
        {
            get {
                Console.Write("No of Threads: ");
                Console.Write(String.Concat(Enumerable.Repeat(" ", intendSpace - ("No of Threads: ".Length))));
                int result = 0;
                string key = "Win32_Process";
                List<string> DeviceIDs = new List<string>();
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key);
                foreach (ManagementObject share in searcher.Get())
                {
                    foreach (var prop in share.Properties)
                    {
                        if (prop.Name == "ThreadCount")
                        {
                            result += Convert.ToInt32(prop.Value);
                            //Console.WriteLine(prop.Value);
                            //Console.WriteLine(Math.Round(((Convert.ToDouble(prop.Value)) / 1024 / 1024), 2).ToString() + " GB");
                        }
                    }
                }
                //Console.WriteLine("Ok");
                return result.ToString();
            }
            set { }
        }
        public static string Up_Time
        {
            get
            {
                Console.Write("Calculating Up Time...");
                string result = "";
                string key = "Win32_OperatingSystem";
                List<string> DeviceIDs = new List<string>();
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key);
                foreach (ManagementObject share in searcher.Get())
                { 
                    foreach (var prop in share.Properties)
                    {
                        if (prop.Name == "LastBootUpTime")
                        {
                            //result += Convert.ToInt32(prop.Value);
                            DateTime lastBootUp = ManagementDateTimeConverter.ToDateTime(prop.Value.ToString());
                            TimeSpan timeStamp = DateTime.Now.ToUniversalTime() - lastBootUp.ToUniversalTime();
                            result = timeStamp.ToString();

                            //Console.WriteLine(Math.Round(((Convert.ToDouble(prop.Value)) / 1024 / 1024), 2).ToString() + " GB");
                        }
                    }
                }
                Console.WriteLine("Ok");
                return result;
            }
            set { }

        }
        public static string Free_Physical_Memory
        {
            get
            {
                Console.Write("Free Physical Memory: ");
                Console.Write(String.Concat(Enumerable.Repeat(" ", intendSpace - ("Free Physical Memory: ").Length)));
                Double result = 0.0 ;
                string key = "Win32_PerfRawData_PerfOS_Memory";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key);

                foreach (ManagementObject share in searcher.Get())
                {
                    result = Convert.ToDouble(share["FreeAndZeroPageListBytes"]) / 1024 / 1024;
                    result = Math.Round(result, 0);
                }
                //Console.WriteLine("Ok");
                return result.ToString() + " MB";
            }
            set { }
        }
        public static string Number_Of_Processor_Sockets     
            {
            get {
                string result = "0";
                Console.Write("Get Number Of CPU Sockets: ");
                Console.Write(String.Concat(Enumerable.Repeat(" ", intendSpace - ("Get Number Of CPU Sockets: ").Length)));
                string query;
                ManagementObjectSearcher searcher;

                // Get the number of physical processors.
                int num_physical_processors = 0;
                query = "SELECT * FROM Win32_ComputerSystem";
                searcher = new ManagementObjectSearcher(query);
                foreach (var sys in searcher.Get())
                {
                    num_physical_processors =
                    num_physical_processors =
                        int.Parse(sys["NumberOfProcessors"].ToString());
                }
                result = Convert.ToString(num_physical_processors);
                
                return result.ToString(); 
                }
            set {}
            
            }

        public static string getValue
        {
            //PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");
            //string[] instances = cat.GetInstanceNames();
            //Console.WriteLine( instances.Length.ToString());
            //string result = "";
            //Console.WriteLine("Calculating Threads...");
            //string SystemDrive = "";
            //string key = "Win32_OperatingSystem";
            //List<string> DeviceIDs = new List<string>();
            //ManagementObjectSearcher searcherOSDrive = new ManagementObjectSearcher("select * from " + key);
            //foreach (ManagementObject share in searcherOSDrive.Get())
            //{
            //    var prop = share.Properties["SystemDrive"];
            //    SystemDrive = prop.Value.ToString();
            //}
            //string key = "Win32_PerfRawData_PerfOS_Memory";
            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + key);
            //foreach (ManagementObject share in searcher.Get())
            //{
            //    Double result = Convert.ToDouble(share["FreeAndZeroPageListBytes"]) / 1024 / 1024 ;
            //    result = Math.Round(result, 0);
            //    Console.WriteLine( result.ToString());
            get { 
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            return name != null ? name.ToString() : "Unknown";
            }
            set { }



        }


    }
}

