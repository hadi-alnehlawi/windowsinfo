using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_Run
{
    public static class CustomerinfoHelper
    {
        public static void writeToDB() {
            Console.WriteLine("Analyse RMG Server Data:");
            String serverName = @"RMGDXB-125\SQLSYMON";
            String databaseName = @"IA-DB";
            String saPassword = @"skr0wteN.GMR"; 
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            SqlDataReader dataReader;
            connetionString = "Data Source=" + serverName + @";Initial Catalog=" + databaseName + ";User ID=sa;Password=" + saPassword;
            Console.WriteLine(connetionString);
            sql = @"use [IA-DB];"
                 // Update [Customerinfo] Table
                 + @"delete from [Customerinfo]; "
                 + @"insert into Customerinfo (Company, ContactName, Telephone, IALinkInSF, Summary, AreaOfConcern, Recomendation, RMGNetworksAnalyst ) values"
                 + @" ('" + Customerinfo.company + @"'"
                 + @"," + @"'" + Customerinfo.contanctName+ @"'"
                 + @"," + @"'" + Customerinfo.telephone + @"'"
                 + @"," + @"'" + Customerinfo.inventoryAssesmentSF + @"'"
                 + @"," + @"'" + Customerinfo.summary + @"'"
                 + @"," + @"'" + Customerinfo.areaOfConcers + @"'"
                 + @"," + @"'" + Customerinfo.recomendations + @"'"
                 + @"," + @"'" + Customerinfo.rmgAnalyst + @"'"
                 + @");"
                  ;

            connection = new SqlConnection(connetionString);
            try
            {
                connection.Open();
                command = new SqlCommand(sql, connection);
                dataReader = command.ExecuteReader();
                dataReader.Close();
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can not open connection ! ", ex);
            }

        }
    }
}
