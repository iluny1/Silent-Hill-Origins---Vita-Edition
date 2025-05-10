using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travis_GruntSFX : SoundFXBank {

    [SerializeField] private AudioSource m_GruntSource;
    private int m_GruntsMaxIndex = -1;

    private void Start()
    {
        m_GruntsMaxIndex = clips.Length - 1;
    }

    public void GruntHit()
    {
        m_GruntSource.clip = clips[Random.Range(0, m_GruntsMaxIndex)];
        m_GruntSource.pitch = Random.Range(0.9f, 1.1f);
        m_GruntSource.Play();
    }

    public void PlaySpecialSFX(AudioClip clip)
    {
        m_GruntSource.Stop();
        m_GruntSource.clip = clip;
        m_GruntSource.pitch = 1f;
        m_GruntSource.Play();
    }

    public void PlaySpecialSFX_Array(AudioClip[] clips)
    {
        m_GruntSource.clip = clips[Random.Range(0, m_GruntsMaxIndex)];
        m_GruntSource.pitch = Random.Range(0.9f, 1.1f);
        m_GruntSource.Play();
    }
}
