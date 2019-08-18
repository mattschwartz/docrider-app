using System;

namespace DocriderParser
{
    class InternalCompilerException : Exception
    {
        public InternalCompilerException(string message)
            : base(message)
        {
        }
    }
}
