using DocriderParser.Tokens;
using System;
using System.IO;
using System.Linq;

namespace DocriderParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] fileText = File.ReadAllLines("Docrider.dr");
            var parser = new Parser();

            //Loopy(parser);

            foreach (var line in fileText)
            {
                ParseLineAndPrint(parser, line);
            }
        }

        private static void Loopy(Parser parser)
        {
            Console.Clear();

            while (true)
            {
                Console.WriteLine("Enter a line to parse:");
                string line = Console.ReadLine();

                ParseLineAndPrint(parser, line);

                Console.WriteLine();
            }
        }

        private static void ParseLineAndPrint(Parser parser, string line)
        {
            try
            {
                SyntaxTree syntaxTree = parser.Tokenize(line);
                PrettyPrintSyntaxTree(syntaxTree);
            }
            catch (SyntaxParseException ex)
            {
                PrettyPrintSyntaxError(line, ex);
            }
        }

        private static void PrettyPrintSyntaxTree(SyntaxTree syntaxTree)
        {
            if (syntaxTree.Declarative == Declarative.Dialogue)
            {
                Console.Write($"\"{syntaxTree.AssignedValue}\"");
                return;
            }

            Console.Write("\t~");
            switch (syntaxTree.Directive)
            {
                case Directive.Define:
                    Console.Write("Defined ");
                    break;

                case Directive.Enter:
                    Console.Write("Entered ");
                    break;

                case Directive.Exit:
                    Console.Write("Exited ");
                    break;

                case Directive.Alias:
                    Console.Write("Aliased ");
                    break;
            }

            switch (syntaxTree.DeclaredType)
            {
                case DeclaredType.Act:
                    Console.WriteLine($"act with name \"{syntaxTree.AssignedValue}\"");
                    break;

                case DeclaredType.Character:
                    Console.WriteLine($"act with name \"{syntaxTree.AssignedValue}\"");
                    break;

                case DeclaredType.Narrative:
                    Console.WriteLine($"narrative with name \"{syntaxTree.AssignedValue}\"");
                    break;

                case DeclaredType.Scene:
                    Console.WriteLine($"scene with name \"{syntaxTree.AssignedValue}\"");
                    break;

                case DeclaredType.Setting:
                    Console.WriteLine($"setting with name \"{syntaxTree.AssignedValue}\"");
                    break;
            }
        }

        private static void PrettyPrintSyntaxError(string line, SyntaxParseException ex)
        {
            if (ex.Position < 0 || string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            line += " ";

            var lastBgColor = Console.BackgroundColor;
            var lastFgColor = Console.ForegroundColor;

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Failed to parse line: {ex.Message}");

            try
            {
                try
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;

                    string substr = line.Substring(0, ex.Position);
                    Console.Write(substr);

                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(line[ex.Position]);

                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Gray;

                    substr = line.Substring(ex.Position + 1);
                    Console.WriteLine(substr);

                    string spacing = new string(' ', ex.Position);
                    Console.WriteLine($"{spacing}^");
                }
                finally
                {
                    Console.BackgroundColor = lastBgColor;
                    Console.ForegroundColor = lastFgColor;
                }
            }
            catch (Exception ex2)
            {
                Console.WriteLine($"Failed to pretty print: {ex2.Message}\n\tStack trace: {ex2.StackTrace}");
            }

            Console.WriteLine();
        }
    }
}
