// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Collections.Generic;
using Nexcal.Engine.Errors;

namespace Nexcal.Engine.Functions
{
	public abstract class FunctionCall : Token
	{
		public FunctionCall(Position position) : base(position)
		{
		}

		public override Precedence Precedence => Precedence.Primary;

		public override Number Evaluate(Calculator calc)
		{
			throw new NotImplementedException();
		}

		internal static void InitIdentifierMap(Dictionary<string, Type> map)
		{
			map["sin"]	= typeof(Sine);
		}

		public static FunctionCall Parse(Parser p)
		{
			throw new NotImplementedException();
			//Operator op;

			//switch (p.CurrentChar)
			//{
			//	case '+':
			//		op = new Add(p.Position);
			//		break;

			//	case '-':
			//		op = new Subtract(p.Position);
			//		break;

			//	default:
			//		throw new ParseException(p.Position, ParseExpectation.Operator);
			//}

			//p.Position.Advance();
			//op.Position.CalculateLength(p.Position);

			//return op;
		}
	}
}
