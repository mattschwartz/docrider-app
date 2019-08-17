using System.Text.RegularExpressions;

namespace DocriderParser.Tokens
{
    class SemicolonTerminatorTreeToken : ATokenizer
    {
        public SemicolonTerminatorTreeToken()
            : base(new Regex(";$"))
        {
        }

        protected override SyntaxTree TokenizeInternal(
            SyntaxTree tree,
            string _1,
            Match _2)
        {
            return tree;
        }
    }

    class DefaultAssignmentOperatorTreeToken : ATokenizer
    {
        public DefaultAssignmentOperatorTreeToken()
            : base(new Regex(@" *=> *([\w ]+)", RegexOptions.Multiline),
                  new SemicolonTerminatorTreeToken())
        {
        }

        protected override SyntaxTree TokenizeInternal(
            SyntaxTree tree,
            string line,
            Match foundMatch)
        {
            tree.AssignedValue = foundMatch.Groups[1].Value;
            return tree;
        }
    }

    class CharacterAssignmentOperatorTreeToken : ATokenizer
    {
        public CharacterAssignmentOperatorTreeToken()
            : base(new Regex(@""))
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            throw new System.NotImplementedException();
        }
    }
}
