using System;
using System.Collections.Generic;

namespace DocriderParser.Models
{
    class Scene
    {
        public string Name { get; }
        public Setting Setting { get; }
        public List<Character> Characters { get; } 

        internal Scene(SceneBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            Name = builder.Name;
            Setting = builder.Setting;
            Characters = builder.Characters;
        }

        public static SceneBuilder Builder()
        {
            return new SceneBuilder();
        }
    }

    class SceneBuilder
    {
        internal string Name;
        internal Setting Setting;
        internal List<Character> Characters;

        public SceneBuilder WithName(string name)
        {
            Name = name;
            return this;
        }

        public SceneBuilder WithSetting(Setting setting)
        {
            Setting = setting;
            return this;
        }

        public SceneBuilder WithCharacters(List<Character> characters)
        {
            Characters = characters;
            return this;
        }

        public Scene Build()
        {
            return new Scene(this);
        }
    }
}
