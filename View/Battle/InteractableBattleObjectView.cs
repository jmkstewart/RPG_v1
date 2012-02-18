using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace View.Battle {
    public interface InteractableBattleObjectView : IBattleObjectView {
        void StartDamageText(int damage);
        string Name { get; }
    }
}
