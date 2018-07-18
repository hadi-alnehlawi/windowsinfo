using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IA_Run
{
    public static class Customerinfo
    {
        public static string company
        {
            get
            {
                Console.Write("Company = ");
                string result = Console.ReadLine();
                return result ;
            }
            set {
                
            }
        }
        public static string contanctName
        {
            get
            {
                Console.Write("Contanct Name = ");
                string result = Console.ReadLine();
                return result;
            }
            set { }
        }
        public static string telephone
        {
            get
            {
                Console.Write("Telephone = ");
                string result = Console.ReadLine();
                return result;
            }
            set { }
        }
        public static string inventoryAssesmentSF
        {
            get
            {
                Console.Write("Inventory Assesment SalesFore Opportunity = ");
                string result = Console.ReadLine();
                return result;

            }
            set { }
        }
        public static string rmgAnalyst
        {
            get
            {
                Console.Write("RMG Networsk Analyst = ");
                string result = Console.ReadLine();
                return result;

            }
            set { }
        }
        public static string summary
        {
            get
            {
                Console.Write("Summary = ");
                string result = Console.ReadLine();
                return result;

            }
            set { }
        }
        public static string areaOfConcers
        {
            get
            {
                Console.Write("Area Of Concerns = ");
                string result = Console.ReadLine();
                return result;

            }
            set { }
        }
        public static string recomendations
        {
            get
            {
                Console.Write("Recomendations = ");
                string result = Console.ReadLine();
                return result;
            }
            set { }
        }

    }
}
