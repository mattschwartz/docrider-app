using System.Text.RegularExpressions;

namespace DocriderParser.Tokens
{
    class DeclarativeTreeToken : ATokenizer
    {
        public DeclarativeTreeToken()
            : base(new Regex(@"^ *>> *(\w+ +\w+ *=> *[\w ]+;)$"),
                new DefineDirectiveTreeToken(),
                new EnterDirectiveTreeToken(),
                new AliasDirectiveTreeToken())
        {
        }

        protected override string GetRemainingTokens(string line, Match foundMatch)
        {
            return line.Substring(foundMatch.Groups[1].Index);
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            tree.Declarative = Declarative.Capture;
            return tree;
        }
    }
}
