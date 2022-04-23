using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    private AudioSource audioSource;
    public static float[] samples = new float[512];
    private static float[] frequencyBand = new float[8];
    private static float[] bandBuffer = new float[8];

    private float[] bufferDecrease = new float[8];
    public float initialBufferDecrease = 0.005f;
    public float bufferGravity = 1.2f;

    private float[] frequencyBandMax = new float[8];
    public static float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
    }

    private void CreateAudioBands()
    {
        for (int i = 0; i < audioBand.Length; i++)
        {
            if (frequencyBand[i] > frequencyBandMax[i])
            {
                frequencyBandMax[i] = frequencyBand[i];
            }

            audioBand[i] = frequencyBand[i] / frequencyBandMax[i];
            audioBandBuffer[i] = bandBuffer[i] / frequencyBandMax[i];
        }
    }

    private void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    private void MakeFrequencyBands()
    {
        // 20 - 60 hertz
        // 60 - 250 hertz
        // 250 - 500 hertz
        // 500 - 2000 hertz
        // 2000 - 4000 hertz
        // 4000 - 6000 hertz
        // 6000 - 20000 hertz

        // 22050 hertz / 512 samples = 43 hertz/sample
        // band -  samples =   frequency = frequency range
        // 0 -   2 samples =    86 hertz = 0     -    86 hertz
        // 1 -   4 samples =   172 hertz = 87    -   258 hertz
        // 2 -   8 samples =   344 hertz = 259   -   602 hertz
        // 3 -  16 samples =   688 hertz = 603   -  1290 hertz
        // 4 -  32 samples =  1376 hertz = 1291  -  2666 hertz
        // 5 -  64 samples =  2752 hertz = 2667  -  5418 hertz
        // 6 - 128 samples =  5504 hertz = 5419  - 10922 hertz
        // 7 - 256 samples = 11008 hertz = 10923 - 21930 hertz
        int count = 0;
        int extra = samples.Length;

        for (int i = 0; i < frequencyBand.Length; i++)
        {
            int sampleCount = (int)Mathf.Pow(2, i + 1);
            float average = 0.0f;

            if (i == frequencyBand.Length)
            {
                sampleCount += extra;
            }
            else
            {
                extra -= sampleCount;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;
            frequencyBand[i] = average * 10.0f;
        }
    }

    private void BandBuffer()
    {
        for (int i = 0; i < bandBuffer.Length; i++)
        {
            if (frequencyBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = frequencyBand[i];
                bufferDecrease[i] = initialBufferDecrease;
            }
            else
            {
                bandBuffer[i] = Mathf.Max(bandBuffer[i] - bufferDecrease[i], frequencyBand[i]);
                bufferDecrease[i] *= bufferGravity;
            }
        }
    }
}
