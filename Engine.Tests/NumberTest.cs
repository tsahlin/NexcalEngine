// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System.Globalization;
using Nexcal.Engine.Errors;
using Xunit;

namespace Nexcal.Engine.Tests
{
	public class NumberTest
	{
		[Fact]
		public void CultureNumberFormat()
		{
			Assert.Equal(",", CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator);
			Assert.Equal(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
		}

		[Theory]
		[InlineData("0b", ParseExpectation.BinNumber)]
		[InlineData("0b2", ParseExpectation.BinNumber)]
		[InlineData("0x", ParseExpectation.HexNumber)]
		[InlineData("0xg", ParseExpectation.HexNumber)]
		[InlineData(".a", ParseExpectation.Number)]
		public void ExParseExpectation(string str, ParseExpectation expectation)
		{
			try
			{
				var calc	= new Calculator();
				var number	= calc.Parse(str);
			}
			catch (ParseException e)
			{
				Assert.Equal(expectation, e.Expectation);
			}
		}

		[Theory]
		[InlineData("0b0", 0)]
		[InlineData("0b11", 3)]
		[InlineData("0b10000000000000000", 65536)]
		[InlineData("0b11111111111111111111111111111111111111111111111111111", 9007199254740991)]
		public void ParseBin(string bin, ulong value)
		{
			var calc	= new Calculator();
			var number	= (Number)calc.Parse(bin).FirstToken;

			Assert.Equal(value, (ulong)number.Value);
			Assert.Equal(NumberBase.Bin, number.Base);
		}

		[Theory]
		[InlineData("0", 0)]
		[InlineData("12345", 12345)]
		[InlineData("6789.0123", 6789.0123)]
		[InlineData(".5678", 0.5678)]
		[InlineData("9007199254740991", 9007199254740991)]
		public void ParseDec(string dec, double value)
		{
			var calc	= new Calculator();
			var number	= (Number)calc.Parse(dec).FirstToken;

			Assert.Equal(value, number.Value);
			Assert.Equal(NumberBase.Dec, number.Base);
		}

		[Theory]
		[InlineData("0x0", 0)]
		[InlineData("0xff", 255)]
		[InlineData("0xFFFF", 65535)]
		[InlineData("0x1FFFFFFFFFFFFF", 9007199254740991)]
		public void ParseHex(string hex, ulong value)
		{
			var calc	= new Calculator();
			var number	= (Number)calc.Parse(hex).FirstToken;

			Assert.Equal(value, (ulong)number.Value);
			Assert.Equal(NumberBase.Hex, number.Base);
		}
	}
}
