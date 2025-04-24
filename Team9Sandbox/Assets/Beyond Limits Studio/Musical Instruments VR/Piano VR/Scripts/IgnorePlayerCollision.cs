using UnityEngine;

namespace BeyondLimitsStudios
{
    namespace VRInteractables
    {
        namespace MusicalInstruments
        {
            public class IgnorePlayerCollision : MonoBehaviour
            {
                public bool ignorePlayer = true;
                public string overridePlayerLayer = "";

                private void Start()
                {
                    if (!ignorePlayer)
                        return;

                    int playerLayer = LayerMask.NameToLayer(!string.IsNullOrEmpty(overridePlayerLayer) ? overridePlayerLayer : "Player");

                    if (playerLayer < 0)
                    {
                        Debug.LogWarning($"No player layer found! Player collisions with non static pianos will not be ignored which is not recommended!");
                        return;
                    }

                    if (ignorePlayer)
                    {
                        foreach (var rb in this.GetComponentsInChildren<Rigidbody>())
                        {
                            rb.excludeLayers = rb.excludeLayers |= (1 << playerLayer);
                        }
                    }
                }
            }
        }
    }
}