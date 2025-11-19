using Game.MinigameFramework.Scripts.Framework.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Starter.PumpkinPlunge {
    public class PumpkinPawn : Pawn {

        [Header("References")]
        public ButtonPanel buttonPanel;
        public GameObject pumpkin;

        public new Rigidbody rigidbody => pumpkin.GetComponent<Rigidbody>();
        public new Collider collider => pumpkin.GetComponent<Collider>();
        public Vector3 pumpkinOffset;

        void Start()
        {
            pumpkinOffset = pumpkin.transform.localPosition;
        }

        public void SetMaterial(Material material)
        {
            pumpkin.GetComponent<MeshRenderer>().material = material;
        }

        public Vector3 GetFocusPosition()
        {
            return pumpkin.transform.position;
        }

        protected override void OnActionPressed(InputAction.CallbackContext context) {
            if (context.action.name == PawnAction.ButtonA) {
                // Activate panel
                buttonPanel.Activate();
            }

            if (context.action.name == PawnAction.ButtonL)
            {
                // Select left type
                buttonPanel.MoveLeft();
            }
            else if (context.action.name == PawnAction.ButtonR)
            {
                // Move panel indicator right
                buttonPanel.MoveRight();
            }
        }

        // protected override void OnActionReleased(InputAction.CallbackContext context) {
        //     if (context.action.name == PawnAction.ButtonB) {
        //         // Stop shooting
        //     }
        // }
    }
}