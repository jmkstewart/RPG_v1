using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace View.Battle {
    public enum BattlePosition {
        EnemyLeft = 0, EnemyMiddle = 1, EnemyRight = 2,
        PlayerLeft = 3, PlayerMiddle = 4, PlayerRight = 5
    }

    public interface InteractableBattleObjectView : IBattleObjectView {
        void StartDamageText(int damage);
        string Name { get; }
        bool Selected { get; set; }
        BattlePosition CurrentBattlePosition { get; set; }
    }
}
