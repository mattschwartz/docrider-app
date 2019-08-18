using DocriderParser.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace DocriderParser.Compilation
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

        public void Debug(TokenizedLine tokenziedLine, string message) =>
            Debug(tokenziedLine.Line, tokenziedLine.LineNumber, 0, message);
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

        public void Warning(TokenizedLine tokenziedLine, string message) =>
            Warning(tokenziedLine.Line, tokenziedLine.LineNumber, 0, message);
        public void Warning(string line, int lineNumber, int column, string message)
        {
            _messages.Add(new CompilerMessage(
                CompilerLevel.Warning,
                line,
                lineNumber,
                column,
                message));
        }

        public void Error(TokenizedLine tokenizedLine, string message) =>
            Error(tokenizedLine.Line, tokenizedLine.LineNumber, 0, message);
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
}
