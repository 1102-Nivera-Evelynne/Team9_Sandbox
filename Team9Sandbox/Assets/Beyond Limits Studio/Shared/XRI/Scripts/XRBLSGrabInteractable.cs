using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_EDITOR
using UnityEditor;

#endif

namespace BeyondLimitsStudios
{
    namespace VRInteractables
    {
        public class XRBLSGrabInteractable : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
        {
            #region casual activate

            [SerializeField]
            ActivateEvent m_PrimaryButtonActivated = new ActivateEvent();

            public ActivateEvent primaryButtonActivated
            {
                get => m_PrimaryButtonActivated;
                set => m_PrimaryButtonActivated = value;
            }

            [SerializeField]
            DeactivateEvent m_PrimaryButtonDeactivated = new DeactivateEvent();

            public DeactivateEvent primaryButtonDeactivated
            {
                get => m_PrimaryButtonDeactivated;
                set => m_PrimaryButtonDeactivated = value;
            }

            [SerializeField]
            ActivateEvent m_SecondaryButtonActivated = new ActivateEvent();

            public ActivateEvent secondaryButtonActivated
            {
                get => m_SecondaryButtonActivated;
                set => m_SecondaryButtonActivated = value;
            }

            [SerializeField]
            DeactivateEvent m_SecondaryButtonDeactivated = new DeactivateEvent();

            public DeactivateEvent secondaryButtonDeactivated
            {
                get => m_SecondaryButtonDeactivated;
                set => m_SecondaryButtonDeactivated = value;
            }

            #endregion

            #region primary attach activate

            [SerializeField]
            ActivateEvent m_PrimaryAttachActivated = new ActivateEvent();

            public ActivateEvent primaryAttachActivated
            {
                get => m_PrimaryAttachActivated;
                set => m_PrimaryAttachActivated = value;
            }

            [SerializeField]
            DeactivateEvent m_PrimaryAttachDeactivated = new DeactivateEvent();

            public DeactivateEvent primaryAttachDeactivated
            {
                get => m_PrimaryAttachDeactivated;
                set => m_PrimaryAttachDeactivated = value;
            }

            [SerializeField]
            ActivateEvent m_PrimaryAttachPrimaryButtonActivated = new ActivateEvent();

            public ActivateEvent primaryAttachPrimaryButtonActivated
            {
                get => m_PrimaryAttachPrimaryButtonActivated;
                set => m_PrimaryAttachPrimaryButtonActivated = value;
            }

            [SerializeField]
            DeactivateEvent m_PrimaryAttachPrimaryButtonDeactivated = new DeactivateEvent();

            public DeactivateEvent primaryAttachPrimaryButtonDeactivated
            {
                get => m_PrimaryAttachPrimaryButtonDeactivated;
                set => m_PrimaryAttachPrimaryButtonDeactivated = value;
            }

            [SerializeField]
            ActivateEvent m_PrimaryAttachSecondaryButtonActivated = new ActivateEvent();

            public ActivateEvent primaryAttachSecondaryButtonActivated
            {
                get => m_PrimaryAttachSecondaryButtonActivated;
                set => m_PrimaryAttachSecondaryButtonActivated = value;
            }

            [SerializeField]
            DeactivateEvent m_PrimaryAttachSecondaryButtonDeactivated = new DeactivateEvent();

            public DeactivateEvent primaryAttachSecondaryButtonDeactivated
            {
                get => m_PrimaryAttachSecondaryButtonDeactivated;
                set => m_PrimaryAttachSecondaryButtonDeactivated = value;
            }

            #endregion

            #region secondary attach activate

            [SerializeField]
            ActivateEvent m_SecondaryAttachActivated = new ActivateEvent();

            public ActivateEvent secondaryAttachActivated
            {
                get => m_SecondaryAttachActivated;
                set => m_SecondaryAttachActivated = value;
            }

            [SerializeField]
            DeactivateEvent m_SecondaryAttachDeactivated = new DeactivateEvent();

            public DeactivateEvent secondaryAttachDeactivated
            {
                get => m_SecondaryAttachDeactivated;
                set => m_SecondaryAttachDeactivated = value;
            }

            [SerializeField]
            ActivateEvent m_SecondaryAttachPrimaryButtonActivated = new ActivateEvent();

            public ActivateEvent secondaryAttachPrimaryButtonActivated
            {
                get => m_SecondaryAttachPrimaryButtonActivated;
                set => m_SecondaryAttachPrimaryButtonActivated = value;
            }

            [SerializeField]
            DeactivateEvent m_SecondaryAttachPrimaryButtonDeactivated = new DeactivateEvent();

            public DeactivateEvent secondaryAttachPrimaryButtonDeactivated
            {
                get => m_SecondaryAttachPrimaryButtonDeactivated;
                set => m_SecondaryAttachPrimaryButtonDeactivated = value;
            }

            [SerializeField]
            ActivateEvent m_SecondaryAttachSecondaryButtonActivated = new ActivateEvent();

            public ActivateEvent secondaryAttachSecondaryButtonActivated
            {
                get => m_SecondaryAttachSecondaryButtonActivated;
                set => m_SecondaryAttachSecondaryButtonActivated = value;
            }

            [SerializeField]
            DeactivateEvent m_SecondaryAttachSecondaryButtonDeactivated = new DeactivateEvent();

            public DeactivateEvent secondaryAttachSecondaryButtonDeactivated
            {
                get => m_SecondaryAttachSecondaryButtonDeactivated;
                set => m_SecondaryAttachSecondaryButtonDeactivated = value;
            }

            #endregion

            // Dictionary<InputAction, AttachInfo> attachInfosDict = new Dictionary<InputAction, AttachInfo>();

            [SerializeField]
            protected AttachInfo primaryAttachInfo = new AttachInfo();
            [SerializeField]
            protected AttachInfo secondaryAttachInfo = new AttachInfo();

            bool primaryAttachTaken = false;
            bool secondaryAttachTaken = false;

            protected override void OnSelectEntered(SelectEnterEventArgs args)
            {
                UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor;

                if (interactor == null)
                {
                    base.OnSelectEntered(args);
                    return;
                }

                BLSActionBasedController controller = interactor.xrController as BLSActionBasedController;

                if (controller == null)
                {
                    base.OnSelectEntered(args);
                    return;
                }

                controller.activateButtonPrimaryAction.reference.action.started += OnPrimaryButtonActivated;
                controller.activateButtonPrimaryAction.reference.action.canceled += OnPrimaryButtonDeactivated;

                controller.activateButtonSecondaryAction.reference.action.started += OnSecondaryButtonActivated;
                controller.activateButtonSecondaryAction.reference.action.canceled += OnSecondaryButtonDeactivated;

                var attach = GetAttachTransform(args.interactorObject);

                bool isPrimaryAttach = attach == attachTransform;
                bool isSecondaryAttach = attach == secondaryAttachTransform;

                if (isPrimaryAttach)
                {
                    primaryAttachTaken = true;
                    primaryAttachInfo.currentInteractor = args.interactorObject;
                    primaryAttachInfo.associatedActions = new List<InputAction>() { controller.activateButtonPrimaryAction.reference.action, controller.activateButtonSecondaryAction.reference.action };
                    primaryAttachInfo.associatedTransform = attach;
                }
                else if (isSecondaryAttach)
                {
                    secondaryAttachTaken = true;
                    secondaryAttachInfo.currentInteractor = args.interactorObject;
                    secondaryAttachInfo.associatedActions = new List<InputAction>() { controller.activateButtonPrimaryAction.reference.action, controller.activateButtonSecondaryAction.reference.action };
                    secondaryAttachInfo.associatedTransform = attach;
                }
                else
                {
                    primaryAttachTaken = true;
                    primaryAttachInfo.currentInteractor = args.interactorObject;
                    primaryAttachInfo.associatedActions = new List<InputAction>() { controller.activateButtonPrimaryAction.reference.action, controller.activateButtonSecondaryAction.reference.action };
                    primaryAttachInfo.associatedTransform = attach;
                }

                base.OnSelectEntered(args);
            }

            protected override void OnSelectExited(SelectExitEventArgs args)
            {
                UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor;

                if (interactor == null)
                {
                    base.OnSelectExited(args);
                    return;
                }

                BLSActionBasedController controller = interactor.xrController as BLSActionBasedController;

                if (controller == null)
                {
                    base.OnSelectExited(args);
                    return;
                }

                controller.activateButtonPrimaryAction.reference.action.started -= OnPrimaryButtonActivated;
                controller.activateButtonPrimaryAction.reference.action.canceled -= OnPrimaryButtonDeactivated;

                controller.activateButtonSecondaryAction.reference.action.started -= OnSecondaryButtonActivated;
                controller.activateButtonSecondaryAction.reference.action.canceled -= OnSecondaryButtonDeactivated;

                if (primaryAttachInfo.currentInteractor == args.interactorObject)
                {
                    primaryAttachTaken = false;
                    primaryAttachInfo.currentInteractor = null;
                    primaryAttachInfo.associatedActions.Clear();
                    primaryAttachInfo.associatedTransform = null;
                }
                else if (secondaryAttachInfo.currentInteractor == args.interactorObject)
                {
                    secondaryAttachTaken = false;
                    secondaryAttachInfo.currentInteractor = null;
                    secondaryAttachInfo.associatedActions.Clear();
                    secondaryAttachInfo.associatedTransform = null;
                }
                else
                {
                    primaryAttachTaken = false;
                    primaryAttachInfo.currentInteractor = null;
                    primaryAttachInfo.associatedActions.Clear();
                    primaryAttachInfo.associatedTransform = null;
                }

                base.OnSelectExited(args);
            }

            public override Transform GetAttachTransform(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor interactor)
            {
                if (attachTransform == null || secondaryAttachTransform == null)
                    return base.GetAttachTransform(interactor);

                // bool isFirst = interactorsSelecting.Count <= 1 || ReferenceEquals(interactor, interactorsSelecting[0]) || ReferenceEquals(interactor, interactorsSelecting[0]);
                bool isFirst = interactorsSelecting.Count <= 1;

                if ((UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor)primaryAttachInfo.currentInteractor == interactor)
                    return primaryAttachInfo.associatedTransform;

                if ((UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor)secondaryAttachInfo.currentInteractor == interactor)
                    return secondaryAttachInfo.associatedTransform;

                if (isFirst || (!primaryAttachTaken && !secondaryAttachTaken))
                {
                    float d1 = Vector3.SqrMagnitude(interactor.GetAttachTransform(this).position - attachTransform.position);
                    float d2 = Vector3.SqrMagnitude(interactor.GetAttachTransform(this).position - secondaryAttachTransform.position);

                    if (d1 <= d2)
                        return attachTransform;
                    else
                        return secondaryAttachTransform;
                }

                if (primaryAttachTaken)
                    return secondaryAttachTransform;

                if (secondaryAttachTaken)
                    return attachTransform;

                return base.GetAttachTransform(interactor);
            }

            public Transform GetInteractorTransform(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor interactor)
            {
                if ((UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor)primaryAttachInfo.currentInteractor == interactor)
                    return primaryAttachInfo.associatedTransform;

                if ((UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor)secondaryAttachInfo.currentInteractor == interactor)
                    return secondaryAttachInfo.associatedTransform;

                return null;
            }

            protected override void OnActivated(ActivateEventArgs args)
            {
                base.OnActivated(args);

                bool primaryAttachInteraction = false;
                bool secondaryAttachInteraction = false;

                if (primaryAttachInfo.currentInteractor == args.interactorObject)
                {
                    primaryAttachInteraction = true;
                }
                else if (secondaryAttachInfo.currentInteractor == args.interactorObject)
                {
                    secondaryAttachInteraction = true;
                }

                if (primaryAttachInteraction)
                    m_PrimaryAttachActivated?.Invoke(args);

                if (secondaryAttachInteraction)
                    m_SecondaryAttachActivated?.Invoke(args);
            }

            protected override void OnDeactivated(DeactivateEventArgs args)
            {
                base.OnDeactivated(args);

                bool primaryAttachInteraction = false;
                bool secondaryAttachInteraction = false;

                if (primaryAttachInfo.currentInteractor == args.interactorObject)
                {
                    primaryAttachInteraction = true;
                }
                else if (secondaryAttachInfo.currentInteractor == args.interactorObject)
                {
                    secondaryAttachInteraction = true;
                }

                if (primaryAttachInteraction)
                    m_PrimaryAttachDeactivated?.Invoke(args);

                if (secondaryAttachInteraction)
                    m_SecondaryAttachDeactivated?.Invoke(args);
            }

            public void OnPrimaryButtonActivated(InputAction.CallbackContext context)
            {
                ActivateEventArgs args = new ActivateEventArgs();

                args.interactableObject = this;

                bool primaryAttachInteraction = false;
                bool secondaryAttachInteraction = false;

                if (primaryAttachInfo.associatedActions.Contains(context.action))
                {
                    primaryAttachInteraction = true;
                    args.interactorObject = primaryAttachInfo.currentInteractor as UnityEngine.XR.Interaction.Toolkit.Interactors.IXRActivateInteractor;
                }
                else if (secondaryAttachInfo.associatedActions.Contains(context.action))
                {
                    secondaryAttachInteraction = true;
                    args.interactorObject = secondaryAttachInfo.currentInteractor as UnityEngine.XR.Interaction.Toolkit.Interactors.IXRActivateInteractor;
                }

                m_PrimaryButtonActivated?.Invoke(args);

                if (primaryAttachInteraction)
                    m_PrimaryAttachPrimaryButtonActivated?.Invoke(args);

                if (secondaryAttachInteraction)
                    m_SecondaryAttachPrimaryButtonActivated?.Invoke(args);
            }

            public void OnPrimaryButtonDeactivated(InputAction.CallbackContext context)
            {
                DeactivateEventArgs args = new DeactivateEventArgs();

                args.interactableObject = this;

                bool primaryAttachInteraction = false;
                bool secondaryAttachInteraction = false;

                if (primaryAttachInfo.associatedActions.Contains(context.action))
                {
                    primaryAttachInteraction = true;
                    args.interactorObject = primaryAttachInfo.currentInteractor as UnityEngine.XR.Interaction.Toolkit.Interactors.IXRActivateInteractor;
                }
                else if (secondaryAttachInfo.associatedActions.Contains(context.action))
                {
                    secondaryAttachInteraction = true;
                    args.interactorObject = secondaryAttachInfo.currentInteractor as UnityEngine.XR.Interaction.Toolkit.Interactors.IXRActivateInteractor;
                }

                m_PrimaryButtonDeactivated?.Invoke(args);

                if (primaryAttachInteraction)
                    m_PrimaryAttachPrimaryButtonDeactivated?.Invoke(args);

                if (secondaryAttachInteraction)
                    m_SecondaryAttachPrimaryButtonDeactivated?.Invoke(args);
            }

            public void OnSecondaryButtonActivated(InputAction.CallbackContext context)
            {
                ActivateEventArgs args = new ActivateEventArgs();

                args.interactableObject = this;

                bool primaryAttachInteraction = false;
                bool secondaryAttachInteraction = false;

                if (primaryAttachInfo.associatedActions.Contains(context.action))
                {
                    primaryAttachInteraction = true;
                    args.interactorObject = primaryAttachInfo.currentInteractor as UnityEngine.XR.Interaction.Toolkit.Interactors.IXRActivateInteractor;
                }
                else if (secondaryAttachInfo.associatedActions.Contains(context.action))
                {
                    secondaryAttachInteraction = true;
                    args.interactorObject = secondaryAttachInfo.currentInteractor as UnityEngine.XR.Interaction.Toolkit.Interactors.IXRActivateInteractor;
                }

                m_SecondaryButtonActivated?.Invoke(args);

                if (primaryAttachInteraction)
                    m_PrimaryAttachSecondaryButtonActivated?.Invoke(args);

                if (secondaryAttachInteraction)
                    m_SecondaryAttachSecondaryButtonActivated?.Invoke(args);
            }

            public void OnSecondaryButtonDeactivated(InputAction.CallbackContext context)
            {
                DeactivateEventArgs args = new DeactivateEventArgs();

                args.interactableObject = this;

                bool primaryAttachInteraction = false;
                bool secondaryAttachInteraction = false;

                if (primaryAttachInfo.associatedActions.Contains(context.action))
                {
                    primaryAttachInteraction = true;
                    args.interactorObject = primaryAttachInfo.currentInteractor as UnityEngine.XR.Interaction.Toolkit.Interactors.IXRActivateInteractor;
                }
                else if (secondaryAttachInfo.associatedActions.Contains(context.action))
                {
                    secondaryAttachInteraction = true;
                    args.interactorObject = secondaryAttachInfo.currentInteractor as UnityEngine.XR.Interaction.Toolkit.Interactors.IXRActivateInteractor;
                }

                m_SecondaryButtonDeactivated?.Invoke(args);

                if (primaryAttachInteraction)
                    m_PrimaryAttachSecondaryButtonDeactivated?.Invoke(args);

                if (secondaryAttachInteraction)
                    m_SecondaryAttachSecondaryButtonDeactivated?.Invoke(args);
            }

            // [System.Serializable]
            protected struct AttachInfo
            {
                [System.NonSerialized]
                public List<InputAction> associatedActions;
                [System.NonSerialized]
                public UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor currentInteractor;
                [System.NonSerialized]
                public Transform associatedTransform;
                // public List<Transform> interactableAttaches;
            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(XRBLSGrabInteractable), true), CanEditMultipleObjects]
        class XRBLSGrabInteractableInspector : UnityEditor.XR.Interaction.Toolkit.Interactables.XRBaseInteractableEditor
        {
            protected SerializedProperty primaryAttachInfo;
            protected SerializedProperty secondaryAttachInfo;

            // additional activate
            protected SerializedProperty m_PrimaryButtonActivated;
            protected SerializedProperty m_OnPrimaryButtonActivateCalls;

            protected SerializedProperty m_PrimaryButtonDeactivated;
            protected SerializedProperty m_OnPrimaryButtonDeactivateCalls;

            protected SerializedProperty m_SecondaryButtonActivated;
            protected SerializedProperty m_OnSecondaryButtonActivateCalls;

            protected SerializedProperty m_SecondaryButtonDeactivated;
            protected SerializedProperty m_OnSecondaryButtonDeactivateCalls;

            // primary attach additional activate
            protected SerializedProperty m_PrimaryAttachActivated;
            protected SerializedProperty m_OnPrimaryAttachActivateCalls;

            protected SerializedProperty m_PrimaryAttachDeactivated;
            protected SerializedProperty m_OnPrimaryAttachDeactivateCalls;

            protected SerializedProperty m_PrimaryAttachPrimaryButtonActivated;
            protected SerializedProperty m_OnPrimaryAttachPrimaryButtonActivateCalls;

            protected SerializedProperty m_PrimaryAttachPrimaryButtonDeactivated;
            protected SerializedProperty m_OnPrimaryAttachPrimaryButtonDeactivateCalls;

            protected SerializedProperty m_PrimaryAttachSecondaryButtonActivated;
            protected SerializedProperty m_OnPrimaryAttachSecondaryButtonActivateCalls;

            protected SerializedProperty m_PrimaryAttachSecondaryButtonDeactivated;
            protected SerializedProperty m_OnPrimaryAttachSecondaryButtonDeactivateCalls;

            // secondary attach additional activate
            protected SerializedProperty m_SecondaryAttachActivated;
            protected SerializedProperty m_OnSecondaryAttachActivateCalls;

            protected SerializedProperty m_SecondaryAttachDeactivated;
            protected SerializedProperty m_OnSecondaryAttachDeactivateCalls;

            protected SerializedProperty m_SecondaryAttachPrimaryButtonActivated;
            protected SerializedProperty m_OnSecondaryAttachPrimaryButtonActivateCalls;

            protected SerializedProperty m_SecondaryAttachPrimaryButtonDeactivated;
            protected SerializedProperty m_OnSecondaryAttachPrimaryButtonDeactivateCalls;

            protected SerializedProperty m_SecondaryAttachSecondaryButtonActivated;
            protected SerializedProperty m_OnSecondaryAttachSecondaryButtonActivateCalls;

            protected SerializedProperty m_SecondaryAttachSecondaryButtonDeactivated;
            protected SerializedProperty m_OnSecondaryAttachSecondaryButtonDeactivateCalls;

            protected override void OnEnable()
            {
                base.OnEnable();

                primaryAttachInfo = serializedObject.FindProperty("primaryAttachInfo");
                secondaryAttachInfo = serializedObject.FindProperty("secondaryAttachInfo");

                // additional activate
                m_PrimaryButtonActivated = serializedObject.FindProperty("m_PrimaryButtonActivated");
                m_OnPrimaryButtonActivateCalls = m_PrimaryButtonActivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                m_PrimaryButtonDeactivated = serializedObject.FindProperty("m_PrimaryButtonDeactivated");
                m_OnPrimaryButtonDeactivateCalls = m_PrimaryButtonDeactivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                m_SecondaryButtonActivated = serializedObject.FindProperty("m_SecondaryButtonActivated");
                m_OnSecondaryButtonActivateCalls = m_SecondaryButtonActivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                m_SecondaryButtonDeactivated = serializedObject.FindProperty("m_SecondaryButtonDeactivated");
                m_OnSecondaryButtonDeactivateCalls = m_SecondaryButtonDeactivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                // primary attach additional activate
                m_PrimaryAttachActivated = serializedObject.FindProperty("m_PrimaryAttachActivated");
                m_OnPrimaryAttachActivateCalls = m_PrimaryAttachActivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                m_PrimaryAttachDeactivated = serializedObject.FindProperty("m_PrimaryAttachDeactivated");
                m_OnPrimaryAttachDeactivateCalls = m_PrimaryAttachDeactivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                m_PrimaryAttachPrimaryButtonActivated = serializedObject.FindProperty("m_PrimaryAttachPrimaryButtonActivated");
                m_OnPrimaryAttachPrimaryButtonActivateCalls = m_PrimaryAttachPrimaryButtonActivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                m_PrimaryAttachPrimaryButtonDeactivated = serializedObject.FindProperty("m_PrimaryAttachPrimaryButtonDeactivated");
                m_OnPrimaryAttachPrimaryButtonDeactivateCalls = m_PrimaryAttachPrimaryButtonDeactivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                m_PrimaryAttachSecondaryButtonActivated = serializedObject.FindProperty("m_PrimaryAttachSecondaryButtonActivated");
                m_OnPrimaryAttachSecondaryButtonActivateCalls = m_PrimaryAttachSecondaryButtonActivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                m_PrimaryAttachSecondaryButtonDeactivated = serializedObject.FindProperty("m_PrimaryAttachSecondaryButtonDeactivated");
                m_OnPrimaryAttachSecondaryButtonDeactivateCalls = m_PrimaryAttachSecondaryButtonDeactivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                // secondary attach additional activate
                m_SecondaryAttachActivated = serializedObject.FindProperty("m_SecondaryAttachActivated");
                m_OnSecondaryAttachActivateCalls = m_SecondaryAttachActivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                m_SecondaryAttachDeactivated = serializedObject.FindProperty("m_SecondaryAttachDeactivated");
                m_OnSecondaryAttachDeactivateCalls = m_SecondaryAttachDeactivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                m_SecondaryAttachPrimaryButtonActivated = serializedObject.FindProperty("m_SecondaryAttachPrimaryButtonActivated");
                m_OnSecondaryAttachPrimaryButtonActivateCalls = m_SecondaryAttachPrimaryButtonActivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                m_SecondaryAttachPrimaryButtonDeactivated = serializedObject.FindProperty("m_SecondaryAttachPrimaryButtonDeactivated");
                m_OnSecondaryAttachPrimaryButtonDeactivateCalls = m_SecondaryAttachPrimaryButtonDeactivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                m_SecondaryAttachSecondaryButtonActivated = serializedObject.FindProperty("m_SecondaryAttachSecondaryButtonActivated");
                m_OnSecondaryAttachSecondaryButtonActivateCalls = m_SecondaryAttachSecondaryButtonActivated.FindPropertyRelative("m_PersistentCalls.m_Calls");

                m_SecondaryAttachSecondaryButtonDeactivated = serializedObject.FindProperty("m_SecondaryAttachSecondaryButtonDeactivated");
                m_OnSecondaryAttachSecondaryButtonDeactivateCalls = m_SecondaryAttachSecondaryButtonDeactivated.FindPropertyRelative("m_PersistentCalls.m_Calls");
            }

            protected override void DrawInspector()
            {
                // DrawDefaultInspector();
                base.DrawInspector();

                var interactable = (XRBLSGrabInteractable)target;
                if (interactable == null) return;

                EditorGUILayout.Space();

                //EditorGUILayout.PropertyField(primaryAttachInfo, EditorGUIUtility.TrTextContent("Primary Attach Info"), true);
                //EditorGUILayout.PropertyField(secondaryAttachInfo, EditorGUIUtility.TrTextContent("Primary Attach Info"), true);

                EditorGUILayout.Space();

                m_PrimaryButtonActivated.isExpanded = EditorGUILayout.Foldout(m_PrimaryButtonActivated.isExpanded, EditorGUIUtility.TrTempContent("Additional Activate Events"), true);
                if (m_PrimaryButtonActivated.isExpanded)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        DrawAdditionalActivateEventsNested();
                    }
                }

                EditorGUILayout.Space();

                m_PrimaryAttachPrimaryButtonActivated.isExpanded = EditorGUILayout.Foldout(m_PrimaryAttachPrimaryButtonActivated.isExpanded, EditorGUIUtility.TrTempContent("Primary Attach Additional Activate Events"), true);
                if (m_PrimaryAttachPrimaryButtonActivated.isExpanded)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        DrawPrimaryAttachAdditionalActivateEventsNested();
                    }
                }

                EditorGUILayout.Space();

                m_SecondaryAttachPrimaryButtonActivated.isExpanded = EditorGUILayout.Foldout(m_SecondaryAttachPrimaryButtonActivated.isExpanded, EditorGUIUtility.TrTempContent("Secondary Attach Additional Activate Events"), true);
                if (m_SecondaryAttachPrimaryButtonActivated.isExpanded)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        DrawSecondaryAttachAdditionalActivateEventsNested();
                    }
                }
            }

            /// <summary>
            /// Draw the nested contents of the Interactable Events foldout.
            /// </summary>
            /// <seealso cref="DrawInteractableEvents"/>
            protected virtual void DrawAdditionalActivateEventsNested()
            {
                EditorGUILayout.LabelField("Activate", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_Activated);
                //if (m_OnActivateCalls.arraySize > 0 || m_OnActivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_Activated, EditorGUIUtility.TrTextContent("On Activated"));
                EditorGUILayout.PropertyField(m_Deactivated);
                //if (m_OnDeactivateCalls.arraySize > 0 || m_OnDeactivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_Deactivated, EditorGUIUtility.TrTextContent("On Deactivated"));

                EditorGUILayout.LabelField("Primary Button Activate", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_PrimaryButtonActivated);
                //if (m_OnPrimaryButtonActivateCalls.arraySize > 0 || m_OnPrimaryButtonActivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_PrimaryButtonActivated, EditorGUIUtility.TrTextContent("On Primary Button Activated"));
                EditorGUILayout.PropertyField(m_PrimaryButtonDeactivated);
                //if (m_OnPrimaryButtonDeactivateCalls.arraySize > 0 || m_OnPrimaryButtonDeactivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_PrimaryButtonDeactivated, EditorGUIUtility.TrTextContent("On Primary Button Deactivated"));

                EditorGUILayout.LabelField("Secondary Button Activate", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_SecondaryButtonActivated);
                //if (m_OnSecondaryButtonActivateCalls.arraySize > 0 || m_OnSecondaryButtonActivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_SecondaryButtonActivated, EditorGUIUtility.TrTextContent("On Secondary Button Activated"));
                EditorGUILayout.PropertyField(m_SecondaryButtonDeactivated);
                //if (m_OnSecondaryButtonDeactivateCalls.arraySize > 0 || m_OnSecondaryButtonDeactivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_SecondaryButtonDeactivated, EditorGUIUtility.TrTextContent("On Secondary Button Deactivated"));
            }

            protected virtual void DrawPrimaryAttachAdditionalActivateEventsNested()
            {
                EditorGUILayout.LabelField("Primary Attach Activate", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_PrimaryAttachActivated);
                //if (m_OnPrimaryAttachActivateCalls.arraySize > 0 || m_OnPrimaryAttachActivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_PrimaryAttachActivated, EditorGUIUtility.TrTextContent("On Primary Attach Activated"));
                EditorGUILayout.PropertyField(m_PrimaryAttachDeactivated);
                //if (m_OnPrimaryAttachDeactivateCalls.arraySize > 0 || m_OnPrimaryAttachDeactivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_PrimaryAttachDeactivated, EditorGUIUtility.TrTextContent("On Primary Attach Deactivated"));

                EditorGUILayout.LabelField("Primary Attach Primary Button Activate", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_PrimaryAttachPrimaryButtonActivated);
                //if (m_OnPrimaryAttachPrimaryButtonActivateCalls.arraySize > 0 || m_OnPrimaryAttachPrimaryButtonActivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_PrimaryAttachPrimaryButtonActivated, EditorGUIUtility.TrTextContent("On Primary Attach Primary Button Activated"));
                EditorGUILayout.PropertyField(m_PrimaryAttachPrimaryButtonDeactivated);
                //if (m_OnPrimaryAttachPrimaryButtonDeactivateCalls.arraySize > 0 || m_OnPrimaryAttachPrimaryButtonDeactivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_PrimaryAttachPrimaryButtonDeactivated, EditorGUIUtility.TrTextContent("On Primary Attach Primary Button Deactivated"));

                EditorGUILayout.LabelField("Primary Attach Secondary Button Activate", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_PrimaryAttachSecondaryButtonActivated);
                //if (m_OnPrimaryAttachSecondaryButtonActivateCalls.arraySize > 0 || m_OnPrimaryAttachSecondaryButtonActivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_PrimaryAttachSecondaryButtonActivated, EditorGUIUtility.TrTextContent("On Primary Attach Secondary Button Activated"));
                EditorGUILayout.PropertyField(m_PrimaryAttachSecondaryButtonDeactivated);
                //if (m_OnPrimaryAttachSecondaryButtonDeactivateCalls.arraySize > 0 || m_OnPrimaryAttachSecondaryButtonDeactivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_PrimaryAttachSecondaryButtonDeactivated, EditorGUIUtility.TrTextContent("On Primary Attach Secondary Button Deactivated"));
            }

            protected virtual void DrawSecondaryAttachAdditionalActivateEventsNested()
            {
                EditorGUILayout.LabelField("Secondary Attach Activate", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_SecondaryAttachActivated);
                //if (m_OnSecondaryAttachActivateCalls.arraySize > 0 || m_OnSecondaryAttachActivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_SecondaryAttachActivated, EditorGUIUtility.TrTextContent("On Secondary Attach Activated"));
                EditorGUILayout.PropertyField(m_SecondaryAttachDeactivated);
                //if (m_OnSecondaryAttachDeactivateCalls.arraySize > 0 || m_OnSecondaryAttachDeactivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_SecondaryAttachDeactivated, EditorGUIUtility.TrTextContent("On Secondary Attach Deactivated"));

                EditorGUILayout.LabelField("Secondary Attach Primary Button Activate", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_SecondaryAttachPrimaryButtonActivated);
                //if (m_OnSecondaryAttachPrimaryButtonActivateCalls.arraySize > 0 || m_OnSecondaryAttachPrimaryButtonActivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_SecondaryAttachPrimaryButtonActivated, EditorGUIUtility.TrTextContent("On Secondary Attach Primary Button Activated"));
                EditorGUILayout.PropertyField(m_SecondaryAttachPrimaryButtonDeactivated);
                //if (m_OnSecondaryAttachPrimaryButtonDeactivateCalls.arraySize > 0 || m_OnSecondaryAttachPrimaryButtonDeactivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_SecondaryAttachPrimaryButtonDeactivated, EditorGUIUtility.TrTextContent("On Secondary Attach Primary Button Deactivated"));

                EditorGUILayout.LabelField("Secondary Attach Secondary Button Activate", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_SecondaryAttachSecondaryButtonActivated);
                //if (m_OnSecondaryAttachSecondaryButtonActivateCalls.arraySize > 0 || m_OnSecondaryAttachSecondaryButtonActivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_SecondaryAttachSecondaryButtonActivated, EditorGUIUtility.TrTextContent("On Secondary Attach Secondary Button Activated"));
                EditorGUILayout.PropertyField(m_SecondaryAttachSecondaryButtonDeactivated);
                //if (m_OnSecondaryAttachSecondaryButtonDeactivateCalls.arraySize > 0 || m_OnSecondaryAttachSecondaryButtonDeactivateCalls.hasMultipleDifferentValues)
                //EditorGUILayout.PropertyField(m_SecondaryAttachSecondaryButtonDeactivated, EditorGUIUtility.TrTextContent("On Secondary Attach Secondary Button Deactivated"));
            }
        }
#endif
    }
}