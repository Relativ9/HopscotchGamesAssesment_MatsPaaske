using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyShieldFadeIn : MonoBehaviour
{

    private Renderer shieldRend;
    private bool shieldHit;
    // Start is called before the first frame update
    void Start()
    {
        shieldRend = GetComponent<Renderer>();
    }

    private void Update()
    {
        float currentAlpha = shieldRend.material.GetFloat("_AlphaStrength");
        float emissiveStrength = shieldRend.material.GetFloat("_EmissiveStrength");
        if (shieldHit)
        {
            currentAlpha += 8f * Time.deltaTime;
            emissiveStrength += 8f * Time.deltaTime;


        } else
        {
            currentAlpha -= 2f * Time.deltaTime;
            emissiveStrength -= 5f * Time.deltaTime;
        }
        currentAlpha = Mathf.Clamp(currentAlpha, 0f, 10f);
        emissiveStrength = Mathf.Clamp(emissiveStrength, 1f, 10f);
        shieldRend.material.SetFloat("_AlphaStrength", currentAlpha);
        shieldRend.material.SetFloat("_EmissiveStrength", emissiveStrength);
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine("pulseShield");
    }

    IEnumerator pulseShield()
    {
        shieldHit = true;
        yield return new WaitForSeconds(0.5f);
        shieldHit = false;
    }
}
