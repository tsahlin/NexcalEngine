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
				else if (HandleOptions(input))
					continue;

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

		static bool HandleOptions(string input)
		{
			switch (input)
			{
				case "anchors:on":
					ShowAnchors = true;
					Console.WriteLine("Anchor output enabled");
					break;

				case "anchors:off":
					ShowAnchors = false;
					Console.WriteLine("Anchor output disabled");
					break;

				case "debug:on":
					Calculator.DebugLogger = LogDebugMessage;
					Console.WriteLine("Debug logging enabled");
					break;

				case "debug:off":
					Calculator.DebugLogger = null;
					Console.WriteLine("Debug logging disabled");
					break;

				case "tree:on":
					ShowTree = true;
					Console.WriteLine("Tree output enabled");
					break;

				case "tree:off":
					ShowTree = false;
					Console.WriteLine("Tree output disabled");
					break;

				default:
					return false;
			}

			return true;
		}

		static void LogDebugMessage(string message)
		{
			Console.WriteLine(message);
		}
	}
}
