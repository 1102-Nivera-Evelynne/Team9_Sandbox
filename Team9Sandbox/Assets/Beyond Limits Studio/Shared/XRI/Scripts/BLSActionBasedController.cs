using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace BeyondLimitsStudios
{
    namespace VRInteractables
    {
        public partial class BLSActionBasedController : ActionBasedController
        {
            [SerializeField]
            InputActionProperty m_ActivateButtonPrimaryAction = new InputActionProperty(new InputAction(type: InputActionType.Button));
            /// <summary>
            /// The Input System action to use for activating a selected Interactable.
            /// Must be an action with a button-like interaction where phase equals performed when pressed.
            /// Typically a <see cref="ButtonControl"/> Control or a Value type action with a Press or Sector interaction.
            /// </summary>
            /// <seealso cref="activateButtonPrimaryActionValue"/>
            public InputActionProperty activateButtonPrimaryAction
            {
                get => m_ActivateButtonPrimaryAction;
                set => SetInputActionProperty(ref m_ActivateButtonPrimaryAction, value);
            }

            [SerializeField]
            InputActionProperty m_ActivateButtonSecondaryAction = new InputActionProperty(new InputAction(type: InputActionType.Button));
            /// <summary>
            /// The Input System action to use for activating a selected Interactable.
            /// Must be an action with a button-like interaction where phase equals performed when pressed.
            /// Typically a <see cref="ButtonControl"/> Control or a Value type action with a Press or Sector interaction.
            /// </summary>
            /// <seealso cref="activateSecondaryActionValue"/>
            public InputActionProperty activateButtonSecondaryAction
            {
                get => m_ActivateButtonSecondaryAction;
                set => SetInputActionProperty(ref m_ActivateButtonSecondaryAction, value);
            }

            protected override void OnEnable()
            {
                base.OnEnable();
                EnableAllDirectActions();
            }

            /// <inheritdoc />
            protected override void OnDisable()
            {
                base.OnDisable();
                DisableAllDirectActions();
            }

            void EnableAllDirectActions()
            {
                m_ActivateButtonPrimaryAction.EnableDirectAction();
                m_ActivateButtonSecondaryAction.EnableDirectAction();
            }

            void DisableAllDirectActions()
            {
                m_ActivateButtonPrimaryAction.DisableDirectAction();
                m_ActivateButtonSecondaryAction.DisableDirectAction();
            }

            void SetInputActionProperty(ref InputActionProperty property, InputActionProperty value)
            {
                if (Application.isPlaying)
                    property.DisableDirectAction();

                property = value;

                if (Application.isPlaying && isActiveAndEnabled)
                    property.EnableDirectAction();
            }

            protected override void UpdateInput(XRControllerState controllerState)
            {
                base.UpdateInput(controllerState);

                if (controllerState == null)
                    return;

                XRBLSControllerState state = controllerState as XRBLSControllerState;

                if (state == null)
                    return;

                var activateButtonPrimaryValueAction = m_ActivateButtonPrimaryAction.action;
                if (activateButtonPrimaryValueAction == null || activateButtonPrimaryValueAction.bindings.Count <= 0)
                    activateButtonPrimaryValueAction = m_ActivateButtonPrimaryAction.action;
                state.activatePrimaryButtonInteractionState.SetFrameState(IsPressed(m_ActivateButtonPrimaryAction.action), ReadValue(activateButtonPrimaryValueAction));

                var activateButtonSecondaryValueAction = m_ActivateButtonSecondaryAction.action;
                if (activateButtonSecondaryValueAction == null || activateButtonSecondaryValueAction.bindings.Count <= 0)
                    activateButtonSecondaryValueAction = m_ActivateButtonSecondaryAction.action;
                state.activateSecondaryButtonInteractionState.SetFrameState(IsPressed(m_ActivateButtonSecondaryAction.action), ReadValue(activateButtonSecondaryValueAction));
            }
        }
    }
}