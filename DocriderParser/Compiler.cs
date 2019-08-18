using DocriderParser.Models;
using DocriderParser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocriderParser
{
    enum CompilerLevel
    {
        Debug,
        Info,
        Warning,
        Error,
    }

    struct CompilerMessage
    {
        public CompilerLevel Level { get; private set; }
        public string Line { get; private set; }
        public int LineNumber { get; private set; }
        public int ColumnNumber { get; private set; }
        public string Message { get; private set; }

        public CompilerMessage(
            CompilerLevel level,
            string line,
            int lineNumber,
            int columnNumber,
            string message)
        {
            Level = level;
            Line = line;
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
            Message = message;
        }
    }

    class CompilerLog
    {
        private readonly List<CompilerMessage> _messages = new List<CompilerMessage>();

        public List<CompilerMessage> GetMessages() =>
            _messages.ToList();

        public void Debug(string line, int lineNumber, int column, string message)
        {
            _messages.Add(new CompilerMessage(
                CompilerLevel.Debug,
                line,
                lineNumber,
                column,
                message));
        }

        public void Info(string line, int lineNumber, int column, string message)
        {
            _messages.Add(new CompilerMessage(
                CompilerLevel.Info,
                line,
                lineNumber,
                column,
                message));
        }

        public void Warning(TokenizedLine tokenziedLine, string message) =>
            Warning(tokenziedLine.Line, tokenziedLine.LineNumber, 0, message);
        public void Warning(string line, int lineNumber, int column, string message)
        {
            _messages.Add(new CompilerMessage(
                CompilerLevel.Warning,
                line,
                lineNumber,
                column,
                message));
        }

        public void Error(TokenizedLine tokenizedLine, string message) =>
            Erorr(tokenizedLine.Line, tokenizedLine.LineNumber, 0, message);
        public void Error(string line, int lineNumber, int column, string message)
        {
            _messages.Add(new CompilerMessage(
                CompilerLevel.Error,
                line,
                lineNumber,
                column,
                message));
        }
    }

    class Compiler
    {
        private readonly Parser _parser;

        public Compiler(Parser parser)
        {
            _parser = parser;
        }

        public CompilerLog Compile(string text)
        {
            var log = new CompilerLog();
            List<TokenizedLine> tokenizedLines = _parser.TokenizeFile(log, text);

            var objectDefinitions = new StackObject();
            var stackFrames = new List<StackFrame>();

            // build object model definitions
            var lineDefinitions = tokenizedLines.Where(t => t.SyntaxTree.Directive == Directive.Define);
            foreach (var tokenizedLine in lineDefinitions)
            {
                if (tokenizedLine.SyntaxTree.Directive == Directive.Define)
                {
                    CompileDefinition(objectDefinitions, tokenizedLine);
                }
            }

            // lifecycle declarations, which describe entrances and exits

            var lineDeclarations = tokenizedLines.Except(lineDefinitions);

            var currentFrame = new StackFrame();
            foreach (var tokenizedLine in lineDeclarations)
            {
                var directive = tokenizedLine.SyntaxTree.Directive;
                if (directive == Directive.Enter)
                {
                    CompileEntrance(objectDefinitions, log, currentFrame, tokenizedLine);
                }
                else if (directive == Directive.Exit)
                {
                    CompileExit(log, currentFrame, tokenizedLine);
                }
            }

            return log;
        }

        /// <summary>
        /// Definitions can be declared anywhere in the doc and aren't related to each other
        /// except that duplicate definitions are invalid.
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="tokenizedLine"></param>
        private void CompileDefinition(StackObject stack, TokenizedLine tokenizedLine)
        {
            SyntaxTree tree = tokenizedLine.SyntaxTree;

            switch (tree.DeclaredType)
            {
                case DeclaredType.Narrative:
                    if (stack.Narrative != null)
                    {
                        throw new InternalCompilerException(
                            $"Only one narrative can be defined per file");
                    }
                    break;

                case DeclaredType.Character:
                    var character = new Character(tree.AssignedValue.ToString());

                    if (stack.Characters.Any(t => t == character))
                    {
                        throw new InternalCompilerException($"Duplicate Character definition for {character.Name}");
                    }
                    else
                    {
                        stack.Characters.Add(character);
                    }

                    break;

                case DeclaredType.Setting:
                    var setting = new Setting(tree.AssignedValue.ToString());
                    if (stack.Settings.Any(t => t == setting))
                    {
                        throw new InternalCompilerException(
                            $"Duplicate Setting definition for {setting.Name}");
                    }
                    else
                    {
                        stack.Settings.Add(setting);
                    }

                    break;

                default:
                    throw new InternalCompilerException(
                        $"Cannot define declared type {tree.DeclaredType}");
            }
        }

        private void CompileEntrance(
            StackObject objectDefinitions,
            CompilerLog log,
            StackFrame currentFrame,
            TokenizedLine tokenizedLine)
        {
            SyntaxTree tree = tokenizedLine.SyntaxTree;

            switch (tree.DeclaredType)
            {
                case DeclaredType.Act:
                    var actName = tree.AssignedValue.ToString();
                    if (currentFrame.Act != null)
                    {
                        log.Error(tokenizedLine, $"Cannot enter act with name \"{actName}\" when already in act \"{currentFrame.Act.Name}\".");
                    }

                    var actToEnter = new Act(actName);
                    if (objectDefinitions.Acts.Contains(actToEnter))
                    {
                        log.Error(tokenizedLine, $"Cannot re-enter act with name \"{actName}\".");
                    }
                    else
                    {
                        objectDefinitions.Acts.Add(actToEnter);
                        currentFrame.Act = actToEnter;
                    }

                    break;

                case DeclaredType.Scene:
                    var sceneName = tree.AssignedValue.ToString();
                    if (currentFrame.Scene != null)
                    {
                        log.Error(tokenizedLine, $"Cannot enter scene with name \"{sceneName}\" when already in scene \"{currentFrame.Scene.Name}\".");
                    }

                    var sceneToEnter = new Scene(sceneName);
                    if (objectDefinitions.Scenes.Contains(sceneToEnter))
                    {
                        log.Error(tokenizedLine, $"Cannot re-enter scene with name \"{sceneName}\".");
                    }
                    else
                    {
                        objectDefinitions.Scenes.Add(sceneToEnter);
                        currentFrame.Scene = sceneToEnter;
                    }

                    break;

                case DeclaredType.Character:
                    var characterName = tree.AssignedValue.ToString();
                    var character = objectDefinitions.Characters.SingleOrDefault(t => t.Name == characterName);

                    if (character == null)
                    {
                        log.Error(tokenizedLine, $"Cannot enter undefined character \"{characterName}\".");
                    }

                    var currentScene = currentFrame.Scene;
                    if (currentScene == null)
                    {
                        log.Error(tokenizedLine, $"Cannot enter character \"{characterName}\" outside of a scene. Please enter a scene first.");
                    }
                    else
                    {
                        currentScene.Characters.Add(character);
                    }

                    break;

                case DeclaredType.Setting:
                    var settingName = tree.AssignedValue.ToString();
                    var setting = objectDefinitions.Settings.SingleOrDefault(t => t.Name == settingName);

                    if (setting == null)
                    {
                        log.Error(tokenizedLine, $"Cannot enter undefined setting \"{settingName}\".");
                    }

                default:
                    throw new InternalCompilerException($"Cannot enter declared type {tree.DeclaredType}");
            }
        }

        private void CompileExit(
            CompilerLog log,
            StackFrame currentFrame,
            TokenizedLine tokenizedLine)
        {
            SyntaxTree tree = tokenizedLine.SyntaxTree;

            switch (tree.DeclaredType)
            {
                case DeclaredType.Act:
                    var actName = tree.AssignedValue.ToString();
                    if (currentFrame.Act == null)
                    {
                        log.Error(tokenizedLine, $"Attempted to exit act \"{actName}\" which was never entered.");
                    }
                    else if (currentFrame.Act.Name != actName)
                    {
                        log.Error(tokenizedLine, $"Attempted to exit act \"{actName}\", but expected to exit current act with name \"{currentFrame.Act.Name}\".");
                    }
                    else
                    {
                        currentFrame.Act = null;
                    }

                    break;

                case DeclaredType.Scene:
                    var sceneName = tree.AssignedValue.ToString();
                    if (currentFrame.Scene == null)
                    {
                        log.Error(tokenizedLine, $"Attempted to exit scene \"{sceneName}\" which was never entered.");
                    }
                    else if (currentFrame.Scene.Name != sceneName)
                    {
                        log.Error(tokenizedLine, $"Attempted to exit act \"{sceneName}\", but expected to exit current act with name \"{currentFrame.Scene.Name}\".");
                    }
                    else if (currentFrame.Characters.Count != 0)
                    {
                        log.Warning(tokenizedLine, "Exiting scene with characters present will exeunt all characters. Did you mean to do this?");

                        currentFrame.Characters.Clear();
                        currentFrame.Scene = null;
                    }
                    else
                    {
                        currentFrame.Scene = null;
                    }
                    break;

                case DeclaredType.Character:
                    var characterName = tree.AssignedValue.ToString();
                    var characterToExit = currentFrame.Characters.SingleOrDefault(t => t.Name == characterName);

                    if (characterToExit == null)
                    {
                        log.Warning(tokenizedLine, $"Attempted to exit character \"{characterName}\" which was not present in scene.");
                    }
                    else
                    {
                        currentFrame.Characters.Remove(characterToExit);
                    }

                    break;

                case DeclaredType.Setting:
                    var settingName = tree.AssignedValue.ToString();

                    if (currentFrame.Setting == null)
                    {
                        log.Error(tokenizedLine, $"Attempted to exit setting \"{settingName}\" which was never entered.");
                    }
                    else if (currentFrame.Setting.Name != settingName)
                    {
                        log.Error(tokenizedLine, $"Attempted to exit setting \"{settingName}\", but expected to exit current setting with name \"{currentFrame.Setting.Name}\".");
                    }
                    else
                    {
                        currentFrame.Setting = null;
                    }

                    break;

                default:
                    throw new InternalCompilerException($"Cannot exit declared type {tree.DeclaredType}");
            }
        }
    }
}
