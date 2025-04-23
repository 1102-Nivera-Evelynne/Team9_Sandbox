using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;



namespace BeyondLimitsStudios
{
    namespace VRInteractables
    {
        public static class XRBaseInteractableExtension
        {
            /// <summary>
            /// Force deselect the selected interactable.
            ///
            /// This is an extension method for <c>XRBaseInteractable</c>.
            /// </summary>
            /// <param name="interactable">Interactable that has been selected by some interactor</param>
            public static void ForceDeselect(this UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable)
            {
                interactable.interactionManager.CancelInteractorSelection((IXRSelectInteractor)interactable);

                Assert.IsFalse(interactable.isSelected);
            }
        }

        public static class XRBaseInteractorExtension
        {
            /// <summary>
            /// Force deselect any selected interactable for given interactor.
            ///
            /// This is an extension method for <c>XRBaseInteractor</c>.
            /// </summary>
            /// <param name="interactor">Interactor that has some interactable selected</param>
            public static void ForceDeselect(this UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor)
            {
                interactor.interactionManager.CancelInteractorSelection((IXRSelectInteractor)interactor);
                Assert.IsFalse(interactor.isSelectActive);
            }
        }
    }
}