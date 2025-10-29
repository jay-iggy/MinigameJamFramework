using Game.MinigameFramework.Scripts.Framework.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SnowDay.Snowfort
{
    public class PlacementCursorTester : Pawn
    {
        public BuildingObject obj;
        public float cursorSpeed = 3;

        Vector2 movement = Vector2.zero;

        protected override void OnActionPressed(InputAction.CallbackContext context)
        {
            if (context.action.name == "Move") movement = context.ReadValue<Vector2>();
            if (context.action.name == "ButtonA") transform.GetChild(0).GetComponent<PlacementCursor>().Place();
        }

        void Update()
        {
            transform.GetChild(0).GetComponent<PlacementCursor>().MoveCursor(movement * Time.deltaTime * cursorSpeed);
        }

        private void Start()
        {
            transform.GetChild(0).GetComponent<PlacementCursor>().AddObject(obj, 3);
        }
    }
}