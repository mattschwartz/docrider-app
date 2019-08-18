using DocriderParser.Compilation;
using DocriderParser.Tokens;
using System;
using System.Collections.Generic;
using System.IO;

namespace DocriderParser
{
    class Program
    {
        delegate void ShellCommand();

        static void Main(string[] args)
        {
            var parser = new Parser();
            var compiler = new Compiler(parser);

            var shellCommands = new Dictionary<string, ShellCommand>
            {
                ["Compile test file (simple)"] = () => CompileFile("Docrider.dr"),
                ["Compile test file (complex)"] = () => CompileFile("Docrider_complex.dr"),
                ["Interactive parser"] = () => Loopy(parser),
                ["Interactive compiler"] = () =>
                {
                    Console.WriteLine("Enter file text (empty line to stop):");
                    string fileText = EnterFileText();
                    CompilerLog log = compiler.Compile(fileText);

                    PrettyPrintCompilerLog(log);
                }
            };

            while (true)
            {
                PrintOptions(shellCommands);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int choice))
                {
                    int i = 0;
                    foreach (var key in shellCommands.Keys)
                    {
                        if (i++ == choice)
                        {
                            shellCommands[key]();
                        }
                    }
                }
                Console.WriteLine("\n");
            }
        }

        private static void PrintOptions(Dictionary<string, ShellCommand> commands)
        {
            int choiceNumber = 0;
            foreach (var key in commands.Keys)
            {
                Console.Write($"[{choiceNumber++}] {key}\n");
            }
            Console.Write("\nEnter your decision> ");
        }

        private static void CompileFile(string filename)
        {
            string fileText = File.ReadAllText(filename);
            var parser = new Parser();
            var compiler = new Compiler(parser);
            CompilerLog log = compiler.Compile(fileText);

            PrettyPrintCompilerLog(log);

            Console.WriteLine($"Parser: {log.ParseTime}ms");
            Console.WriteLine($"Compiler: {log.CompileTime}ms");
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
            var lastBgColor = Console.BackgroundColor;
            var lastFgColor = Console.ForegroundColor;

            foreach (var message in log.GetMessages())
            {
                switch (message.Level)
                {
                    case CompilerLevel.Debug:
                        Console.ForegroundColor = ConsoleColor.DarkGray;

                        Console.WriteLine($"DBG [L#{message.LineNumber,3}] {message.Message}");
                        break;

                    case CompilerLevel.Info:
                        Console.ForegroundColor = ConsoleColor.Gray;

                        Console.WriteLine($"INF [L#{message.LineNumber,3}] {message.Message}");
                        break;

                    case CompilerLevel.Warning:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;

                        Console.WriteLine($"WRN [L#{message.LineNumber,3}] {message.Message}");
                        break;

                    case CompilerLevel.Error:
                        PrettyPrintSyntaxError(
                            message.Line,
                            message.LineNumber,
                            message.ColumnNumber,
                            message.Message);
                        break;
                }

                Console.BackgroundColor = lastBgColor;
                Console.ForegroundColor = lastFgColor;
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

            Console.ForegroundColor = ConsoleColor.DarkRed;

            Console.WriteLine($"ERR [L#{lineNumber,3}] Failed to parse line: {message}");

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
