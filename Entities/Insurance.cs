using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proiect_paw_2.Entities
{
    public class Insurance
    {
        public int Insurance_Id { get; set; }
        public DateTime Start_Date { get; set;}
        public DateTime End_Date { get; set;}
        public int Sum { get; set;}

        public Insurance()
        {
            Start_Date = DateTime.Now;
            End_Date = DateTime.Now;
        }

        public Insurance(int insurance_Id, DateTime start_Date, DateTime end_Date, int sum)
        {
            Insurance_Id = insurance_Id;
            Start_Date = start_Date;
            End_Date = end_Date;
            Sum = sum;
        }

        // Method to export insurance report to a text file
        public void ExportReport(string filePath)
        {
            string reportContent = GenerateReport();
            File.WriteAllText(filePath, reportContent);
        }

        // Generate report content as a string
        private string GenerateReport()
        {
            return $"Insurance Report\n" +
                   $"----------------\n" +
                   $"Insurance ID: {Insurance_Id}\n" +
                   $"Start Date: {Start_Date.ToShortDateString()}\n" +
                   $"End Date: {End_Date.ToShortDateString()}\n" +
                   $"Sum: {Sum}\n";
        }
    }
}
