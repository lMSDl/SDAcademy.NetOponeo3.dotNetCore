using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleApp
{
    public class Crud
    {
        private IList<int> items = new List<int>();
        private ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();

        public void Add(int element)
        {
            lockSlim.EnterWriteLock();
            items.Add(element);
            lockSlim.ExitWriteLock();
        }

        public void Delete(int index)
        {
            lockSlim.EnterWriteLock();
            try
            {
                items.RemoveAt(index);
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        public void Update(int index, int value)
        {
            lockSlim.EnterUpgradeableReadLock();
            try
            {
                items.RemoveAt(index);
                items.Insert(index, value);
            }
            finally
            {
                lockSlim.ExitUpgradeableReadLock();
            }
        }

        public int Read(int index)
        {
            lockSlim.EnterReadLock();
            try
            {
                return items[index];
            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }
    }
}
