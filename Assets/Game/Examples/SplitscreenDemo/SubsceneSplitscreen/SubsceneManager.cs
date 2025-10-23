using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts;
using Game.MinigameFramework.Scripts.Framework;
using Game.MinigameFramework.Scripts.Framework.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Examples.Splitscreen {
    public class SubsceneManager : MonoBehaviour {
        public Pawn pawn;
        [SerializeField] Camera cam;
        public int playerIndex = -1;
        
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
            subscenePosition.x += 100 * playerIndex;
            transform.position = subscenePosition;
        }
        private void SetupSubsceneCamera() {
            cam.rect = _superManager.cameraRect[playerIndex];
        }
        private void BindPawn() {
            PawnBindingManager.BindPlayerInputToPawn(playerIndex, pawn);
        }

    }
}
