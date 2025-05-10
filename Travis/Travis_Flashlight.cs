using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travis_Flashlight : MonoBehaviour {

    [SerializeField] Light m_Spot;
    [SerializeField] Light m_Ambient;
    [SerializeField] AudioSource m_sfx;

    [SerializeField] AudioClip m_sfxOn;
    [SerializeField] AudioClip m_sfxOff;

    [SerializeField] FogLightFX m_FogLightFX;

    [SerializeField] private bool isFlashlightOn = false;

    private void Start()
    {
        m_FogLightFX = FindObjectOfType<FogLightFX>();
        if (m_FogLightFX != null) m_FogLightFX.ToogleFX(isFlashlightOn); 
        m_Spot.enabled = m_Ambient.enabled = isFlashlightOn;
    }

    public void ToogleFlashlight()
    {
        m_Spot.enabled = m_Ambient.enabled = isFlashlightOn = !isFlashlightOn;
        if (m_FogLightFX != null) m_FogLightFX.ToogleFX(isFlashlightOn);
        PlaySound();
    }

    public void ForceFlashlight(bool newState)
    {
        if (m_FogLightFX != null) m_FogLightFX.ToogleFX(isFlashlightOn);
        m_Spot.enabled = m_Ambient.enabled = isFlashlightOn = newState;
        PlaySound();
    }

    private void PlaySound()
    {
        if (isFlashlightOn)
            m_sfx.clip = m_sfxOn;
        else
            m_sfx.clip = m_sfxOff;

        m_sfx.Play();
    }
}
