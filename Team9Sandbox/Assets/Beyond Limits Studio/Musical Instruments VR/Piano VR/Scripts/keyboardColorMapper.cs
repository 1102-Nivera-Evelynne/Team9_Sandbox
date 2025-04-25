using System.Collections;
using UnityEngine;

public class KeyboardColorMapper : MonoBehaviour
{
    [Header("Assign all piano key GameObjects here (88 keys total)")]
    public GameObject[] pianoKeys;

    [Header("Delay before key color resets")]
    public float resetDelay = 0.3f;

    private Color[] originalColors;

    private void Start()
    {
        // Cache original key colors
        originalColors = new Color[pianoKeys.Length];

        for (int i = 0; i < pianoKeys.Length; i++)
        {
            Renderer rend = pianoKeys[i].GetComponent<Renderer>();
            if (rend != null)
            {
                originalColors[i] = rend.material.color;
            }
        }
    }

    public void HighlightKey(int keyIndex)
    {
        if (keyIndex < 0 || keyIndex >= pianoKeys.Length)
        {
            Debug.LogWarning("Key index out of range.");
            return;
        }

        Renderer renderer = pianoKeys[keyIndex].GetComponent<Renderer>();
        if (renderer != null)
        {
            Debug.Log("Key Pressed: " + keyIndex);
            // Hue: spread across all 88 keys
            float hue = (float)keyIndex / pianoKeys.Length;

            // Octave: each 12 keys is roughly an octave
            int octave = keyIndex / 12;

            // Alpha: from 0.3 (low) to 1.0 (high)
            float alpha = Mathf.Lerp(0.3f, 1.0f, Mathf.Clamp01((float)octave / 7f));

            // HSV to RGB color
            Color dynamicColor = Color.HSVToRGB(hue, 1f, 1f);
            dynamicColor.a = alpha;

            // Create and assign a transparent material
            Material tempMat = new Material(renderer.material);
            tempMat.color = dynamicColor;

            // Enable transparency on material
            tempMat.SetFloat("_Mode", 3);
            tempMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            tempMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            tempMat.SetInt("_ZWrite", 0);
            tempMat.DisableKeyword("_ALPHATEST_ON");
            tempMat.EnableKeyword("_ALPHABLEND_ON");
            tempMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            tempMat.renderQueue = 3000;

            renderer.material = tempMat;

            // Reset color after delay
            StartCoroutine(ResetKeyColorAfterDelay(keyIndex, resetDelay));
        }
    }

    private IEnumerator ResetKeyColorAfterDelay(int keyIndex, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (keyIndex < 0 || keyIndex >= pianoKeys.Length)
            yield break;

        Renderer renderer = pianoKeys[keyIndex].GetComponent<Renderer>();
        if (renderer != null)
        {
            Material tempMat = new Material(renderer.material);
            tempMat.color = originalColors[keyIndex];

            // Optional: revert material settings if needed
            renderer.material = tempMat;
        }
    }
}
