using Xunit;

namespace Nexcal.Engine.Tests
{
	public class NumberTest
	{
		[Theory]
		[InlineData("0b0", 0)]
		[InlineData("0b11", 3)]
		[InlineData("0b10000000000000000", 65536)]
		[InlineData("0b100000000000000000000000000000000000000000000000000000", 9007199254740992)]
		public void ParseBin(string bin, ulong value)
		{
			var parser = new Parser(bin);
			var number = Number.Parse(parser, new Position());

			Assert.Equal(value, (ulong)number.Value);
			Assert.Equal(NumberBase.Bin, number.Base);
		}

		[Theory]
		[InlineData("0x0", 0)]
		[InlineData("0xff", 255)]
		[InlineData("0xFFFF", 65535)]
		[InlineData("0x20000000000000", 9007199254740992)]
		public void ParseHex(string hex, ulong value)
		{
			var parser = new Parser(hex);
			var number = Number.Parse(parser, new Position());

			Assert.Equal(value, (ulong)number.Value);
			Assert.Equal(NumberBase.Hex, number.Base);
		}
	}
}
