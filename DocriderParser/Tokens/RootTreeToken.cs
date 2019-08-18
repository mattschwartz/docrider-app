using System.Text.RegularExpressions;

namespace DocriderParser.Tokens
{
    class RootTreeToken : ATokenizer
    {
        public RootTreeToken()
            : base(new Regex(""),
                new DeclarativeTreeToken(),
                new RevocativeTreeToken(),
                new DialogueTreeToken())
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            return tree;
        }
    }
}
