using DocriderParser.Models;
using DocriderParser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocriderParser
{
    enum CompilerLevel
    {
        Debug,
        Info,
        Warning,
        Error,
    }

    struct CompilerMessage
    {
        public CompilerLevel Level { get; private set; }
        public string Line { get; private set; }
        public int LineNumber { get; private set; }
        public int ColumnNumber { get; private set; }
        public string Message { get; private set; }

        public CompilerMessage(
            CompilerLevel level,
            string line,
            int lineNumber,
            int columnNumber,
            string message)
        {
            Level = level;
            Line = line;
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
            Message = message;
        }
    }

    class CompilerLog
    {
        private readonly List<CompilerMessage> _messages = new List<CompilerMessage>();

        public List<CompilerMessage> GetMessages() =>
            _messages.ToList();

        public void Debug(string line, int lineNumber, int column, string message)
        {
            _messages.Add(new CompilerMessage(
                CompilerLevel.Debug,
                line,
                lineNumber,
                column,
                message));
        }

        public void Info(string line, int lineNumber, int column, string message)
        {
            _messages.Add(new CompilerMessage(
                CompilerLevel.Info,
                line,
                lineNumber,
                column,
                message));
        }

        public void Warning(string line, int lineNumber, int column, string message)
        {
            _messages.Add(new CompilerMessage(
                CompilerLevel.Warning,
                line,
                lineNumber,
                column,
                message));
        }

        public void Error(string line, int lineNumber, int column, string message)
        {
            _messages.Add(new CompilerMessage(
                CompilerLevel.Error,
                line,
                lineNumber,
                column,
                message));
        }
    }

    class Compiler
    {
        private readonly Parser _parser;

        public Compiler(Parser parser)
        {
            _parser = parser;
        }

        public CompilerLog Compile(string text)
        {
            var log = new CompilerLog();
            List<SyntaxTree> syntaxTrees = ParseFile(log, text);

            var objectModel = new ObjectModel();

            foreach (var tree in syntaxTrees)
            {
                if (tree.Directive == Directive.Define)
                {
                    CompileDefinition(objectModel, tree);
                }
            }

            return log;
        }

        private void CompileDefinition(ObjectModel objectModel, SyntaxTree tree)
        {
            switch (tree.DeclaredType)
            {
                case DeclaredType.Narrative:
                    break;
                case DeclaredType.Character:
                    break;
                case DeclaredType.Setting:
                    break;

                default:
                    throw new InternalCompilerException($"Cannot define declared type {tree.DeclaredType}");
            }
        }

        private List<SyntaxTree> ParseFile(CompilerLog log, string text)
        {
            text = text.Replace('\r', '\n');
            string[] lines = text.Split('\n');

            var result = new List<SyntaxTree>();

            for (int lineNumber = 0; lineNumber < lines.Length; ++lineNumber)
            {
                string line = lines[lineNumber];

                if (string.IsNullOrWhiteSpace(line))
                {
                    log.Debug(line, lineNumber, 0, "Skipping empty line");
                }

                if (line.TrimStart().StartsWith("#"))
                {
                    log.Debug(line, lineNumber, 0, "Skipping comment");
                }

                try
                {
                    SyntaxTree tree = _parser.Tokenize(line);
                    result.Add(tree);
                    log.Debug(line, lineNumber, 0, $"Success => {tree}");
                }
                catch (SyntaxParseException ex)
                {
                    log.Error(line, lineNumber, ex.Position, ex.Message);
                }
            }

            return result;
        }

        private Narrative DefineNarrative(string line)
        {
            try
            {
                SyntaxTree tree = _parser.Tokenize(line);
                return new Narrative(tree.AssignedValue.ToString());
            }
            catch (SyntaxParseException)
            {
                return null;
            }
        }

        private void CompileTree(SyntaxTree tree)
        {
            // ok we're creating something, great
            if (tree.Directive == Directive.Define)
            {
                // what are we creating?
                switch (tree.DeclaredType)
                {
                    case DeclaredType.Narrative:
                        throw new SyntaxParseException("Duplicate narrative definition. Expected only 1 narrative definition per file.");

                    case DeclaredType.Character:
                        return new Character(tree.AssignedValue.ToString());

                        case DeclaredType.
                }
            }

            switch (tree.Directive)
            {

            }
        }
    }
}
