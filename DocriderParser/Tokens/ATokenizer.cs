using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DocriderParser.Tokens
{
    interface ITokenizer
    {
        Match CanTokenize(string line);

        SyntaxTree Tokenize(SyntaxTree tree, string line, Match foundMatch, int position);
    }

    abstract class ATokenizer : ITokenizer
    {
        protected readonly Regex _tokenMatcher;
        protected List<ITokenizer> _tokenizers;

        public ATokenizer(
            Regex tokenMatcher,
            params ITokenizer[] directiveTreeTokens)
        {
            _tokenMatcher = tokenMatcher;
            _tokenizers = directiveTreeTokens.ToList();
        }

        public virtual Match CanTokenize(string line) =>
            _tokenMatcher.Match(line);

        public virtual SyntaxTree Tokenize(
          SyntaxTree tree,
          string line,
          Match foundMatch,
          int position)
        {
            SyntaxTree? result = null;
            string substr = GetRemainingTokens(line, foundMatch);

            if (_tokenizers.Count == 0)
            {
                return TokenizeInternal(tree, substr, foundMatch);
            }

            foreach (var tokenizer in _tokenizers)
            {
                Match match = tokenizer.CanTokenize(substr);
                if (match.Success)
                {
                    result = TokenizeInternal(tree, substr, foundMatch);

                    result = tokenizer.Tokenize(
                        result.Value,
                        substr,
                        match,
                        position + match.Index);

                    break;
                }
            }

            if (result.HasValue)
            {
                return result.Value;
            }
            else
            {
                throw new SyntaxParseException(
                    $"[{GetType().Name}] Invalid token at col {position + foundMatch.Length}",
                    position + foundMatch.Length);
            }
        }

        protected virtual string GetRemainingTokens(string line, Match foundMatch)
        {
            return line.Substring(foundMatch.Index);
        }

        protected abstract SyntaxTree TokenizeInternal(
            SyntaxTree tree,
            string line,
            Match foundMatch);
    }
}
