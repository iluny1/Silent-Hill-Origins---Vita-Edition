using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour {

    [SerializeField] private AudioSource m_SFX;
    [SerializeField] private AudioSource m_Steps;

    [SerializeField] private AudioClip[] m_StepClips;

    [SerializeField] private AudioClip[] m_IdleFX;
    [SerializeField] private AudioClip[] m_SpottedFX;
    [SerializeField] private AudioClip[] m_AttackFX;
    [SerializeField] private AudioClip[] m_ImpactFX;
    [SerializeField] private AudioClip[] m_GruntFX;
    [SerializeField] private AudioClip m_OnGroundFX;
    [SerializeField] private AudioClip m_DeathFX;

    private bool m_Loop = false;


    public void PlayStep()
    {
        m_Steps.clip = m_StepClips[Random.Range(0, 3)];
        m_Steps.pitch = Random.Range(0.8f, 1.2f);
        m_Steps.Play();
    }

    public void PlayFX(Enums.EnemySFXTypes type)
    {
        m_SFX.loop = false;

        switch (type)
        {
            case Enums.EnemySFXTypes.Idle:
                {
                    m_SFX.clip = m_IdleFX[Random.Range(0, m_IdleFX.Length - 1)];
                    break;
                }
            case Enums.EnemySFXTypes.Spot:
                {
                    m_SFX.clip = m_SpottedFX[Random.Range(0, m_SpottedFX.Length - 1)];
                    break;
                }
            case Enums.EnemySFXTypes.Attack:
                {
                    m_SFX.clip = m_AttackFX[Random.Range(0, m_AttackFX.Length - 1)];
                    break;
                }
            case Enums.EnemySFXTypes.Death:
                {
                    m_SFX.clip = m_DeathFX;
                    break;
                }
            case Enums.EnemySFXTypes.Impact:
                {
                    m_SFX.clip = m_ImpactFX[Random.Range(0, m_ImpactFX.Length - 1)];
                    break;
                }
            case Enums.EnemySFXTypes.Writhe:
                {
                    m_SFX.clip = m_OnGroundFX;
                    break;
                }
        }

        m_SFX.pitch = Random.Range(0.9f, 1.1f);
        m_SFX.Play();
    }

    public void SetLoop(bool state)
    {
        m_SFX.loop = state;
    }
}
