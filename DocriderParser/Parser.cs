using DocriderParser.Compilation;
using DocriderParser.Tokens;
using System.Collections.Generic;

namespace DocriderParser
{
    class Parser
    {
        public List<TokenizedLine> TokenizeFile(CompilerLog log, string filetext)
        {
            filetext = filetext.Replace('\r', '\n');
            string[] lines = filetext.Split('\n');

            var result = new List<TokenizedLine>();

            for (int lineNumber = 0; lineNumber < lines.Length; ++lineNumber)
            {
                string line = lines[lineNumber];

                if (string.IsNullOrWhiteSpace(line))
                {
                    log.Debug(line, lineNumber, 0, "Skipping empty line");
                }

                if (line.TrimStart().StartsWith("#"))
                {
                    log.Debug(line, lineNumber, 0, "Skipping comment");
                }

                try
                {
                    SyntaxTree tree = Tokenize(line);

                    result.Add(new TokenizedLine
                    {
                        Line = line,
                        LineNumber = lineNumber,
                        SyntaxTree = tree
                    });

                    log.Debug(line, lineNumber, 0, $"Success => {tree}");
                }
                catch (SyntaxParseException ex)
                {
                    log.Error(line, lineNumber, ex.Position, ex.Message);
                }
            }

            return result;
        }

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
