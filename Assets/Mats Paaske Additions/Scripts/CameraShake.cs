using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool isShaking;
    public float shakeIntensity;
    public Vector3 defaultPos;

    public void Start()
    {
        defaultPos = transform.position;
    }

    public void Update()
    {
        shaking();
    }

    public void shaking() //simple camera shake to give a more immersive feel, spesifically made to make the slow and "heavy" rocket dud launch feel better
    {
        float x = Random.Range(-1f, 1f) * shakeIntensity;
        float y = Random.Range(-1f, 1f) * shakeIntensity;

        if (isShaking)
        {
            transform.position += new Vector3(x, y, 0);
        } else
        {
            transform.position = defaultPos;
        }
    }
}
