using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.MinigameFramework.Scripts.Framework.PlayerInfo {
    /// <summary>
    /// Barebones player class. Contains player index and PlayerInput.
    /// Can be extended to add more information. However, the constructor cannot accept any parameters.
    /// </summary>
    public class Player {
        public int playerIndex;
        /// <summary>
        /// Do not bind events to this directly. Use PawnBindingManager.BindPlayerInputToPawn instead.
        /// Do not use this to get the player's index. Use playerIndex instead.
        /// </summary>
        public PlayerInput playerInput;
        public int points = 0;
        public Color color;
        public bool isDummy = false;
    }
}