using System;
using System.Collections.Generic;
using System.Text;

namespace DocriderParser.Tokens
{
    public enum Directive
    {
        Define,
        Enter,
        Exit,

        Alias,
    }

    public enum DeclaredType
    {
        Narrative,
        Character,
        Setting,
        Act,
        Scene,
    }

    public enum Declarative
    {
        Capture,
        Dialogue,
    }

    public class SyntaxParseException : Exception
    {
        public readonly int Position;

        public SyntaxParseException(string msg, int position)
            : base(msg)
        {
            Position = position;
        }
    }

    public struct SyntaxTree
    {
        public Declarative Declarative { get; set; }
        public Directive Directive { get; set; }
        public DeclaredType DeclaredType { get; set; }
        public object AssignedValue { get; set; }

        public override string ToString()
        {
            return $"[{nameof(Declarative)}={Declarative}, {nameof(Directive)}={Directive}, {nameof(DeclaredType)}={DeclaredType}, {nameof(AssignedValue)}={AssignedValue}]";
        }
    }
}
