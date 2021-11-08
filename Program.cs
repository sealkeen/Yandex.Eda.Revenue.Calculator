using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CountCourierRublesPerHour
{
    public class Program
    {
        private static double totalMoney = 0.0, totalHours = 0.0, totalOrders = 0.0, totalFines = 0.0;
        private static string header = "\tDate\tSalary\t\t Hours\t\tРуб/Час\t\tOrders\tРуб/Зак Fines (Rub.):";
        static string lastPath = ""; //static List<Dictionary<double, double>> moneyAndHours = new List<Dictionary<double, double>>();
        static WeekRange weekRange;

        [STAThread]
        static void Main(string[] args)
        {
            string path = "";
            do {
                Initialize();
                Console.WriteLine("Enter the path to finances comma separated :");

                if ( File.Exists(lastPath) )
                    System.Windows.Forms.Clipboard.SetText(lastPath);
                path = Console.ReadLine();
                if (File.Exists(path))
                    WriteLastPath(path);

                if ( File.Exists(path) ) {
                    try {
                        ReceiveMoney(path);
                        CountResultingWeekFinances();
                        ShowEachWeekFinances();
                        ShowResultingWeekFinances();
                        ShowAverageWeekFinances();
                    } catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                }
            } while (true);
        }

        static void Initialize()
        {
            lastPath = "";
            /* moneyAndHours = new List<Dictionary<double, double>>();*/
            weekRange = new WeekRange(1);
            ReadLastPath();
        }

        static void ReadLastPath()
        {
            if (File.Exists("lastPath.txt")) {
                StreamReader sR = new StreamReader("lastPath.txt");
                lastPath = sR.ReadLine();
                if (!File.Exists(lastPath))
                    lastPath = "";
                sR.Close();
            }
        }

        static void WriteLastPath(string lP)
        {
            StreamWriter sW = new StreamWriter("lastPath.txt", false);
            //if (File.Exists(lP))
                sW.Write(lP);
            sW.Close();
        }

        static WeekInfo ReceiveNewWeekMoneyAndHours(string commaSeparatedMoneyAndHours)
        {
            WeekInfo weekInfo = new WeekInfo();
            DisassembleString(commaSeparatedMoneyAndHours, weekInfo);
            //Dictionary<double, double> result = new Dictionary<double, double>();
            //result.Add(money, hours);

            return weekInfo;
        }

        private static void DisassembleString( string commaSeparatedMoneyAndHours, 
            WeekInfo weekInfo )
        {
            if (commaSeparatedMoneyAndHours == string.Empty) {
                throw new ArgumentNullException("String week Money / Hours is Empty.");
            }
            if (commaSeparatedMoneyAndHours[0] < '0' || commaSeparatedMoneyAndHours[0] > '9')
                throw new ArgumentNullException("First string character is not a number.");
            string[] moneyAndHours = commaSeparatedMoneyAndHours.Split(',');
            if (moneyAndHours.Length < 2)
                throw new Exception("New String does not contain the amount of money and hours");
            try {
                weekInfo.salary = Convert.ToDouble(moneyAndHours[0]);
                weekInfo.hours = Convert.ToDouble(moneyAndHours[1]);
            try {
                weekInfo.fines = Convert.ToDouble(moneyAndHours[2]); 
                weekInfo.orderCount = Convert.ToInt32(moneyAndHours[3]); 
                weekInfo.startDay = Convert.ToInt32(moneyAndHours[4]);
                weekInfo.startMonth = Convert.ToInt32(moneyAndHours[5]);
                weekInfo.endDay = Convert.ToInt32(moneyAndHours[6]);    
                weekInfo.endMonth = Convert.ToInt32(moneyAndHours[7]);
            } catch {
                    //TODO: Handle
                }
            } catch {
                throw new ArgumentException("Hours and Money don't have the proper format.");
            }
        }

        static void ReceiveMoney(string filePath) {
            StreamReader sR = new StreamReader(filePath);
            string[] contents = sR.ReadToEnd().Split('\n');
            foreach (string moneyHoursLine in contents) {
                try {
                    /**/ weekRange.AddWeek(ReceiveNewWeekMoneyAndHours(moneyHoursLine));
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            sR.Close();
        }

        static void ShowEachWeekFinances()
        {
            string result = "";
            foreach (WeekInfo weekFinances in /**/ weekRange) {
                var week = weekFinances;
                result +=
                    "\t" + GetMonthByNumber(week.startMonth) + week.startDay +
                    "\t" + (week.salary + " P.").PadRight(totalMoney.ToString().Length+3, ' ') +
                    "\t" + (week.hours + "ч ").PadRight(totalHours.ToString().Length + 3, ' ') +
                    "\t\t" + week.GetAverageRevenuePerHour();

                if (week.orderCount >= 0) {
                    result +=
                        "\t"/*\t*/ + week.orderCount +
                        "\t"/*\t*/ + week.GetAverageRevenuePerOrder() +
                        "\t" + week.fines;
                }
                result += "\n";
            }

            Console.WriteLine(header);
            Console.WriteLine(result);
        }

        static void ShowWeeksAndMonthsCount()
        {
            Console.WriteLine($"Weeks total: {weekRange.Count}. ");
        }

        static void ShowAverageWeekFinances() {
            double averageMoney = 0.0, averageHours = 0.0, 
                avgRubPerHour = 0.0, avgRubPerOrder = 0.0,
                avgOrders = 0.0;
            foreach (WeekInfo week in weekRange) {
                averageMoney += week.salary;
                averageHours += week.hours;
                avgRubPerHour += (week.salary / week.hours);
                avgRubPerOrder += (week.salary / week.orderCount);
                avgOrders += week.orderCount;
            }
            averageMoney /= weekRange.Count;
            averageHours /= weekRange.Count;
            avgRubPerHour /= weekRange.Count;
            avgRubPerOrder /= weekRange.Count;
            avgOrders /= weekRange.Count;
            string result = "[avg.]" +
                "\t\t" + Math.Round(averageMoney, 3).ToString() + " р." +
                "\t" + Math.Round(averageHours, 3).ToString() + "ч" +
                "\t\t" + Math.Round(avgRubPerHour, 3).ToString() +
                "\t\t" + Math.Round(avgOrders, 3).ToString() +
                "\t" + Math.Round(avgRubPerOrder, 3).ToString(); ;
            Console.WriteLine(result);
        }
        static void CountResultingWeekFinances()
        {
            foreach (WeekInfo weekFinances in /**/ weekRange) {
                var week = weekFinances;
                totalMoney += week.salary;
                totalHours += week.hours;
                totalOrders += week.orderCount;
                totalFines += week.fines;
            }
        }
        static void ShowResultingWeekFinances()
        {
            string result = "[total]" +
                "\t\t" + totalMoney.ToString() + " Р." +
                "\t" + totalHours.ToString() + "ч" +
                "\t\t\t" /*\t\t\t*/ + totalOrders +
                "\t\t" + (-totalFines).ToString();

            Console.WriteLine(header);
            Console.WriteLine(result);
        }

        static void ShowAveragePerOrderRevenue()
        {
            
        }


        static string GetMonthByNumber(int number) {
            switch (number) {
                case 1: return "Jan.";
                case 2: return "Feb.";
                case 3: return "Mar.";
                case 4: return "Arp.";
                case 5: return "May ";
                case 6: return "June";
                case 7: return "July";
                case 8: return "Aug.";
                case 9: return "Sep.";
                case 10: return "Oct.";
                case 11: return "Nov.";
                case 12: return "Dec.";
            }
            return "err.";
        }

        static double maxSymbols = 104.0;
        static void WritePercent(int value, int maxValue) {
            if (maxValue == 0)
                return;
            double percent = ((maxSymbols / maxValue)*value);

            for (int i = 0; i < percent; i++)
                Console.Write("o");
            Console.WriteLine();
        }
    }
}
