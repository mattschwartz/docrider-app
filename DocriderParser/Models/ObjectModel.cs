using System;
using System.Collections.Generic;
using System.Text;

namespace DocriderParser.Models
{
    class ObjectModel
    {
        public Narrative Narrative { get; set; }
        public List<Character> Characters { get; set; }
        public Setting Setting { get; set; }
        public Act Act { get; set; }
        public Scene Scene { get; set; }
    }
}
