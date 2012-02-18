using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model {
    public class Actions {
        public List<Action> ActionList;
    }

    public class Action {
        public string Name;
        public int Damage;
        public List<string> Elements;
        public List<string> StatusEffects;
    }
}
