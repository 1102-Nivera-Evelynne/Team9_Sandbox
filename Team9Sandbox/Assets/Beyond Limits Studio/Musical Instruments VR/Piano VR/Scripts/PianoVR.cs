using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BeyondLimitsStudios
{
    namespace VRInteractables
    {
        namespace MusicalInstruments
        {
            public class PianoVR : MonoBehaviour
            {
                [Tooltip("Event invoked when a key is pressed. Passes note as a argument.")]
                public UnityEvent<Note> onKeyPressed;
                [Tooltip("Event invoked when a key is released. Passes note as a argument.")]
                public UnityEvent<Note> onKeyReleased;

                [SerializeField]
                [Tooltip("Main rigidbody of the piano. Keys need to be connected to this key.")]
                private Rigidbody mainRigidbody;

                [SerializeField]
                [Tooltip("Piano colliders array. Used for ignoring collisions with keys.")]
                private Collider[] pianoColliders = new Collider[0];
                [SerializeField]
                [Tooltip("Piano keys array.")]
                private PianoKey[] pianoKeys = new PianoKey[0];

                [SerializeField]
                [Tooltip("Audio Set of this piano.")]
                private PianoAudioSet audioSet;

                [SerializeField]
                [Tooltip("All piano Audio Sources are parented to this GameObject.")]
                private GameObject audioSourcesHolder;

                [SerializeField]
                [Tooltip("Stores key information. AudioSources references, note, key.")]
                private List<PianoKeyInfo> pianoKeyInfos = new List<PianoKeyInfo>();

                [Tooltip("Time to mute piano audio after key is released.")]
                public float sustain = 1f;

                [Tooltip("If false the piano can't play any sounds.")]
                public bool isPianoEnabled = true;

                private void Awake()
                {
                    if (pianoKeys == null || pianoKeys.Length == 0)
                        pianoKeys = this.GetComponentsInChildren<PianoKey>();

                    if (pianoColliders == null || pianoColliders.Length == 0)
                        pianoColliders = this.GetComponentsInChildren<Collider>();

                    // Ignore key collisions with piano and other keys
                    foreach (var key in pianoKeys)
                    {
                        key.SetPiano(this);

                        var keyCol = key.GetComponentInChildren<Collider>();

                        if (keyCol != null)
                            foreach (var col in pianoColliders)
                                Physics.IgnoreCollision(keyCol, col, true);
                    }
                }

                /// <summary>
                /// Enables piano.
                /// </summary>
                public void EnablePiano()
                {
                    isPianoEnabled = true;
                }

                /// <summary>
                /// Disables piano.
                /// </summary>
                public void DisablePiano()
                {
                    isPianoEnabled = false;
                }

                /// <summary>
                /// Gets this piano colliders.
                /// </summary>
                public void GetColliders()
                {
                    pianoColliders = this.GetComponentsInChildren<Collider>();
                }

                /// <summary>
                /// Changes audio set of this piano and updates audio clips on all audio sources.
                /// </summary>
                /// <param name="set">Piano Audio Set to use.</param>
                public void ChangeAudioSet(PianoAudioSet set)
                {
                    audioSet = set;
                    UpdateAudioClips();
                }

                /// <summary>
                /// Updates audio clips on all audio sources.
                /// </summary>
                public void UpdateAudioClips()
                {
                    foreach (var audio in audioSet.pianoAudios)
                    {
                        int index = pianoKeyInfos.FindIndex(k => k.note == audio.note);

                        if (index == -1)
                            continue;

                        pianoKeyInfos[index].primaryAudioSource.clip = audio.audioClip;
                        pianoKeyInfos[index].secondaryAudioSource.clip = audio.audioClip;
                    }
                }

                /// <summary>
                /// Gets keys and connects the ket to mainRigidbody.
                /// </summary>
                public void GetKeys()
                {
                    pianoKeys = GetComponentsInChildren<PianoKey>();

                    if (mainRigidbody == null)
                        mainRigidbody = this.GetComponentInChildren<Rigidbody>();

                    if (mainRigidbody == null)
                        return;

                    foreach (var key in pianoKeys)
                    {
                        var joint = key.GetComponent<ConfigurableJoint>();
                        joint.connectedBody = mainRigidbody;
                    }
                }

                /// <summary>
                /// Sets up all audio sources.
                /// </summary>
                public void AssignAudioSources()
                {
                    pianoKeyInfos.Clear();

                    if (pianoKeys.Length != audioSet.pianoAudios.Count)
                        Debug.LogWarning("There should be as much audio clips in audioClips array as pianoKeys!");

                    List<AudioSource> audioSources = CreateAudioSources();

                    Dictionary<Note, AudioClip> audioClipsToNotes = new Dictionary<Note, AudioClip>();

                    foreach (var pianoAudio in audioSet.pianoAudios)
                    {
                        audioClipsToNotes.Add(pianoAudio.note, pianoAudio.audioClip);
                    }

                    int lastAudioSource = 0;

                    foreach (PianoKey pianoKey in pianoKeys)
                    {
                        Note note;
                        if (Note.TryParse(pianoKey.gameObject.name, out note))
                        {
                            if (audioClipsToNotes.ContainsKey(note))
                            {
                                PianoKeyInfo pianoKeyInfo = new PianoKeyInfo();
                                pianoKeyInfo.note = note;
                                pianoKeyInfo.attachedKey = pianoKey;
                                pianoKeyInfo.primaryAudioSource = audioSources[lastAudioSource];
                                pianoKeyInfo.secondaryAudioSource = audioSources[lastAudioSource+1];
                                pianoKeyInfo.primaryAudioSource.clip = audioClipsToNotes[note];
                                pianoKeyInfo.secondaryAudioSource.clip = audioClipsToNotes[note];
                                pianoKeyInfo.attachedKey.SetAudioSources(pianoKeyInfo.primaryAudioSource, pianoKeyInfo.secondaryAudioSource);
                                pianoKeyInfo.attachedKey.SetNote(note);
                                pianoKeyInfos.Add(pianoKeyInfo);
                                lastAudioSource += 2;
                            }
                            else
                            {
                                Debug.LogWarning($"Couldn't find AudioClip for piano key {pianoKey}!");
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"Couldn't parse audio clip {pianoKey.gameObject.name}! Make sure all PianoKey gameObjects are named correctly to use \"BasedOnNames\" option!");
                        }
                    }
                }

                /// <summary>
                /// Creates audio sources if needed.
                /// </summary>
                private List<AudioSource> CreateAudioSources()
                {
                    if (audioSourcesHolder == null)
                    {
                        audioSourcesHolder = new GameObject();
                        audioSourcesHolder.name = "AudioSourcesHolder";
                        audioSourcesHolder.transform.parent = this.transform;
                        audioSourcesHolder.transform.position = this.transform.position;
                    }

                    List<AudioSource> result = audioSourcesHolder.GetComponentsInChildren<AudioSource>().ToList();

                    if (result == null)
                        result = new List<AudioSource>();

                    int x = result.Count;

                    for (int i = 0; i < Mathf.Min(pianoKeys.Length, audioSet.pianoAudios.Count) * 2 - x; i+=2)
                    {
                        GameObject audioSourceGo = new GameObject();
                        audioSourceGo.transform.parent = audioSourcesHolder.transform;
                        AudioSource audioSource = audioSourceGo.AddComponent<AudioSource>();
                        audioSource.spatialBlend = 1f;
                        result.Add(audioSource);
                        audioSource = audioSourceGo.AddComponent<AudioSource>();
                        audioSource.spatialBlend = 1f;
                        result.Add(audioSource);
                    }

                    foreach (var item in result)
                    {
                        item.playOnAwake = false;
                    }

                    return result;
                }

                [System.Serializable]
                public struct PianoKeyInfo
                {
                    public Note note;
                    public PianoKey attachedKey;
                    public AudioSource primaryAudioSource;
                    public AudioSource secondaryAudioSource;
                }

                [System.Serializable]
                public struct Note
                {
                    // Properties to store the note and octave
                    public MusicalNote NoteName;
                    public int Octave;

                    // Constructor to initialize the Note struct
                    public Note(MusicalNote note, int octave)
                    {
                        NoteName = note;
                        Octave = octave;
                    }

                    // Override ToString method for better readability
                    public override string ToString()
                    {
                        return $"{NoteName}{Octave}";
                    }

                    public static bool TryParse(string noteString, out Note result)
                    {
                        if (string.IsNullOrEmpty(noteString))
                        {
                            result = default(Note);
                            return false;
                        }

                        noteString = noteString.Replace("b", "Flat");

                        // Normalize input string to lowercase for case-insensitivity
                        noteString = noteString.ToLower();

                        // Replace various representations of sharp and flat symbols
                        noteString = noteString.Replace("#", "Sharp").Replace("sharp", "Sharp").Replace("♭", "Flat").Replace("flat", "Flat");

                        // Split the string into note and octave parts
                        int octaveLength = 0;

                        for (int i = noteString.Length - 1; i >= 0; i--)
                        {
                            if (!char.IsNumber(noteString[i]))
                                break;

                            octaveLength++;
                        }

                        string notePart = noteString.Substring(0, noteString.Length - octaveLength);
                        string octavePart = noteString.Substring(noteString.Length - octaveLength);

                        // Parse note and octave
                        if (Enum.TryParse<MusicalNote>(notePart, ignoreCase: true, out MusicalNote note) && int.TryParse(octavePart, out int octave))
                        {
                            result = new Note(note, octave);
                            return true;
                        }
                        else
                        {
                            result = default(Note);
                            return false;
                        }
                    }

                    public static Note Parse(string noteString)
                    {
                        if (string.IsNullOrEmpty(noteString))
                        {
                            throw new ArgumentException("Invalid note string");
                        }

                        noteString = noteString.Replace("b", "Flat");

                        // Normalize input string to lowercase for case-insensitivity
                        noteString = noteString.ToLower();

                        // Replace various representations of sharp and flat symbols
                        noteString = noteString.Replace("#", "Sharp").Replace("sharp", "Sharp").Replace("♭", "Flat").Replace("flat", "Flat");

                        // Split the string into note and octave parts
                        int octaveLength = 0;

                        for (int i = noteString.Length - 1; i >= 0; i--)
                        {
                            if (!char.IsNumber(noteString[i]))
                                break;

                            octaveLength++;
                        }

                        string notePart = noteString.Substring(0, noteString.Length - octaveLength);
                        string octavePart = noteString.Substring(noteString.Length - octaveLength);

                        // Parse note and octave
                        if (Enum.TryParse<MusicalNote>(notePart, ignoreCase: true, out MusicalNote note) && int.TryParse(octavePart, out int octave))
                        {
                            return new Note(note, octave);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid note string format");
                        }

                    }

                    // Override == and != operators to compare Note instances
                    public static bool operator ==(Note note1, Note note2)
                    {
                        return note1.Equals(note2);
                    }

                    public static bool operator !=(Note note1, Note note2)
                    {
                        return !note1.Equals(note2);
                    }

                    // Override Equals method to provide custom comparison logic
                    public override bool Equals(object obj)
                    {
                        if (obj is Note otherNote)
                        {
                            return NoteName == otherNote.NoteName && Octave == otherNote.Octave;
                        }
                        return false;
                    }

                    public override int GetHashCode() 
                    {
                        return base.GetHashCode();
                    }
                }

                // Enum to represent musical notes
                [System.Serializable]
                public enum MusicalNote
                {
                    C = 0,
                    CSharp = 1,
                    DFlat = CSharp,
                    D = 2,
                    DSharp = 3,
                    EFlat = DSharp,
                    E = 4,
                    F = 5,
                    FSharp = 6,
                    GFlat = FSharp,
                    G = 7,
                    GSharp = 8,
                    AFlat = GSharp,
                    A = 9,
                    ASharp = 10,
                    BFlat = ASharp,
                    B = 11
                }
            }

#if UNITY_EDITOR
            [CustomEditor(typeof(PianoVR)), CanEditMultipleObjects]
            class PianoVREditor : Editor
            {
                public override void OnInspectorGUI()
                {
                    var pianoVR = (PianoVR)target;
                    if (pianoVR == null) return;

                    if (GUILayout.Button("Update Audio Clips"))
                    {
                        pianoVR.UpdateAudioClips();
                    }

                    if (GUILayout.Button("Get Colliders"))
                    {
                        pianoVR.GetColliders();
                    }

                    // Using this method might break something. Use Piano Base prefabs since they are already set up for use.
                    //if (GUILayout.Button("Assign Audio Sources"))
                    //{
                    //    pianoVR.AssignAudioSources();
                    //}

                    // Using this method might break something. Use Piano Base prefabs since they are already set up for use.
                    //if (GUILayout.Button("Get Keys"))
                    //{
                    //    pianoVR.GetKeys();
                    //}

                    DrawDefaultInspector();
                }
            }
#endif
        }
    }
}       
