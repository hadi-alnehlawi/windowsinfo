using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;
using System.Management; 

namespace WindowsInfo
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
                Console.WriteLine("Analysing Windows Operating System Info:");
                Console.WriteLine(String.Concat(Enumerable.Repeat("-", ("Analysing Windows Operating System Info:").Length)));
                Console.WriteLine();
                Console.WriteLine(Systeminfo.Processors);
                Console.WriteLine();
                Console.WriteLine(Systeminfo.Number_Of_Cores);
                Console.WriteLine();
                Console.WriteLine(Systeminfo.Number_Of_Logical_Processors);
                Console.WriteLine();
                Console.WriteLine(Systeminfo.Number_Of_Processor_Sockets);
                Console.WriteLine();
                Console.WriteLine(Systeminfo.Processor_Usage);
                Console.WriteLine();
                Console.WriteLine(Systeminfo.OS_Name);
                Console.WriteLine();
                Console.WriteLine(Systeminfo.Free_Space_OS_Drive);
                Console.WriteLine();
                Console.WriteLine(Systeminfo.Disk_Write_Time);
                Console.WriteLine();
                Console.WriteLine(Systeminfo.Processes);
                Console.WriteLine();
                Console.WriteLine(Systeminfo.Handles);
                Console.WriteLine();
                Console.WriteLine(Systeminfo.Threads);
                Console.WriteLine();
                Console.WriteLine(Systeminfo.Total_Physical_Memory);
                Console.WriteLine();
                Console.WriteLine(Systeminfo.Available_Physical_Memory);
                Console.WriteLine();
                Console.WriteLine(Systeminfo.Cache_Memory);
                Console.WriteLine();
                Console.WriteLine(Systeminfo.Free_Physical_Memory);
                Console.WriteLine();
                

            }

            //Console.WriteLine(Systeminfo.Free_Space_OS_Drive);
            //Console.WriteLine(Systeminfo.getValue);
            Console.Write("Press any key to close");
            Console.ReadLine();
        }
    }
}