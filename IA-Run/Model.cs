using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IA_Run
{
    class Model
    {
        public static bool executionFlag = true;

        public static string direcotyUNC
        {
            get
            {
                return System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }
            set { }
        }

        public static string ssrcExeUNC
        {
            get
            {
                return @"C:\\Program Files (x86)\Microsoft SQL Server\Report Builder 3.0\MSReportBuilder.exe";
            }
            set { }
        }

        public static string ssrsFileName = "IA-Report.rdl";

        public static string createDbSQL
        {
            get
            {
                string result = "";
                result = @"
                -- Create IA-DB Databae
                IF EXISTS(select * from sys.databases where name='IA-DB')
                drop database [IA-DB];

                CREATE DATABASE [IA-DB];
                ALTER DATABASE [IA-DB] SET COMPATIBILITY_LEVEL = 100;

                declare @collation nvarchar(max);

                SELECT  @collation = [collation_name] FROM sys.databases
                where [name] = 'SymonData';

                execute(
                'Alter DATABASE [IA-DB] collate '+ @collation);
                ";
                return result;
            }
            set { }
        }

        public static string createSysteminfoSQL
        {
            get
            {
                string result = @"
                        use[IA-DB];"
                         // Update [Systeminfo] Table
                         + @"delete from [Systeminfo];"
                         + @"insert into Systeminfo (OS_Name, UP_Time, Free_Space_OS_Drive, Free_Space_RMG_Drive, Disk_Write_Time, Processors, Number_Of_Cores, Number_Of_Logical_Processors, Processor_Usage,Processes, Handles, Threads, Total_Physical_Memory, Available_Physical_Memory, Free_Physical_Memory,  Cache_Memory) values"
                         + @" ('" + Systeminfo.OS_Name + @"'"
                         + @"," + @"'" + Systeminfo.Up_Time + @"'"
                         + @"," + @"'" + Systeminfo.Free_Space_OS_Drive + @"'"
                         + @"," + @"'" + Systeminfo.Free_Space_RMG_Drive + @"'"
                         + @"," + @"'" + Systeminfo.Disk_Write_Time + @"'"
                         + @"," + @"'" + Systeminfo.Processors + @"'"
                         + @"," + @"'" + Systeminfo.Number_Of_Cores + @"'"
                         + @"," + @"'" + Systeminfo.Number_Of_Logical_Processors + @"'"
                         + @"," + @"'" + Systeminfo.Processor_Usage + @"'"
                         + @"," + @"'" + Systeminfo.Processes + @"'"
                         + @"," + @"'" + Systeminfo.Handles + @"'"
                         + @"," + @"'" + Systeminfo.Threads + @"'"
                         + @"," + @"'" + Systeminfo.Total_Physical_Memory + @"'"
                         + @"," + @"'" + Systeminfo.Available_Physical_Memory + @"'"
                         + @"," + @"'" + Systeminfo.Free_Physical_Memory + @"'"
                         + @"," + @"'" + Systeminfo.Cache_Memory + @"'"
                         + @")"
                         // Update [SQLServerinfo] Table
                         //P.C table is used coz the data coming from sp_who2
                         + @"delete from [Sqlerverprocesses];"
                         + @"insert INTO [Sqlerverprocesses] EXEC  sp_who2;"
                         + @"delete from [Sqlerverprocesses] Where [SPID] < 51;"
                          ;
                return result;
            }
        }

        public static string createCustomerinfoSQL
        {
            get
            {
                string sql = @"use [IA-DB];"
                             // Update [Customerinfo] Table
                             + @"delete from [Customerinfo]; "
                             + @"insert into Customerinfo (Company, ContactName, Telephone, IALinkInSF, Summary, AreaOfConcern, Recomendation, RMGNetworksAnalyst ) values"
                             + @" ('" + Customerinfo.company + @"'"
                             + @"," + @"'" + Customerinfo.contanctName + @"'"
                             + @"," + @"'" + Customerinfo.telephone + @"'"
                             + @"," + @"'" + Customerinfo.inventoryAssesmentSF + @"'"
                             + @"," + @"'" + Customerinfo.summary + @"'"
                             + @"," + @"'" + Customerinfo.areaOfConcers + @"'"
                             + @"," + @"'" + Customerinfo.recomendations + @"'"
                             + @"," + @"'" + Customerinfo.rmgAnalyst + @"'"
                             + @");";
                return sql;
            }
            set { }
        }

        public static string queryFileUNC
        {
            get
            {
                return "/IA-Report-SQL.sql";
            }
            set { }
        }

        private static string databaseName = @"master";
        public static string DatabaseName
        {
            get { return databaseName; }
            set
            {
                databaseName = value;
            }
        }

        public static string ServerName { get; set; } = @"RMGDXB-125\SQLSYMON";

        public static string SAPassword { get; set; } = @"skr0wteN.GMR";

        public static string ConctionString
        {
            get
            {
                string result = "Data Source=" + Model.ServerName + @";Initial Catalog=" + Model.DatabaseName + ";User ID=sa;Password=" + Model.SAPassword;
                return result;
            }
            set
            {

            }
        }

        public static void executeSQL(string sqlCmd, string jobName)
        {
            Console.Write(jobName + "....");
            SqlConnection connection;
            SqlCommand command;
            SqlDataReader dataReader;
            connection = new SqlConnection(Model.ConctionString);
            //sqlCmd = File.ReadAllText(Model.direcotyUNC + Model.queryFileUNC);
            try
            {
                connection.Open();
                command = new SqlCommand(sqlCmd, connection);
                dataReader = command.ExecuteReader();
                dataReader.Close();
                command.Dispose();
                connection.Close();
                Console.WriteLine("Completed Successfully");
            }
            catch (Exception ex)
            {
                connection.Close();
                Console.WriteLine(ex.Message);
                Model.executionFlag = false;
            }
        }

        public static void runSSRS()
        {
            if (File.Exists(Model.ssrcExeUNC))
            {
                string ssrcConnectString = @"Data Source=" + Model.ServerName + ";Initial Catalog=IA-DB";
                Model.changeSSRSConnectString(ssrcConnectString);
                try
                {
                    Console.WriteLine("Launching SqlServer Report File....");
                    Process ssrcProc = new Process();
                    ssrcProc.StartInfo.UseShellExecute = false;
                    ssrcProc.StartInfo.FileName = ssrcExeUNC;
                    ssrcProc.StartInfo.Arguments = direcotyUNC + @"\" + Model.ssrsFileName;
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

        public static void changeSSRSConnectString(string ssrcConnectString)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            try
            {
                doc.Load(Model.direcotyUNC + @"\" + Model.ssrsFileName);
                foreach (XmlNode n1 in doc.ChildNodes)
                {
                    if (n1.Name.ToLower().Contains("report"))
                    {
                        foreach (XmlNode n2 in n1.ChildNodes)
                        {
                            if (n2.Name.ToLower().Contains("datasource"))
                            {
                                foreach (XmlNode n3 in n2.ChildNodes)
                                {
                                    if (n3.Name.ToLower().Contains("datasource"))
                                    {
                                        foreach (XmlNode n4 in n3.ChildNodes)
                                        {
                                            if (n4.Name.Contains("ConnectionProperties"))
                                            {
                                                foreach (XmlNode n5 in n4.ChildNodes)
                                                {
                                                    if (n5.Name.Contains("ConnectString"))
                                                    {
                                                        string newConnectStr = ssrcConnectString;
                                                        n5.InnerText = newConnectStr;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                doc.Save(direcotyUNC + @"\" + Model.ssrsFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void installSSRS()
        {
            // ReportBuilder3.msi  must be exisited in the execution folder
            string cmdText = "/i ReportBuilder3.msi /quiet /qr /l* log.txt ";
            Console.Write("Installing SQL Server Report Builder....");
            ProcessStartInfo ssrcProcInfo = new ProcessStartInfo("msiexec");
            Process ssrcProc = new Process();
            ssrcProc.StartInfo.UseShellExecute = true;
            ssrcProcInfo.Arguments = cmdText;
            ssrcProc.StartInfo = ssrcProcInfo;
            //Start the process
            ssrcProc.Start();
            ssrcProc.WaitForExit();
            Console.WriteLine("Ok");
            Console.WriteLine("-------------------------------------------------------------------------");

        }

        

    }
}


