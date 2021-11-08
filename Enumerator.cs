using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CountCourierRublesPerHour
{
    public class Enumerator : IEnumerator
    {
        private WeekInfo[] weekArr;
        private int Cursor;
        public Enumerator(WeekInfo[] weekarr)
        {
            this.weekArr = weekarr;
            Cursor = -1;
        }
        object IEnumerator.Current {
            get {
                if ((Cursor < 0) || (Cursor == weekArr.Length))
                    throw new InvalidOperationException();
                return weekArr[Cursor];
            }
        }
        void IEnumerator.Reset()
        {
            Cursor = -1;
        }
        bool IEnumerator.MoveNext()
        {
            if (Cursor < weekArr.Length)
                Cursor++;

            return (!(Cursor == weekArr.Length));
        }
    }
}
