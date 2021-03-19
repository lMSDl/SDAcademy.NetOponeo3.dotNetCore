using System;
using Xunit;
using ClassLib;

namespace ClassLib.Test
{
    public class StringConcatenatorUnitTest
    {
        public StringConcatenator CreateDefaultStringConcatenator()
        {
            return new StringConcatenator();
        }

        [Theory]
        [InlineData("a", "a")]
        [InlineData("B", "b")]
        [InlineData("0", "0")]
        public void Concat_SingleSign_ResturnSameSign(string input, string expected)
        {
            //Arrange
            var stringConcatenator = CreateDefaultStringConcatenator();

            //Act
            var result = stringConcatenator.Concat(input).ToString();

            //Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Concat_LongString_ThrowsOverflowException()
        {
            //Arrange
            var stringConcatenator = CreateDefaultStringConcatenator();
            const string OVER_MAXIMUM_LENGTH = "ala ma kota i psa";

            //Act
            Action action = () => stringConcatenator.Concat(OVER_MAXIMUM_LENGTH);

            //Assert
            Assert.Throws<OverflowException>(action);
        }

        [Fact]
        public void Concat_NullParameter_ThrowsArgumentNullException()
        {
            //Arrange
            var stringConcatenator = CreateDefaultStringConcatenator();

            //Act
            Action action = () => stringConcatenator.Concat(null);

            //Assert
            var argumentNullException = Assert.Throws<ArgumentNullException>(action);
            Assert.Equal("value", argumentNullException.ParamName);
        }
    }
}
