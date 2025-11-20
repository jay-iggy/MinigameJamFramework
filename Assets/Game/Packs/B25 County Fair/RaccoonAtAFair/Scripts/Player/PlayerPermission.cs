using System;
using System.Collections.Generic;
using UnityEngine;
using XiaoHuanXiong.Common;
using XiaoHuanXiong.Game;

namespace XiaoHuanXiong.Player
{
    
    [System.Flags]
    public enum PlayerAction
    {
        None = 0,
        Move = 1 << 0,
        Attack = 1 << 1,
        Search = 1 << 2,
    }

    public class PlayerPermission : MonoBehaviour
    {
        #region private class
        private class Scope : IDisposable
        {
            private Action _onDispose;
            public Scope(Action onDispose)
            {
                _onDispose = onDispose;
            }
            public void Dispose()
            {
                _onDispose?.Invoke();
            }
        }
        #endregion

        private ActionLocker _locker = new ActionLocker();

        private readonly Dictionary<GameMode, PlayerAction> _allow = new Dictionary<GameMode, PlayerAction>()
    {
        { GameMode.BeforeStart, PlayerAction.None},
        { GameMode.Playing, PlayerAction.Move | PlayerAction.Attack | PlayerAction.Search},
        { GameMode.Ending, PlayerAction.None},
    };

        public IDisposable ScopedLock(PlayerAction action)
        {
            _locker.Lock(action);
            return new Scope(() => _locker.Unlock(action));
        }

        public bool CanPerformAction(PlayerAction action)
        {
            return _isGameModeAllowed(action) && !_locker.IsLocked(action);
        }



        private bool _isGameModeAllowed(PlayerAction action)
        {
            if (_allow.TryGetValue(GameManager.Instance.currentGameMode, out var allowedActions))
            {
                return (allowedActions & action) == action;
            }
            Debug.LogError($"GameManager: No allowed actions defined for game mode {GameManager.Instance.currentGameMode}");
            return false;
        }

    }
}


