using DocriderParser.Models;
using System.Collections.Generic;

namespace DocriderParser.Compilation
{
    class StackObject
    {
        public Narrative Narrative { get; set; }
        public List<Character> Characters { get; set; } = new List<Character>();
        public List<Setting> Settings { get; set; } = new List<Setting>();
        public List<Act> Acts { get; set; } = new List<Act>();
        public List<Scene> Scenes { get; set; } = new List<Scene>();
    }

    class StackFrame
    {
        public Narrative Narrative { get; set; }
        public Setting Setting { get; set; }
        public Act Act { get; set; }
        public Scene Scene { get; set; }
    }
}
