using System.Collections.Generic;

namespace ProjectT
{
    class JsonChatter
    {
        public Links _links { get; set; }
        public int chatter_count { get; set; }
        public Chatters chatters { get; set; }

        public JsonChatter()
        {

        }
        public class Links
        {

        }

        public class Chatters
        {
            public List<string> broadcaster { get; set; }
            public List<string> vips { get; set; }
            public List<string> moderators { get; set; }
            public List<string> staff { get; set; }
            public List<string> admins { get; set; }
            public List<string> global_mods { get; set; }
            public List<string> viewers { get; set; }
        }
    }
}
