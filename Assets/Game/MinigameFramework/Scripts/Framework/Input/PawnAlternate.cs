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
                case "Move":
                    OnMovement(context);
                    break;
                case "Look":
                    OnLook(context);
                    break;
                case "ButtonA":
                    OnButtonA();
                    break;
                case "ButtonB":
                    OnButtonB();
                    break;
                case "ButtonX":
                    OnActionX();
                    break;
                case "ButtonY":
                    OnActionY();
                    break;
                case "ButtonL":
                    OnActionL();
                    break;
                case "ButtonR":
                    OnActionR();
                    break;
            }
        }

        protected override void OnActionReleased(InputAction.CallbackContext context) {
            switch (context.action.name) {
                case "ButtonA":
                    OnActionAReleased();
                    break;
                case "ButtonB":
                    OnButtonBReleased();
                    break;
                case "ButtonX":
                    OnActionXReleased();
                    break;
                case "ButtonY":
                    OnActionYReleased();
                    break;
                case "ButtonL":
                    OnActionLReleased();
                    break;
                case "ButtonR":
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