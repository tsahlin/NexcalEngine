// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;

namespace Nexcal.Engine
{
	public class Expression : Token
	{
		public Token FirstToken { get; internal set; }

		public Token LastToken { get; internal set; }

		public void Add(Token token)
		{
			if (LastToken == null)
			{
				FirstToken	= token;
				LastToken	= token;
			}
			else
			{
				LastToken.RightToken	= token;
				LastToken				= token;
			}
		}

		public override Number Evaluate(Calculator calc)
		{
			throw new NotImplementedException();
		}
	}

	/*
	- List med tokens
	- SortedDictionary med prio => Lista med tokens att utvärdera (operator, sub-expression, unit)
	- Number innehåller value och unit
	- BinaryOperator (1+2), PreOperator (ex -5), PostOperator (ex fakultet 7!, enhet 5m)
	- Enheter ses som UnaryOperator. Deras operation är att tilldela unit till dess Number.
		Efter utförande ersätts number+unit tokens av endast number token
	- Token properties
		Position - rad och kolumn i det ursprungliga uttrycket, sätts av parsern
		LeftNumber - tal eller utvärderat uttryck till vänster
		RightNumber - tal eller utvärderat uttryck till höger
	*/
}
