using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;
using System.Management; 

namespace IA_Run
{

    class Program
    {

        public static string ReadPassword()

        {

            string password = "";

            ConsoleKeyInfo info = Console.ReadKey(true);

            while (info.Key != ConsoleKey.Enter)

            {

                if (info.Key != ConsoleKey.Backspace)

                {

                    Console.Write("*");

                    password += info.KeyChar;

                }

                else if (info.Key == ConsoleKey.Backspace)

                {

                    if (!string.IsNullOrEmpty(password))

                    {

                        // remove one character from the list of password characters

                        password = password.Substring(0, password.Length - 1);

                        // get the location of the cursor

                        int pos = Console.CursorLeft;

                        // move the cursor to the left by one character

                        Console.SetCursorPosition(pos - 1, Console.CursorTop);

                        // replace it with space

                        Console.Write(" ");

                        // move the cursor to the left by one character again

                        Console.SetCursorPosition(pos - 1, Console.CursorTop);

                    }

                }

                info = Console.ReadKey(true);

            }

            // add a new line because user pressed enter at the end of their password

            Console.WriteLine();

            return password;

        }
        static void Main(string[] args)
        {
            {
                Console.WriteLine("Running Inventory Assesment Application-RMG Networks Holding Corporation.");
                Console.WriteLine("-------------------------------------------------------------------------");
                // Prepare [IA-DB]
                if (Model.executionFlag)
                {
                    Console.Write(@"sa password = ");
                    Model.SAPassword = ReadPassword();
                    Console.Write(@"Sql Server Instance (ex. RMGDXB-125\SQLSYMON)  = ");
                    Model.ServerName = Console.ReadLine();
                    Model.executeSQL(Model.createDbSQL, "Prepare [IA-DB]");
                    Console.WriteLine("-------------------------------------------------------------------------");
                }

                // Initialize SQL Views
                if (Model.executionFlag)
                {
                    Model.executeSQL((File.ReadAllText(Model.direcotyUNC + Model.queryFileUNC)), "Initialize SQL Views");
                    Console.WriteLine("-------------------------------------------------------------------------");
                }

                // Get Customer Info.
                if (Model.executionFlag)
                {
                    // Delete CustomerinfoHelper.writeToDB();
                    Model.executeSQL(Model.createCustomerinfoSQL, "Get Customer Info....");
                    Console.WriteLine("-------------------------------------------------------------------------");
                }

                // Get RMG Server Info
                if (Model.executionFlag)
                {
                    //Delete SysteminfoHelper.writeToDB();
                    Model.executeSQL(Model.createSysteminfoSQL, "Get RMG Server Info....");
                    Console.WriteLine("-------------------------------------------------------------------------");
                }

                // installRSS
                if (Model.executionFlag)
                {

                    Model.installSSRS();
                    Console.WriteLine("-------------------------------------------------------------------------");
                }

                // runSSRS
                if (Model.executionFlag)
                {
                    //Delete LaunchSSRS.runSSRS();
                    Model.runSSRS();
                    Console.WriteLine("-------------------------------------------------------------------------");
                }

                // Final Execution
                if (Model.executionFlag)
                {
                    Console.ReadLine();
                    Console.Write("Closing, Thank You.");
                    System.Threading.Thread.Sleep(1250);
                }
                else
                {
                    Console.WriteLine("Error, please try again....!!");
                    Console.Write("Press any key to close");
                    Console.ReadLine();
                }
            }

            //Console.WriteLine(Systeminfo.Free_Space_OS_Drive);
            //Console.WriteLine(Systeminfo.getValue);
            //Console.ReadLine();
        }
    }
}