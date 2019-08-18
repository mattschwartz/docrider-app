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
            var parser = new Parser();
            var compiler = new Compiler(parser);

            //string fileText = File.ReadAllText("Docrider.dr");

            while (true)
            {
                Console.WriteLine("Enter file text (empty line to stop):");
                string fileText = EnterFileText();
                CompilerLog log = compiler.Compile(fileText);

                PrettyPrintCompilerLog(log);
            }

            //Loopy(parser);
        }

        private static string EnterFileText()
        {
            string result = "";

            while (true)
            {
                string line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                result += line + "\n";
            }

            return result;
        }

        private static void Loopy(Parser parser)
        {
            Console.Clear();

            while (true)
            {
                Console.WriteLine("Enter a line to parse:");
                string line = Console.ReadLine();

                try
                {
                    SyntaxTree syntaxTree = parser.Tokenize(line);
                    PrettyPrintSyntaxTree(syntaxTree);
                }
                catch (SyntaxParseException ex)
                {
                    PrettyPrintSyntaxError(line, ex);
                }

                Console.WriteLine();
            }
        }

        private static void PrettyPrintCompilerLog(CompilerLog log)
        {
            foreach (var message in log.GetMessages())
            {
                switch (message.Level)
                {
                    case CompilerLevel.Debug:
                        Console.WriteLine($"debug  [line#{message.LineNumber}] {message.Message}");
                        break;

                    case CompilerLevel.Info:
                        Console.WriteLine($"info   [line#{message.LineNumber}] {message.Message}");
                        break;

                    case CompilerLevel.Warning:
                        Console.WriteLine($"warn   [line#{message.LineNumber}] {message.Message}");
                        break;

                    case CompilerLevel.Error:
                        PrettyPrintSyntaxError(
                            message.Line,
                            message.LineNumber,
                            message.ColumnNumber,
                            message.Message);
                        break;
                }
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


        private static void PrettyPrintSyntaxError(
            string line, SyntaxParseException ex)
        {
            PrettyPrintSyntaxError(line, 0, ex.Position, ex.Message);
        }

        private static void PrettyPrintSyntaxError(
            string line,
            int lineNumber,
            int columnNumber,
            string message)
        {
            if (columnNumber < 0 || string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            line += " ";

            var lastBgColor = Console.BackgroundColor;
            var lastFgColor = Console.ForegroundColor;

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine($"error [line#{lineNumber}] Failed to parse line: {message}");

            try
            {
                try
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;

                    string substr = line.Substring(0, columnNumber);
                    Console.Write(substr);

                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(line[columnNumber]);

                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Gray;

                    substr = line.Substring(columnNumber + 1);
                    Console.WriteLine(substr);

                    string spacing = new string(' ', columnNumber);
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
