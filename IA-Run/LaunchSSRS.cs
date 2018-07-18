using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace IA_Run
{
    class LaunchSSRS
    {
        public static string direcotyUNC = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public static string ssrcExeUNC = @"C:\\Program Files (x86)\Microsoft SQL Server\Report Builder 3.0\MSReportBuilder.exe";
        public static string ssrsFileName = "IA-Report.rdl";
        //public static void DeleteLater()
        //{
        //    Console.WriteLine("Launching SqlServer Report File....");
        //    ProcessStartInfo ssrcProcInfo = new ProcessStartInfo(@"MSReportBuilder", direcotyUNC+@"\"+ssrsFileName);
        //    Process ssrcProc = new Process();
        //    ssrcProc.StartInfo.UseShellExecute = false;
        //    ssrcProc.StartInfo = ssrcProcInfo;
        //    //Start the process
        //    ssrcProc.Start();
        //    ssrcProc.WaitForExit();
        //}

        public static void runSSRS()
        {
            if (File.Exists(ssrcExeUNC))
            {
                try
                {
                    Console.WriteLine("Launching SqlServer Report File....");
                    Process ssrcProc = new Process();
                    ssrcProc.StartInfo.UseShellExecute = false;
                    ssrcProc.StartInfo.FileName = ssrcExeUNC;
                    ssrcProc.StartInfo.Arguments = direcotyUNC + @"\" + ssrsFileName;
                    ssrcProc.StartInfo.CreateNoWindow = true;
                    ssrcProc.Start();
                    ssrcProc.WaitForExit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine(@"Error loading the report , please click 'setpup.exe' before running this file....");
            }
            Console.WriteLine("press any key to close");
            Console.Read();

        }

    }
    
}
