using System;

namespace ClassLib
{
    public class StringConcatenator
    {
        private string _value = "";
        private static int MAX_LENGTH = 10;

        public StringConcatenator Concat(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (_value.Length + value.Length > MAX_LENGTH)
                throw new OverflowException();
            ToLowerAndConcat(value);

            return this;
        }

        private void ToLowerAndConcat(string value)
        {
            _value += value.ToLower();
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
