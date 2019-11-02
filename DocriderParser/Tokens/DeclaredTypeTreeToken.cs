using System.Text.RegularExpressions;

namespace DocriderParser.Tokens
{
    class NarrativeDeclaredTypeTreeToken : ATokenizer
    {
        public NarrativeDeclaredTypeTreeToken()
            : base(new Regex(@"^narrative *", RegexOptions.IgnoreCase),
                new DefaultAssignmentOperatorTreeToken())
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            tree.DeclaredType = DeclaredType.Narrative;
            return tree;
        }
    }

    class CharacterDeclaredTypeTreeToken : ATokenizer
    {
        public CharacterDeclaredTypeTreeToken()
            : base(new Regex(@"^character *", RegexOptions.IgnoreCase),
                new DefaultAssignmentOperatorTreeToken())
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            tree.DeclaredType = DeclaredType.Character;
            return tree;
        }
    }

    class SettingDeclaredTypeTreeToken : ATokenizer
    {
        public SettingDeclaredTypeTreeToken()
            : base(new Regex(@"^setting *", RegexOptions.IgnoreCase),
                new DefaultAssignmentOperatorTreeToken())
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            tree.DeclaredType = DeclaredType.Setting;
            return tree;
        }
    }

    class ActDeclaredTypeTreeToken : ATokenizer
    {
        public ActDeclaredTypeTreeToken()
            : base(new Regex(@"^act *", RegexOptions.IgnoreCase),
                new DefaultAssignmentOperatorTreeToken())
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            tree.DeclaredType = DeclaredType.Act;
            return tree;
        }
    }

    class SceneDeclaredTypeTreeToken : ATokenizer
    {
        public SceneDeclaredTypeTreeToken()
            : base(new Regex(@"^scene *", RegexOptions.IgnoreCase),
                new DefaultAssignmentOperatorTreeToken())
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            tree.DeclaredType = DeclaredType.Scene;
            return tree;
        }
    }

    class SceneWithCharactersDeclaredTypeTreeToken : ATokenizer
    {
        public SceneWithCharactersDeclaredTypeTreeToken()
            : base(new Regex(@"^scene +with +\[([\w ,]+)\] *", RegexOptions.IgnoreCase),
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
            tree.AssignedValue = "Huh?";
            tree.DeclaredType = DeclaredType.Scene;
            return tree;
        }
    }
}
