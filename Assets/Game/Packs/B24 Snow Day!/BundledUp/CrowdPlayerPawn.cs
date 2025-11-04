using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Tags;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace SnowDay.BundledUp {
    public class CrowdPlayerPawn : Pawn, IComparable<CrowdPlayerPawn> {
        public int playerPawnIndex;
        [SerializeField] private Color playerColor;
        [SerializeField] private float pedestrianSpeed = 3.25f;
        [SerializeField] private float pointerSpeed = 10f;

        private Vector2 _moveInput = Vector2.zero;
        private bool _select = false;

        private PedestrianBehavior controlledPedestrian, selectedPedestrian, highlightedPedestrian;
        [SerializeField] private GameObject pointer;
        private float selectionStartTime, highlightTime;

        private void Start() {
            pointer.SetActive(false);
        }

        private void Update() {
            if (CrowdGameManager.inst.currentGamePhase == CrowdGameManager.gamePhase.Search) {
                controlledPedestrian.SetVelocity(_moveInput * pedestrianSpeed);
            }
            else if (CrowdGameManager.inst.currentGamePhase == CrowdGameManager.gamePhase.Select) {
                Vector2 delta = _moveInput * pointerSpeed * Time.deltaTime;
                pointer.transform.position += new Vector3(delta.x, delta.y, 0);

                Vector2 boundaries = CrowdGameManager.inst.boundaries;
                Vector2 center = CrowdGameManager.inst.center;
                pointer.transform.position = new Vector2(
                    Mathf.Clamp(pointer.transform.position.x - center.x, -boundaries.x, boundaries.x),
                    Mathf.Clamp(pointer.transform.position.y - center.y, -boundaries.y, boundaries.y)
                ) + center;
            }
        }

        private void FixedUpdate() {
            if (CrowdGameManager.inst.currentGamePhase == CrowdGameManager.gamePhase.Select) {
                Collider2D col = Physics2D.OverlapCircle(pointer.transform.position, .01f);
                if (col != null) {
                    PedestrianBehavior newPedestrian = col.transform.GetComponent<PedestrianBehavior>();
                    if (selectedPedestrian != newPedestrian) {
                        if (selectedPedestrian != null) selectedPedestrian.Select(false);
                        selectedPedestrian = newPedestrian;
                        selectedPedestrian.Select(true);
                        AudioManager.inst.PlayHover();
                    }
                }
                else if (selectedPedestrian != null) {
                    selectedPedestrian.Select(false);
                    selectedPedestrian = null;
                }

                if (_select && selectedPedestrian != null && selectedPedestrian != highlightedPedestrian &&
                    !selectedPedestrian.IsHighlighted()) {
                    if (highlightedPedestrian != null) highlightedPedestrian.Highlight(Color.white);
                    highlightedPedestrian = selectedPedestrian;
                    highlightedPedestrian.Highlight(playerColor);
                    highlightTime = Time.time;
                    AudioManager.inst.PlayHighlight();
                }
            }
        }

        public void SceneChanged() {
            if (CrowdGameManager.inst.currentGamePhase == CrowdGameManager.gamePhase.Intro) {
                pointer.SetActive(false);
            }
            else if (CrowdGameManager.inst.currentGamePhase == CrowdGameManager.gamePhase.Search) {
                if (highlightedPedestrian != null) {
                    highlightedPedestrian.Highlight(Color.white);
                    highlightedPedestrian = null;
                }

                if (controlledPedestrian != null) {
                    controlledPedestrian.ChooseAttire();
                    controlledPedestrian.CeasePlayer();
                }

                controlledPedestrian = CrowdGameManager.inst.ChooseRandomPlayer(playerPawnIndex);
            }
            else if (CrowdGameManager.inst.currentGamePhase == CrowdGameManager.gamePhase.Select) {
                selectionStartTime = Time.time;
                pointer.SetActive(true);
                pointer.transform.position =
                    CrowdGameManager.inst.center + new Vector2(-1.25f + .75f * playerPawnIndex, 0);
            }
            else if (CrowdGameManager.inst.currentGamePhase == CrowdGameManager.gamePhase.Score) {
                if (selectedPedestrian != null) selectedPedestrian.Select(false);
                controlledPedestrian.SetAttire(playerColor);
            }
        }

        // Handle input
        protected override void OnActionPressed(InputAction.CallbackContext context) {
            if (context.action.name == "Move") _moveInput = context.ReadValue<Vector2>();
            if (context.action.name == "ButtonA") _select = true;
        }

        protected override void OnActionReleased(InputAction.CallbackContext context) {
            if (context.action.name == "ButtonA") _select = false;
        }

        public bool HighlightedAny() {
            return highlightedPedestrian != null;
        }

        public bool HighlightedCorrectPedestrian() {
            return HighlightedAny() ? highlightedPedestrian.controlledByIndex == playerPawnIndex : false;
        }

        public bool HighlightedStolen() {
            return !HighlightedCorrectPedestrian() && highlightedPedestrian.controlledByIndex != -1;
        }

        public float GetTimeToHighlight() {
            return highlightTime - selectionStartTime;
        }

        public int CompareTo(CrowdPlayerPawn b) {
            var a = this;

            if (a.GetTimeToHighlight() < b.GetTimeToHighlight())
                return -1;

            if (a.GetTimeToHighlight() > b.GetTimeToHighlight())
                return 1;

            return 0;
        }

        public Vector3 GetHighlightedPosition() {
            if (highlightedPedestrian != null) {
                return highlightedPedestrian.transform.position;
            }
            else {
                return pointer.transform.position;
            }
        }

        public Vector3 GetPointerPosition() {
            return pointer.transform.position;
        }

        public Color GetColor() {
            return playerColor;
        }
    }
}