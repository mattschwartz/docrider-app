using DocriderParser.Compilation;
using DocriderParser.Models;
using DocriderParser.Tokens;
using System.Collections.Generic;
using System.Linq;

namespace DocriderParser
{
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
            var currentFrame = new StackFrame();

            foreach (var tokenizedLine in tokenizedLines)
            {
                switch (tokenizedLine.SyntaxTree.Directive)
                {
                    case Directive.Define:
                        CompileDefinition(log, objectDefinitions, tokenizedLine);
                        break;

                    case Directive.Enter:
                        CompileEntrance(objectDefinitions, log, currentFrame, tokenizedLine);
                        break;

                    case Directive.Exit:
                        CompileExit(log, currentFrame, tokenizedLine);
                        break;

                    default:
                        log.Warning(tokenizedLine, $"Could not operate on unknown directive {tokenizedLine.SyntaxTree.Directive}.");
                        break;
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
        private void CompileDefinition(
            CompilerLog log,
            StackObject stack,
            TokenizedLine tokenizedLine)
        {
            SyntaxTree tree = tokenizedLine.SyntaxTree;

            switch (tree.DeclaredType)
            {
                case DeclaredType.Narrative:
                    if (stack.Narrative != null)
                    {
                        log.Error(tokenizedLine, $"Cannot define additional narrative \"{tree.AssignedValue}\". Narrative \"{stack.Narrative.Name}\" already defined.");
                    }
                    else
                    {
                        stack.Narrative = new Narrative(tree.AssignedValue.ToString());
                    }
                    break;

                case DeclaredType.Character:
                    var characterName = tree.AssignedValue.ToString();
                    var characterToAdd = new Character(characterName);

                    if (stack.Characters.Contains(characterToAdd))
                    {
                        log.Error(tokenizedLine, $"Duplicate Character definition for \"{characterName}\".");
                    }
                    else
                    {
                        stack.Characters.Add(characterToAdd);
                    }
                    break;

                case DeclaredType.Setting:
                    var settingName = tree.AssignedValue.ToString();
                    var setting = new Setting(settingName);

                    if (stack.Settings.Contains(setting))
                    {
                        log.Error(tokenizedLine, $"Duplicate Setting definition for \"{setting.Name}\".");
                    }
                    else
                    {
                        stack.Settings.Add(setting);
                    }
                    break;

                default:
                    log.Error(tokenizedLine, $"Cannot define declared type {tree.DeclaredType}");
                    break;
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
                    else
                    {
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
                    }
                    break;

                case DeclaredType.Scene:
                    var sceneName = tree.AssignedValue.ToString();
                    if (currentFrame.Scene != null)
                    {
                        log.Error(tokenizedLine, $"Cannot enter scene with name \"{sceneName}\" when already in scene \"{currentFrame.Scene.Name}\".");
                    }
                    else
                    {
                        var sceneToEnter = new Scene(sceneName);
                        if (objectDefinitions.Scenes.Contains(sceneToEnter))
                        {
                            log.Error(tokenizedLine, $"Cannot re-enter scene with name \"{sceneName}\".");
                        }
                        else
                        {
                            log.Debug(tokenizedLine, $"Entered scene \"{sceneName}\"");
                            objectDefinitions.Scenes.Add(sceneToEnter);
                            currentFrame.Scene = sceneToEnter;
                        }
                    }
                    break;

                case DeclaredType.Character:
                    var characterName = tree.AssignedValue.ToString();
                    var character = objectDefinitions.Characters.SingleOrDefault(t => t.Name == characterName);

                    if (character == null)
                    {
                        log.Error(tokenizedLine, $"Cannot enter undefined character \"{characterName}\".");
                    }
                    else
                    {
                        var currentScene = currentFrame.Scene;
                        if (currentScene == null)
                        {
                            log.Error(tokenizedLine, $"Cannot enter character \"{characterName}\" outside of a scene. Please enter a scene first.");
                        }
                        else
                        {
                            log.Debug(tokenizedLine, $"Entered Character \"{characterName}\"");
                            currentScene.Characters.Add(character);
                        }
                    }
                    break;

                case DeclaredType.Setting:
                    var settingName = tree.AssignedValue.ToString();
                    var setting = objectDefinitions.Settings.SingleOrDefault(t => t.Name == settingName);

                    if (setting == null)
                    {
                        log.Error(tokenizedLine, $"Cannot enter undefined setting \"{settingName}\".");
                    }
                    else
                    {
                        if (currentFrame.Setting != null)
                        {
                            log.Warning(tokenizedLine, $"Switching setting to \"{settingName}\" from current setting \"{currentFrame.Setting.Name}\".");
                        }

                        log.Debug(tokenizedLine, $"Entered setting \"{settingName}\"");
                        currentFrame.Setting = setting;
                    }
                    break;

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
                    if (currentFrame.Act == null)
                    {
                        log.Warning(tokenizedLine, $"Attempted to exit act but currently not in an act.");
                    }
                    else
                    {
                        log.Debug(tokenizedLine, $"Exiting act \"{currentFrame.Act.Name}\"");
                        currentFrame.Act = null;
                    }

                    break;

                case DeclaredType.Scene:
                    if (currentFrame.Scene == null)
                    {
                        log.Warning(tokenizedLine, $"Attempted to exit scene but currently not in a scene.");
                    }
                    else if (currentFrame.Scene.Characters.Count != 0)
                    {
                        log.Warning(tokenizedLine, "Exiting scene with characters present will exeunt all characters. Did you mean to do this?");
                        currentFrame.Scene = null;
                    }
                    else
                    {
                        log.Debug(tokenizedLine, $"Exiting scene \"{currentFrame.Scene.Name}\"");
                        currentFrame.Scene = null;
                    }
                    break;

                case DeclaredType.Character:
                    var characterName = tree.AssignedValue.ToString();

                    if (currentFrame.Scene == null)
                    {
                        log.Error(tokenizedLine, $"Cannot exit character outside of scene. Please enter a scene first.");
                    }
                    else
                    {
                        Character characterToExit = currentFrame.Scene.Characters
                            .SingleOrDefault(t => t.Name == characterName);

                        if (characterToExit == null)
                        {
                            log.Warning(tokenizedLine, $"Attempted to exit character \"{characterName}\" who was not present in scene.");
                        }
                        else
                        {
                            log.Debug(tokenizedLine, $"Exiting character \"{characterName}\"");
                            currentFrame.Scene.Characters.Remove(characterToExit);
                        }
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
                        log.Debug(tokenizedLine, $"Exiting setting \"{settingName}\"");
                        currentFrame.Setting = null;
                    }

                    break;

                default:
                    throw new InternalCompilerException($"Cannot exit declared type {tree.DeclaredType}");
            }
        }
    }
}
