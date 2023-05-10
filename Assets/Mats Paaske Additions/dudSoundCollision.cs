using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dudSoundCollision : MonoBehaviour
{
    private AudioSource dudSound;

    void Start()
    {
        dudSound = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        dudSound.Play();
    }
}
