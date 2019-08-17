using System.Text.RegularExpressions;

namespace DocriderParser.Tokens
{
    class RootTreeToken : ATokenizer
    {
        public RootTreeToken()
            : base(new Regex(@"^(.*)$"),
                new DirectiveTreeToken(),
                new DialogueTreeToken())
        {
        }

        protected override string GetRemainingTokens(string line, Match foundMatch)
        {
            return base.GetRemainingTokens(line, foundMatch);
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            return tree;
        }
    }
}
