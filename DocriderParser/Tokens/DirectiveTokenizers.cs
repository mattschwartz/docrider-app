using System;
using System.Text.RegularExpressions;

namespace DocriderParser.Tokens
{
    class DefineDirectiveTreeToken : ATokenizer
    {
        public DefineDirectiveTreeToken()
            : base(new Regex(@"^define +", RegexOptions.IgnoreCase),
                new NarrativeDeclaredTypeTreeToken(),
                new CharacterDeclaredTypeTreeToken(),
                new SettingDeclaredTypeTreeToken())
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            tree.Directive = Directive.Define;
            return tree;
        }
    }

    class EnterDirectiveTreeToken : ATokenizer
    {
        public EnterDirectiveTreeToken()
            : base(new Regex(@"^enter +", RegexOptions.IgnoreCase),
                new CharacterDeclaredTypeTreeToken(),
                new SettingDeclaredTypeTreeToken(),
                new ActDeclaredTypeTreeToken(),
                new SceneWithCharactersDeclaredTypeTreeToken(),
                new SceneDeclaredTypeTreeToken())
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            tree.Directive = Directive.Enter;
            return tree;
        }
    }

    class AliasDirectiveTreeToken : ATokenizer
    {
        public AliasDirectiveTreeToken()
            : base(new Regex(@"^alias +", RegexOptions.IgnoreCase))
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            tree.Directive = Directive.Alias;
            return tree;
        }
    }
}
