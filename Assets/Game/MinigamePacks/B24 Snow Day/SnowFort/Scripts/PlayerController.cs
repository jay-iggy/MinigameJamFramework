using Game.MinigameFramework.Scripts.Framework.Input;
using Snowfort;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Snowfort
{
    public class PlayerController : Pawn
    {
        public float cursorSpeed = 3f;

        public PlayerState playerState = PlayerState.OBSERVER;

        public PlacementCursor cursor;
        public FortController fort;

        Vector2 movement = Vector2.zero;

        bool ready;
        float readyTimer = 1f;

        public void SetState(PlayerState state)
        {
            if (playerIndex == -1) return;

            playerState = state;

            switch (state)
            {
                case PlayerState.PLACEMENT:
                    cursor.SetEnabledState(true);
                    fort.SetActiveState(false);
                    break;

                case PlayerState.BATTLE:
                    cursor.SetEnabledState(false);
                    fort.SetActiveState(true);
                    break;

                case PlayerState.OBSERVER:
                    cursor.SetEnabledState(false);
                    fort.SetActiveState(false);
                    break;
            }
        }

        protected override void OnActionPressed(InputAction.CallbackContext context)
        {
            switch (context.action.name)
            {
                case "Move":
                    movement = context.ReadValue<Vector2>();
                    break;

                case "Look":
                    break;

                case "ButtonA":
                    if (!context.performed && !context.started) return;
                    cursor.Place();
                    fort.Use();
                    break;

                case "ButtonB":
                    if (!context.performed) return;
                    cursor.Scroll(false);
                    fort.Scroll(false);
                    break;

                case "ButtonX":
                    if (!context.performed) return;
                    ready = true;
                    readyTimer = 1f;
                    break;

                case "ButtonY":
                    if (!context.performed) return;
                    cursor.Scroll(true);
                    fort.Scroll(true);
                    break;

                case "ButtonL":
                    break;

                case "ButtonR":
                    break;
            }
        }

        protected override void OnActionReleased(InputAction.CallbackContext context)
        {
            switch (context.action.name)
            {
                case "Move":
                    break;

                case "ButtonX":
                    ready = false;
                    break;
            }
        }

        void Update()
        {
            readyTimer -= Time.deltaTime;

            if (ready && readyTimer <= 0)
                SetState(PlayerState.OBSERVER);

            cursor.MoveCursor(movement * Time.deltaTime * cursorSpeed);
        }

        private void Start()
        {
            cursor = transform.GetChild(0).GetComponent<PlacementCursor>();
            fort = transform.GetChild(1).GetComponent<FortController>();

            cursor.SetEnabledState(false);
            fort.SetActiveState(false);
        }

        public enum PlayerState
        {
            PLACEMENT,
            BATTLE,
            OBSERVER
        }
    }
}