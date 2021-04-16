using UnityEngine;
using UnityEngine.Events;

public class BeatListener : MonoBehaviour
{
    public bool detectAutomatically = true;
    public AudioSource audioSource;
    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;

    public float clipLoudness;
    private float[] clipSampleData;

    public float sizeFactor = 1;
    public float time = 0;
    public float threshold = 0.25f;

    public UnityEvent onDetectBeat;
    private float lastBeatTime = 0;

    // Use this for initialization
    private void Awake()
    {
        clipSampleData = new float[sampleDataLength];
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;
        currentUpdateTime += Time.deltaTime;
        if (!detectAutomatically)
        {
            return;
        }

        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            audioSource.clip.GetData(clipSampleData,
                audioSource
                    .timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }

            clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for

            clipLoudness *= sizeFactor;
            clipLoudness = Mathf.Clamp(clipLoudness, 0, Mathf.Infinity);
            if (clipLoudness > threshold)
            {
                InvokeDetectBeat();
            }
        }
    }

    public void InvokeDetectBeat()
    {
        onDetectBeat?.Invoke();
    }
}