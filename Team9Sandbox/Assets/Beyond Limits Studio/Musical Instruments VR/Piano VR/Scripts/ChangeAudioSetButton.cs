using System.Linq;
using UnityEngine;

namespace BeyondLimitsStudios
{
    namespace VRInteractables
    {
        namespace MusicalInstruments
        {
            public class ChangeAudioSetButton : MonoBehaviour
            {
                [SerializeField]
                [Tooltip("You can ignore some colliders so they cannot press the button if you need.")]
                private Collider[] ignoreColliders;

                [SerializeField]
                [Tooltip("Button model transform. It will move down a bit when the button is pressed.")]
                private Transform buttonModel;

                [SerializeField]
                [Tooltip("Audio set to set on a piano when the button is pressed.")]
                private PianoAudioSet audioSet;
                [SerializeField]
                [Tooltip("Piano which you assign the audioSet to.")]
                private PianoVR piano;

                [Tooltip("Button \"click\" Audio Source.")]
                private AudioSource audioSource;

                /// <summary>
                /// Get "Click" Audio Source.
                /// </summary>
                private void Awake()
                {
                    // Get "click" Audio Source
                    audioSource = GetComponent<AudioSource>();
                }

                /// <summary>
                /// This function is called by unity when collider "other" hits this GameObject's collider.
                /// </summary>
                /// <param name="other">The collider that hit this GameObject.</param>
                private void OnTriggerEnter(Collider other)
                {
                    // Trigger colliders cannot click the button
                    if (other.isTrigger)
                        return;

                    // You can ignore some colliders if you need
                    if (ignoreColliders.Contains(other))
                        return;

                    // Move button model a little bit when it's pressed.
                    buttonModel.localPosition = new Vector3(0f, -0.005f, 0f);

                    // Set the audio set on the piano.
                    piano.ChangeAudioSet(audioSet);

                    // Play "click" sound.
                    audioSource.Play();
                }

                /// <summary>
                /// This function is called by unity when collider "other" stops overlaping GameObject's collider.
                /// </summary>
                /// <param name="other">The collider that hit this GameObject.</param>
                private void OnTriggerExit(Collider other)
                {
                    // Trigger colliders cannot click the button
                    if (other.isTrigger)
                        return;

                    // You can ignore some colliders if you need
                    if (ignoreColliders.Contains(other))
                        return;

                    // Reset button model position.
                    buttonModel.localPosition = new Vector3(0f, 0f, 0f);

                    // Play "click" sound.
                    audioSource.Play();
                }
            }
        }
    }
}