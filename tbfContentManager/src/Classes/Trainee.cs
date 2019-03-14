using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tbfContentManager.Classes
{
    class Trainee
    {
        public int ID { get; set; }
        public bool IsInRoomSelection { get; set; }
        public string Nickname { get; set; }
        public string Mailadress { get; set; }

        public Trainee(int ID, bool isInRoomSelection, string nickname, string mailadresse)
        {
            this.ID = ID;
            this.IsInRoomSelection = isInRoomSelection;
            this.Nickname = nickname;
            this.Mailadress = mailadresse;
        }
    }
}
