using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightJacket_QTEFX : MonoBehaviour {

    [SerializeField] private ParticleSystem m_Goo;
    [SerializeField] private AudioClip m_Intro;
    [SerializeField] private AudioClip m_Struggle;
    [SerializeField] private AudioClip m_AttackHit;
    [SerializeField] private AudioClip m_AttackMiss;
    [SerializeField] private AudioClip m_Orgasm;
    [SerializeField] private AudioClip m_OrgasmHit;
    [SerializeField] private AudioClip m_OrgasmMiss;

    [SerializeField] private AudioSource m_EnemySFX;

    public void QTEFX_Intro()
    {
        m_EnemySFX.Stop();
        m_EnemySFX.clip = m_Intro;
        m_EnemySFX.loop = false;
        m_EnemySFX.Play();
    }

    public void QTEFX_Struggle()
    {
        m_EnemySFX.clip = m_Struggle;
        m_EnemySFX.loop = true;
        m_EnemySFX.timeSamples = Random.Range(0, m_Struggle.samples - 1);
        m_EnemySFX.Play();
    }

    public void QTEFX_Orgasm()
    {
        m_EnemySFX.clip = m_Orgasm;
        m_EnemySFX.loop = true;
        m_EnemySFX.timeSamples = Random.Range(0, m_Orgasm.samples - 1);
        m_EnemySFX.Play();
    }

    public void QTEFX_AttackMiss()
    {
        m_EnemySFX.Stop();
        m_EnemySFX.clip = m_AttackMiss;
        m_EnemySFX.loop = false;
        m_EnemySFX.timeSamples = 0;
        m_EnemySFX.Play();
    }

    public void QTEFX_AttackHit()
    {
        m_EnemySFX.Stop();
        m_EnemySFX.clip = m_AttackHit;
        m_EnemySFX.loop = false;
        m_EnemySFX.timeSamples = 0;
        m_EnemySFX.Play();
    }

    public void QTEFX_OrgasmMiss()
    {
        m_EnemySFX.Stop();
        m_EnemySFX.clip = m_OrgasmMiss;
        m_EnemySFX.loop = false;
        m_EnemySFX.timeSamples = 0;
        m_EnemySFX.Play();
    }

    public void QTEFX_OrgasmHit()
    {
        m_EnemySFX.Stop();
        m_EnemySFX.clip = m_OrgasmHit;
        m_EnemySFX.loop = false;
        m_EnemySFX.timeSamples = 0;
        m_EnemySFX.Play();
        m_Goo.Play();
    }
}
