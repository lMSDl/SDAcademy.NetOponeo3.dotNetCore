using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class Exercise2
    {
        private delegate IEnumerable<int> AsyncGenerate(int from, int count);
        AsyncGenerate GenerateCaller;

        public Exercise2()
        {
            GenerateCaller = Generate;
        }

        public IEnumerable<int> Generate(int from, int count)
        {
            throw new NotImplementedException();
        }
        public IAsyncResult BeginGenerate(int from, int count)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<int> EndGenerate(IAsyncResult asyncResult)
        {
            throw new NotImplementedException();
        }
    }
}
