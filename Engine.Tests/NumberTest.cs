// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System.Globalization;
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
		[InlineData("0b0", 0)]
		[InlineData("0b11", 3)]
		[InlineData("0b10000000000000000", 65536)]
		[InlineData("0b11111111111111111111111111111111111111111111111111111", 9007199254740991)]
		public void ParseBin(string bin, ulong value)
		{
			var parser = new Parser(bin);
			var number = Number.Parse(parser);

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
			var parser = new Parser(dec);
			var number = Number.Parse(parser);

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
			var parser = new Parser(hex);
			var number = Number.Parse(parser);

			Assert.Equal(value, (ulong)number.Value);
			Assert.Equal(NumberBase.Hex, number.Base);
		}
	}
}
