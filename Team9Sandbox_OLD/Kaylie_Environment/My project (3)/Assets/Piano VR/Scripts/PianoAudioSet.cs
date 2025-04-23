using System.Collections.Generic;
using UnityEngine;
using static BeyondLimitsStudios.VRInteractables.MusicalInstruments.PianoVR;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BeyondLimitsStudios
{
    namespace VRInteractables
    {
        namespace MusicalInstruments
        {
            [CreateAssetMenu(fileName = "New Piano Audio Set", menuName = "Interactive Piano VR/Piano Audio Set")]
            public class PianoAudioSet : ScriptableObject
            {
                [Tooltip("List of Audio Clips to use by a piano when automatically creating pianoAudios. The audio clips names need to follow \"NoteOctave\" pattern in order to work. \"A#4\" or \"B6\" for example.")]
                public List<AudioClip> audioClips = new List<AudioClip>();

                [Tooltip("List of Note-AudioClip pairs used by a piano.")]
                public List<PianoAudio> pianoAudios = new List<PianoAudio>();

                /// <summary>
                /// This function automatically creates PianoAudios from audioClips list. The audio clips names need to follow \"NoteOctave\" pattern in order to work. \"A#4\" or \"B6\" for example."
                /// </summary>
                public void Process()
                {
                    pianoAudios.Clear();

                    foreach (var clip in audioClips)
                    {
                        Note note;
                        if (Note.TryParse(clip.name, out note))
                        {
                            PianoAudio pianoAudio;
                            pianoAudio.note = note;
                            pianoAudio.audioClip = clip;
                            pianoAudios.Add(pianoAudio);
                        }
                    }
                }
            }

            [System.Serializable]
            public struct PianoAudio
            {
                public Note note;
                public AudioClip audioClip;
            }

#if UNITY_EDITOR
            [CustomEditor(typeof(PianoAudioSet))]
            class PianoAudioSetEditor : Editor
            {
                public override void OnInspectorGUI()
                {
                    var pianoAudioSet = (PianoAudioSet)target;
                    if (pianoAudioSet == null) return;

                    GUIContent content = new GUIContent();

                    content.text = "Process";
                    content.tooltip = "Click this button to automatically create PianoAudios (Note-AudioClip pairs). This pairs are used by pianos. The function uses audioClips list. The audio clips names need to follow \"NoteOctave\" pattern in order to work. \"A#4\" or \"B6\" for example.";

                    if (GUILayout.Button(content))
                    {
                        pianoAudioSet.Process();
                    }

                    DrawDefaultInspector();
                }
            }
#endif
        }
    }
}