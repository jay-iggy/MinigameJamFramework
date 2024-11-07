using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.MinigameFramework.Scripts.Framework.Input {
    /// <summary>
    /// Pre-made pawn that will be bound to a PlayerInput and invokes events when the corresponding InputActions are performed.
    /// Used as a middleman between a custom character controller and the pawn system, so that a character controller doesn't need to be a pawn.
    /// </summary>
    public class PawnMiddleman : Pawn {
        /// <summary>
        /// Defines the delegates that will be invoked when the corresponding InputActions are performed.
        /// Converted into a usable type at Start so can only be added in Awake or in the inspector.
        /// </summary>
        [Tooltip("Events bound to the controls for this pawn. Converted into a usable type at start so can't add during play.")]
        public List<ControlBinding> controlBindings = new List<ControlBinding>();

        /// <summary>
        /// The control bindings set in editor converted into a usable form.
        /// </summary>
        private Dictionary<string, UnityEvent<InputAction.CallbackContext>> _boundEvents = new Dictionary<string, UnityEvent<InputAction.CallbackContext> >();
    
        private void Start() {
            foreach (ControlBinding controlBinding in controlBindings) {
                string key = controlBinding.controlToBind.ToString();
            
                if (controlBinding.onRelease) _boundEvents.Add(key + " Released", controlBinding.boundEvent);
                else _boundEvents.Add(key, controlBinding.boundEvent);
            }
        }

        protected override void OnActionPressed(InputAction.CallbackContext context) {
            if (_boundEvents.ContainsKey(context.action.name)) {
                _boundEvents[context.action.name].Invoke(context);
            }
        }
        protected override void OnActionReleased(InputAction.CallbackContext context) {
            if (_boundEvents.ContainsKey(context.action.name + " Released")) {
                _boundEvents[context.action.name + " Released"].Invoke(context);
            }
        }
    
        public enum ControlToBind {
            Move,
            Look,
            ButtonA,
            ButtonB,
            ButtonX,
            ButtonY,
            ButtonL,
            ButtonR,
        }
    
        [Serializable]
        public struct ControlBinding {
            [Tooltip("The input action to bind to.")]
            public ControlToBind controlToBind;
            [Tooltip("If true, the action will be triggered when the button is released, rather than pressed. Only works for Buttons, not Value types.")]
            public bool onRelease;
            public UnityEvent<InputAction.CallbackContext> boundEvent;
            
            public ControlBinding(ControlToBind controlToBind, UnityEvent<InputAction.CallbackContext> boundEvent, bool onRelease = false) {
                this.controlToBind = controlToBind;
                this.onRelease = onRelease;
                this.boundEvent = boundEvent;
            }
        }
    }
}