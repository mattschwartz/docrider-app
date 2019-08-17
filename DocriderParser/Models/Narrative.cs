using System.Collections.Generic;

namespace DocriderParser.Models
{
    class Narrative
    {
        public string Name { get; }
        public List<Act> Acts { get; } = new List<Act>();
        public List<Scene> Scenes { get; } = new List<Scene>();
        public List<Setting> Settings { get; } = new List<Setting>();
        public List<Character> Characters { get; } = new List<Character>();

        public Narrative(string name)
        {
            Name = name;
        }
    }
}
