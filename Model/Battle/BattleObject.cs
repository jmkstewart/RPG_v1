using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Battle {
    public class BattleObject {
        public static int TimeToAction = 1000;

        public string Texture { get; set; }
        public string Name { get; set; }
        public int HP { get; set; }
        public int HPMax { get; set; }
        public int MP { get; set; }
        public int MPMax { get; set; }
        public int Id { get; set; }
        public int Str { get; set; }
        public int Speed { get; set; }
        public int CurrentTimeToAction { get; set; }
        public bool Enemy { get; set; }
        public string Status { get; set; }
        public List<string> AttackList { get; set; }
        public List<string> MagicList { get; set; }
        public List<string> ItemList { get; set; }

        public BattleObject() {
            CurrentTimeToAction = TimeToAction;
        }
    }
}
