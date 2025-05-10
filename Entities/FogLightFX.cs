using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogLightFX : MonoBehaviour {

    private ParticleSystem m_FogFX;
    private CapsuleCollider m_Collider;

    private void Awake()
    {
        m_FogFX = GetComponent<ParticleSystem>();
        m_Collider = GetComponent<CapsuleCollider>();
    }

    public void ToogleFX(bool enabled)
    {
        if (enabled)
            m_FogFX.Play();
        else
            m_FogFX.Stop();

        m_Collider.enabled = enabled;
    }    
}
