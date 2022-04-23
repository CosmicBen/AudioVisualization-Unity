using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    public int band;
    public float startScale, scaleMultiplier;
    public bool useBuffer = true;
    private Material material;

    private void Awake()
    {
        material = GetComponentInChildren<Renderer>().material;
    }

    //private void Update()
    //{
    //    //float scale = (useBuffer ? AudioPeer.bandBuffer[band] : AudioPeer.frequencyBand[band]) * scaleMultiplier + startScale;
    //    float scale = (useBuffer ? AudioPeer.audioBandBuffer[band] : AudioPeer.audioBand[band]) * scaleMultiplier + startScale;
    //    transform.localScale = new Vector3(transform.localScale.x, scale, transform.localScale.z);
    //}

    private void Update()
    {
        float bandValue = useBuffer ? AudioPeer.audioBandBuffer[band] : AudioPeer.audioBand[band];
        bandValue = float.IsNaN(bandValue) ? 0 : bandValue;

        transform.localScale = new Vector3(transform.localScale.x, bandValue * scaleMultiplier + startScale, transform.localScale.z);
        material.SetColor("_EmissiveColor", new Color(bandValue, bandValue, bandValue));
    }
}
