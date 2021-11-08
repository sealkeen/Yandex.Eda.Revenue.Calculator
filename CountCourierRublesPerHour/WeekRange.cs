using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CountCourierRublesPerHour
{
    public class WeekRange : ICollection
    {
        WeekInfo[] weekInfos;
        private int _iter = -1;
        public int Count { get { return weekInfos.Length; } }
        public WeekRange(int ct)
        {
            weekInfos = new WeekInfo[ct];
            _iter = -1;
        }
        public bool AddWeek(WeekInfo weekInfo)
        {
            if (weekInfo != null) {
                if (++_iter >= weekInfos.Length)
                    Array.Resize(ref weekInfos, weekInfos.Length + 1);
                if ((_iter) < weekInfos.Length) {
                    weekInfos[_iter] = weekInfo;
                    return true;
                }
            }
            return false;
        }

        //public int GetMax

        void ICollection.CopyTo(Array array, int index)
        {
            foreach (WeekInfo weekInfo in array) {
                weekInfos.SetValue(weekInfo, index);
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(weekInfos);
        // The IsSynchronized Boolean property returns True if the // collection is designed to be thread safe; otherwise, it returns False.
        bool ICollection.IsSynchronized { get { return false; } }
        // The SyncRoot property returns an object, which is used for synchronizing // the collection. This returns the instance of the object or returns the 
        // SyncRoot of other collections if the collection contains other collections. // 
        object ICollection.SyncRoot { get { return this; } }
        // The Count read-only property returns the number // of items in the collection.
        int ICollection.Count { get { return weekInfos.Length; } }
    }
}
