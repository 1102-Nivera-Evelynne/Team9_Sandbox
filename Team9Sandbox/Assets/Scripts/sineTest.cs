using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SineWaveSound : MonoBehaviour
{
    private AudioSource audioSource;
    public Transform player;

    private float baseFrequency = 50.0f;  
    private float maxFrequency = 500.0f; 
    private float minDistance = 1.0f;  
    private float maxDistance = 20.0f; 
    private float sampleRate = 48000f;

    private float currentFrequency; 

    private double phase; // Continuous phase accumulator
    private double phaseIncrement; // How much phase increases per sample
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 1.0f;
        audioSource.Play();
        currentFrequency = baseFrequency;
        phaseIncrement = (2.0 * Mathf.PI * currentFrequency) / sampleRate;
    }

    void Update()
    {
        // Distance between the player and the music source. Distance is built in Unity function. 
        float distance = Vector3.Distance(transform.position, player.position);
        // Keeps the distance value between 0 and 1. 
        float normalizedDistance = Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
        //Smooth transition between frequencies. 
        float targetFrequency = Mathf.Lerp(maxFrequency, baseFrequency, normalizedDistance);

        // Smoothly interpolate frequency
        currentFrequency = Mathf.Lerp(currentFrequency, targetFrequency, Time.deltaTime * 3);
        //TBh i do not understand the phase stuff 
        phaseIncrement = (2.0 * Mathf.PI * currentFrequency) / sampleRate; // Update phase increment
    }

    //Builtin Unity function to generate audio. https://docs.unity3d.com/6000.0/Documentation/ScriptReference/MonoBehaviour.OnAudioFilterRead.html
    void OnAudioFilterRead(float[] data, int channels)
    {
        double localPhase = phase; // Store current phase to avoid thread issues
        double localPhaseIncrement = phaseIncrement; // Use a stable phase increment value

        //Audio samples per channel I got this from the documentation above 
        int dataLen = data.Length / channels;

        int n = 0;
        //Iterate through all the samples 
        while (n < dataLen)
        {
            //Basic sin wave. There are other kinds of waves but I do not know how to do them off the top of my head. 
            float sample = Mathf.Sin((float)localPhase);

            //Assins the sample to the channel so that it will play. 
            data[n*channels] = sample;

            //More phase stuff here are the comments chat gave me
            // Increase the phase to progress through the sine wave
            localPhase += localPhaseIncrement;

            // Keep phase within range (prevent overflow)
            if (localPhase > 2.0 * Mathf.PI)
                localPhase -= 2.0 * Mathf.PI;

            n++;
        }
        // Store phase for next frame
        phase = localPhase; 
    }
}

//This sound sucks and this can definitely be optimized but it was a relatively quick proof of concept for distance based sound generation. 
//Right now it only uses left channel so we would probably want to have it uitilize both with another loop. 
//I do not entirely understand the phase stuff but originally the sound had very bad crackling and this is how chat GPT said ot fix it. 
//If we go forward with making our own theremin, I can look into it more or assist whoever takes over the audio generation. 