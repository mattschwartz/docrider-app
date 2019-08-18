using System.Text.RegularExpressions;

namespace DocriderParser.Tokens
{
    /// <summary>
    /// Revocatives are declaratives that remove things from the stack.
    /// i.e., "<< exit...;"
    /// </summary>
    class RevocativeTreeToken : ATokenizer
    {
        public RevocativeTreeToken()
            : base(new Regex(@"^ *<< *"),
                new ExitDirectiveTreeToken())
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string line, Match foundMatch)
        {
            tree.Declarative = Declarative.Release;
            return tree;
        }
    }

    class ExitDirectiveTreeToken : ATokenizer
    {
        public ExitDirectiveTreeToken()
            : base(new Regex(@"^exit +", RegexOptions.IgnoreCase),
                new ReleaseCharacterTokenizer(),
                new ReleaseSettingTokenizer(),
                new ReleaseSceneTokenizer(),
                new ReleaseActTokenizer())
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            tree.Directive = Directive.Exit;
            return tree;
        }
    }

    class ReleaseCharacterTokenizer : ATokenizer
    {
        public ReleaseCharacterTokenizer()
            : base(new Regex(@"^character *", RegexOptions.IgnoreCase),
                new DefaultAssignmentOperatorTreeToken())
        {
        }

        public override Match CanTokenize(string line)
        {
            var match = _tokenMatcher.Match(line);
            return match;
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string line, Match foundMatch)
        {
            var characterToRelease = foundMatch.Groups[1].Value;

            tree.AssignedValue = characterToRelease;
            tree.DeclaredType = DeclaredType.Character;

            return tree;
        }
    }

    class ReleaseSettingTokenizer : ATokenizer
    {
        public ReleaseSettingTokenizer()
            : base(new Regex(@"^setting *;$", RegexOptions.IgnoreCase))
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string line, Match foundMatch)
        {
            tree.DeclaredType = DeclaredType.Setting;
            return tree;
        }
    }

    class ReleaseActTokenizer : ATokenizer
    {
        public ReleaseActTokenizer()
            : base(new Regex(@"^act *;$", RegexOptions.IgnoreCase))
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string line, Match foundMatch)
        {
            tree.DeclaredType = DeclaredType.Act;
            return tree;
        }
    }

    class ReleaseSceneTokenizer : ATokenizer
    {
        public ReleaseSceneTokenizer()
            : base(new Regex(@"^scene *;", RegexOptions.IgnoreCase))
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string line, Match foundMatch)
        {
            tree.DeclaredType = DeclaredType.Scene;
            return tree;
        }
    }
}
