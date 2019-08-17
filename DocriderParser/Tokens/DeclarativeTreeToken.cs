using System;
using System.Text.RegularExpressions;

namespace DocriderParser.Tokens
{
    class DefineDeclarativeTreeToken : ATokenizer
    {
        public DefineDeclarativeTreeToken()
            : base(new Regex(@"(define) +", RegexOptions.IgnoreCase),
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

    class EnterDeclarativeTreeToken : ATokenizer
    {
        public EnterDeclarativeTreeToken()
            : base(new Regex(@"(enter) +", RegexOptions.IgnoreCase),
                new CharacterDeclaredTypeTreeToken(),
                new SettingDeclaredTypeTreeToken(),
                new ActDeclaredTypeTreeToken(),
                new SceneDeclaredTypeTreeToken())
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            tree.Directive = Directive.Enter;
            return tree;
        }
    }

    class ExitDeclarativeTreeToken : ATokenizer
    {
        public ExitDeclarativeTreeToken()
            : base(new Regex(@"(exit) +", RegexOptions.IgnoreCase),
                new CharacterDeclaredTypeTreeToken(),
                new SettingDeclaredTypeTreeToken(),
                new ActDeclaredTypeTreeToken(),
                new SceneDeclaredTypeTreeToken())
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            tree.Directive = Directive.Exit;
            return tree;
        }
    }

    class AliasDeclarativeTreeToken : ATokenizer
    {
        public AliasDeclarativeTreeToken()
            : base(new Regex(@"(alias) +", RegexOptions.IgnoreCase))
        {
        }

        protected override SyntaxTree TokenizeInternal(SyntaxTree tree, string _1, Match _2)
        {
            tree.Directive = Directive.Alias;
            return tree;
            throw new NotImplementedException();
        }
    }
}
