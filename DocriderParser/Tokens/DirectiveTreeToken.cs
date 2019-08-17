using System.Text.RegularExpressions;

namespace DocriderParser.Tokens
{
    class DirectiveTreeToken : ATokenizer
    {
        public DirectiveTreeToken()
            : base(new Regex(@"^( *>> *(\w+) +(\w+) *=> *([\w ]+));$"),
                new DefineDeclarativeTreeToken(),
                new EnterDeclarativeTreeToken(),
                new ExitDeclarativeTreeToken(),
                new AliasDeclarativeTreeToken())
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            tree.Declarative = Declarative.Capture;
            return tree;
        }
    }
}
