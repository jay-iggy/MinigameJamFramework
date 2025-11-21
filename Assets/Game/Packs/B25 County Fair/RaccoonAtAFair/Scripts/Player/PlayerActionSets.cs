using UnityEngine;

namespace XiaoHuanXiong.Player
{
    public static class PlayerActionSets
    {
        public const PlayerAction ALL = PlayerAction.Move | PlayerAction.Attack | PlayerAction.Search;
        public const PlayerAction CombatLockedOnHit = PlayerAction.Move | PlayerAction.Attack | PlayerAction.Search;
    }

}
