using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static BeyondLimitsStudios.VRInteractables.MusicalInstruments.PianoVR;

namespace BeyondLimitsStudios
{
    namespace VRInteractables
    {
        namespace MusicalInstruments
        {
            public class PianoKey : MonoBehaviour
            {
                [Tooltip("Even invoked when the key is pressed.")]
                public UnityEvent onKeyPressed;
                [Tooltip("Even invoked when the key is released.")]
                public UnityEvent onKeyReleased;

                [SerializeField]
                [Tooltip("Primary audio note audio source. Two audio sources are used to prevent some weird audio issues.")]
                private AudioSource primaryAudioSource;
                [SerializeField]
                [Tooltip("Secondary audio note audio source. Two audio sources are used to prevent some weird audio issues.")]
                private AudioSource secondaryAudioSource;

                [Tooltip("Coroutine used to mute primary audio source reference.")]
                private Coroutine primaryAudioCoroutine;
                [Tooltip("Coroutine used to mute secondary audio source reference.")]
                private Coroutine secondaryAudioCoroutine;

                [Tooltip("Used to determine which audio source to use.")]
                private bool playSecond = false;

                [SerializeField]
                [Tooltip("Angle at which audio is played.")]
                private float activationAngle = 2f;
                [SerializeField]
                [Tooltip("Angle at which audio is muted.")]
                private float deactivationAngle = 1f;
                [SerializeField]
                [Tooltip("True if key is pressed. False otherwise.")]
                private bool isActivated = false;

                [Tooltip("Default volume.")]
                private float volume = 1f;

                [Tooltip("Piano the key is connected to.")]
                private PianoVR connectedPiano;
                [Tooltip("Note this key plays.")]
                private Note note;

                private void Awake()
                {
                    onKeyPressed.AddListener(PlaySound);
                    onKeyReleased.AddListener(MuteSound);

                    if (primaryAudioSource != null)
                        volume = primaryAudioSource.volume;
                }

                private void Update()
                {
                    // Current angle of the key
                    float angle = this.transform.localEulerAngles.x;

                    if (angle > 180)
                        angle -= 360f;

                    if (!isActivated)
                    {
                        if (!connectedPiano.isPianoEnabled)
                            return;

                        if (angle >= activationAngle)
                        {
                            isActivated = true;
                            onKeyPressed?.Invoke();
                            connectedPiano.onKeyPressed?.Invoke(note);
                        }
                    }
                    else
                    {
                        if (angle < deactivationAngle)
                        {
                            isActivated = false;
                            onKeyReleased?.Invoke();
                            connectedPiano.onKeyReleased?.Invoke(note);
                        }
                    }
                }

                /// <summary>
                /// Detect when a finger touches the key.
                /// </summary>
                private void OnTriggerEnter(Collider other)
                {
                    if (other.CompareTag("Finger") && !isActivated) // Ensure it's a finger and not already activated
                    {
                        isActivated = true;
                        onKeyPressed?.Invoke();
                        connectedPiano?.onKeyPressed?.Invoke(note);
                    }
                }

                /// <summary>
                /// Detect when a finger leaves the key.
                /// </summary>
                private void OnTriggerExit(Collider other)
                {
                    if (other.CompareTag("Finger") && isActivated) // Ensure it's a finger leaving
                    {
                        isActivated = false;
                        onKeyReleased?.Invoke();
                        connectedPiano?.onKeyReleased?.Invoke(note);
                    }
                }

                /// <summary>
                /// This function is called from PianoVR and is used to assign primary and secondary audio sources.
                /// </summary>
                /// <param name="primaryAudioSource">First Audio Source.</param>
                /// <param name="secondaryAudioSource">Second Audio Source.</param>
                public void SetAudioSources(AudioSource primaryAudioSource, AudioSource secondaryAudioSource)
                {
                    this.primaryAudioSource = primaryAudioSource;
                    this.secondaryAudioSource = secondaryAudioSource;
                    volume = primaryAudioSource.volume;
                    primaryAudioSource.gameObject.name = this.gameObject.name;
                }

                /// <summary>
                /// This function is called from PianoVR and is used to assign piano.
                /// </summary>
                /// <param name="piano">Piano.</param>
                public void SetPiano(PianoVR piano)
                {
                    connectedPiano = piano;
                }

                /// <summary>
                /// This function is called from PianoVR and is used to assign note of this key.
                /// </summary>
                /// <param name="note">Note of this key.</param>
                public void SetNote(Note note)
                {
                    this.note = note;
                }

                /// <summary>
                /// Plays audio source.
                /// </summary>
                public void PlaySound()
                {
                    if (!playSecond)
                    {
                        if (primaryAudioCoroutine != null)
                            StopCoroutine(primaryAudioCoroutine);

                        primaryAudioCoroutine = StartCoroutine(MuteAudioSource(primaryAudioSource));
                    }
                    else
                    {
                        if (secondaryAudioCoroutine != null)
                            StopCoroutine(secondaryAudioCoroutine);

                        secondaryAudioCoroutine = StartCoroutine(MuteAudioSource(secondaryAudioSource));
                    }

                    playSecond = !playSecond;

                    if (!playSecond)
                    {
                        if (primaryAudioSource == null)
                            return;

                        if (primaryAudioCoroutine != null)
                            StopCoroutine(primaryAudioCoroutine);

                        primaryAudioSource.volume = volume;
                        primaryAudioSource.Play();
                    }
                    else
                    {
                        if (secondaryAudioSource == null)
                            return;

                        if (secondaryAudioCoroutine != null)
                            StopCoroutine(secondaryAudioCoroutine);

                        secondaryAudioSource.volume = volume;
                        secondaryAudioSource.Play();
                    }
                }

                /// <summary>
                /// Mutes audio source.
                /// </summary>
                public void MuteSound()
                {
                    if (!playSecond)
                    {
                        if (primaryAudioCoroutine != null)
                            StopCoroutine(primaryAudioCoroutine);

                        primaryAudioCoroutine = StartCoroutine(MuteAudioSource(primaryAudioSource));
                    }
                    else
                    {
                        if (secondaryAudioCoroutine != null)
                            StopCoroutine(secondaryAudioCoroutine);

                        secondaryAudioCoroutine = StartCoroutine(MuteAudioSource(secondaryAudioSource));
                    }
                }

                private IEnumerator MuteAudioSource(AudioSource audioSource, bool forceQuick = false)
                {
                    float sustain = connectedPiano != null ? connectedPiano.sustain : 1f;

                    if (forceQuick)
                        sustain = 0.2f;

                    float maxVolume = audioSource.volume;
                    float proportion = audioSource.volume / sustain;

                    while (sustain > 0f)
                    {
                        sustain -= Time.deltaTime;
                        audioSource.volume = sustain * proportion;
                        yield return null;
                    }
                }
            }
        }
    }
}