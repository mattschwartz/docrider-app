using System.Text.RegularExpressions;

namespace DocriderParser.Tokens
{
    class DialogueTreeToken : ITokenizer
    {
        private readonly Regex _matcher;

        public DialogueTreeToken()
        {
            _matcher = new Regex("^((?! *>>).*)$");
        }

        public Match CanTokenize(string line)
        {
            return _matcher.Match(line);
        }

        public SyntaxTree Tokenize(
            SyntaxTree tree,
            string line,
            Match foundMatch,
            int position)
        {
            return new SyntaxTree
            {
                Declarative = Declarative.Dialogue,
                AssignedValue = foundMatch.Value,
            };
        }
    }
}
