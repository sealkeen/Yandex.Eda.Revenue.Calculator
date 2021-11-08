using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CountCourierRublesPerHour
{
    public class WeekInfo
    {
        public double salary;
        public double hours;
        public double fines;
        public int orderCount;
        public int startDay;
        public int startMonth;
        public int endDay;
        public int endMonth;
        public WeekInfo(//double salary, double hours
        )
        {
            salary = -1; hours = -1;
            fines = -1; orderCount = -1;
            startDay = -1; startMonth = -1;
            endDay = -1; endMonth = -1;
        }

        public double GetAverageRevenuePerOrder()
        {
            return Math.Round(this.salary / this.orderCount);
        }

        public double GetAverageRevenuePerHour()
        {
            return Math.Round(this.salary / this.hours);
        }
    }
}
