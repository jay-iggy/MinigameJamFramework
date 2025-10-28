using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.MinigameFramework.Scripts.Framework.Input {
    /// <summary>
    /// Alternate version of Pawn. Breaks down the OnActionPressed and OnActionReleased methods into separate methods for each action.
    /// Create a new class that inherits from this class and override the methods you need.
    /// </summary>
    public abstract class PawnAlternate : Pawn {
        protected override void OnActionPressed(InputAction.CallbackContext context) {
            switch (context.action.name) {
                case PawnAction.Move:
                    OnMovement(context);
                    break;
                case PawnAction.Look:
                    OnLook(context);
                    break;
                case PawnAction.ButtonA:
                    OnButtonA();
                    break;
                case PawnAction.ButtonB:
                    OnButtonB();
                    break;
                case PawnAction.ButtonX:
                    OnActionX();
                    break;
                case PawnAction.ButtonY:
                    OnActionY();
                    break;
                case PawnAction.ButtonL:
                    OnActionL();
                    break;
                case PawnAction.ButtonR:
                    OnActionR();
                    break;
            }
        }

        protected override void OnActionReleased(InputAction.CallbackContext context) {
            switch (context.action.name) {
                case PawnAction.ButtonA:
                    OnActionAReleased();
                    break;
                case PawnAction.ButtonB:
                    OnButtonBReleased();
                    break;
                case PawnAction.ButtonX:
                    OnActionXReleased();
                    break;
                case PawnAction.ButtonY:
                    OnActionYReleased();
                    break;
                case PawnAction.ButtonL:
                    OnActionLReleased();
                    break;
                case PawnAction.ButtonR:
                    OnActionRReleased();
                    break;
            }
        }
        
        protected virtual void OnMovement(InputAction.CallbackContext context) { }
        protected virtual void OnLook(InputAction.CallbackContext context) { }
        protected virtual void OnButtonA() {}
        protected virtual void OnActionAReleased() { }
        protected virtual void OnButtonB() { }
        protected virtual void OnButtonBReleased() { }
        protected virtual void OnActionX() { }
        protected virtual void OnActionXReleased() { }
        protected virtual void OnActionY() { }
        protected virtual void OnActionYReleased() { }
        protected virtual void OnActionL() { }
        protected virtual void OnActionLReleased() { }
        protected virtual void OnActionR() { }
        protected virtual void OnActionRReleased() { }
    }
}