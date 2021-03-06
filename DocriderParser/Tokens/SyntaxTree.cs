namespace DocriderParser.Tokens
{
    public enum Directive
    {
        None,
        Define,
        Enter,
        Exit,

        Alias,
    }

    public enum DeclaredType
    {
        None,
        Narrative,
        Character,
        Setting,
        Act,
        Scene,
    }

    public enum Declarative
    {
        None,
        Capture,
        Release,
        Dialogue,
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
