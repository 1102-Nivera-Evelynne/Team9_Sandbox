using UnityEngine;
using System.Collections;

public class PianoParticle : MonoBehaviour
{
    // Existing fields
    public GameObject[] pianoKeys;
    public float resetDelay = 0.3f;

    // ➕ Add this
    [Header("Assign your particle prefab here")]
    public ParticleSystem particlePrefab;

    private Color[] originalColors;

    private void Start()
    {
        originalColors = new Color[pianoKeys.Length];

        for (int i = 0; i < pianoKeys.Length; i++)
        {
            Renderer rend = pianoKeys[i].GetComponent<Renderer>();
            if (rend != null)
                originalColors[i] = rend.material.color;
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
            float hue = (float)keyIndex / pianoKeys.Length;
            int octave = keyIndex / 12;
            float alpha = Mathf.Lerp(0.3f, 1.0f, Mathf.Clamp01((float)octave / 7f));

            Color dynamicColor = Color.HSVToRGB(hue, 1f, 1f);
            dynamicColor.a = alpha;

            Material tempMat = new Material(renderer.material);
            tempMat.color = dynamicColor;
            tempMat.SetFloat("_Mode", 3);
            tempMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            tempMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            tempMat.SetInt("_ZWrite", 0);
            tempMat.DisableKeyword("_ALPHATEST_ON");
            tempMat.EnableKeyword("_ALPHABLEND_ON");
            tempMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            tempMat.renderQueue = 3000;

            renderer.material = tempMat;

            // ➕ Spawn particles with the same color
            SpawnParticleEffect(pianoKeys[keyIndex].transform.position, dynamicColor);

            StartCoroutine(ResetKeyColorAfterDelay(keyIndex, resetDelay));
        }
    }

    // ➕ Particle Spawning Method
    private void SpawnParticleEffect(Vector3 position, Color color)
    {
        if (particlePrefab == null) return;

        ParticleSystem ps = Instantiate(particlePrefab, position + Vector3.up * 0.1f, Quaternion.identity);
        var main = ps.main;
        main.startColor = color;

        ps.Play();
        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
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
            renderer.material = tempMat;
        }
    }
}

