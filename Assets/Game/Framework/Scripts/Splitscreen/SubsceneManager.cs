using System;
using System.Collections;
using Game.MinigameFramework.Scripts.Framework;
using Game.MinigameFramework.Scripts.Framework.Input;
using UnityEngine;

namespace Examples.Splitscreen {
    /// <summary>
    /// Manages camera setup and pawn binding for a single subscene created by SplitscreenManager.
    /// </summary>
    public class SubsceneManager : MonoBehaviour {
        [SerializeField] Camera cam;
        public int playerIndex { get; private set; }
        
        private SplitscreenManager _superManager;

        private void Awake() {
            _superManager = SplitscreenManager.instance;
            playerIndex = _superManager.loadedSubscenes.Count;
            _superManager.loadedSubscenes.Add(this);
            SetSubscenePosition();
            SetupSubsceneCamera();
            BindPawn();
        }

        /// <summary>
        ///  Sets the position of the subscene to avoid overlapping with other subscenes.
        /// </summary>
        private void SetSubscenePosition() {
            Vector3 subscenePosition = transform.position;
            subscenePosition += _superManager.subsceneOffset * playerIndex;
            transform.position = subscenePosition;
        }
        private void SetupSubsceneCamera() {
            cam.rect = _superManager.cameraRect[playerIndex];
        }
        private void BindPawn() {
            Pawn pawn = GetComponentInChildren<Pawn>();
            PawnBindingManager.BindPlayerInputToPawn(playerIndex, pawn);
        }
    }
}
