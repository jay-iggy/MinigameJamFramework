using System.Collections.Generic;
using UnityEngine;

namespace XiaoHuanXiong.Player
{
    public class ActionLocker
    {
        private readonly Dictionary<PlayerAction, int> _actionLocks = new Dictionary<PlayerAction, int>();

        public void Lock(PlayerAction action)
        {
            if (_actionLocks.ContainsKey(action))
            {
                _actionLocks[action]++;
            }
            else
            {
                _actionLocks[action] = 1;
            }
        }

        public void Unlock(PlayerAction action)
        {
            if (_actionLocks.ContainsKey(action))
            {
                _actionLocks[action]--;
                if (_actionLocks[action] <= 0)
                {
                    _actionLocks.Remove(action);
                }
            }
            else
            {
                Debug.LogWarning($"Attempted to unlock action {action} which is not currently locked.");
            }
        }

        public bool IsLocked(PlayerAction action)
        {
            return _actionLocks.ContainsKey(action);
        }
    }
}

