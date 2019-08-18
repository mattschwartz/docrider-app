using System;

namespace DocriderParser
{
    public class SyntaxParseException : Exception
    {
        public readonly int Position;

        public SyntaxParseException(string msg, int position = -1)
            : base(msg)
        {
            Position = position;
        }
    }
}
