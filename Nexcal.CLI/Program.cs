using System;
using Nexcal.Engine;
using Nexcal.Engine.Errors;

namespace Nexcal.CLI
{
	class Program
	{
		static private Calculator Calculator { get; } = new Calculator();

		static private bool ShowAnchors { get; set; } = false;
		static private bool ShowTree { get; set; } = false;
		static private bool Exit { get; set; } = false;

		static void Main(string[] args)
		{
			while (!Exit)
			{
				Console.Write("> ");

				var input = Console.ReadLine();

				if (input == "exit" || input == "quit")
					break;
				else if (input == "anchors:on")
				{
					ShowAnchors = true;
					Console.WriteLine("Anchor output enabled");
				}
				else if (input == "anchors:off")
				{
					ShowAnchors = false;
					Console.WriteLine("Anchor output disabled");
				}
				else if (input == "tree:on")
				{
					ShowTree = true;
					Console.WriteLine("Tree output enabled");
				}
				else if (input == "tree:off")
				{
					ShowTree = false;
					Console.WriteLine("Tree output disabled");
				}
				else
					Calc(input);
			}
		}

		static void Calc(string input)
		{
			try
			{
				var expr = Calculator.Parse(input);

				if (ShowAnchors)
					Console.WriteLine(expr.AnchorList);

				if (ShowTree)
					Console.WriteLine(expr.TreeList);

				Console.WriteLine(Calculator.Calculate(expr));

				foreach (var warning in Calculator.Warnings)
					Console.WriteLine($"{warning.Position}: {warning.Code}");

				Calculator.Warnings.Clear();
			}
			catch (NexcalException e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
