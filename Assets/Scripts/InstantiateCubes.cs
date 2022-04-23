using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCubes : MonoBehaviour
{
    public GameObject cubePrefab;
    public float radius = 100.0f;
    public float cubeScale = 1.0f;
    public float maxVerticalScale = 1.0f;
    public float baseVerticalScale = 2.0f;
    private GameObject[] cubes = new GameObject[512];

    private void Start()
    {
        float angle = 360.0f / cubes.Length;

        for (int i = 0; i < cubes.Length; i++)
        {
            GameObject cubeGameObject = Instantiate(cubePrefab);
            cubeGameObject.transform.parent = transform;
            cubeGameObject.transform.localPosition = Vector3.zero;
            cubeGameObject.name = "SampleCube" + i;

            transform.eulerAngles = new Vector3(0.0f, angle * i, 0.0f);
            cubeGameObject.transform.position = Vector3.forward * radius;
            cubes[i] = cubeGameObject;
        }
    }

    private void Update()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            if (cubes[i] != null)
            {
                cubes[i].transform.localScale = new Vector3(cubeScale, AudioPeer.samples[i] * maxVerticalScale + baseVerticalScale, cubeScale);
            }
        }
    }
}
