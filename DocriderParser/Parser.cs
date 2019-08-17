using DocriderParser.Tokens;

namespace DocriderParser
{
    class Parser
    {
        public SyntaxTree Tokenize(string line)
        {
            ValidateTokenizable(line);

            string tokenizeableLine = line.Trim();
            var tree = BuildTree(new RootTreeToken(), tokenizeableLine);

            return tree;
        }

        // todo: Should probably plan on putting this logic before the tokenizer outside of this
        //  class or something
        private void ValidateTokenizable(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new SyntaxParseException("Line cannot be empty", -1);
            }
        }

        private SyntaxTree BuildTree(RootTreeToken rootTreeToken, string line)
        {
            var tree = new SyntaxTree();
            var match = rootTreeToken.CanTokenize(line);

            if (match.Success)
            {
                return rootTreeToken.Tokenize(tree, line, match, 0);
            }

            throw new SyntaxParseException($"Invalid token at {match.Index}", match.Index);
        }
    }
}
