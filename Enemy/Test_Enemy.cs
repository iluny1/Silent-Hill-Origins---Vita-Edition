using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Enemy : MonoBehaviour, IEnemy, IHealth {

    public Enums.Enemies Type { get; } = Enums.Enemies.StraightJacket;
    public Enums.ThinkStates ThinkState { get; } = Enums.ThinkStates.Idle;

    [SerializeField] private ParticleSystem m_Blood;
    [SerializeField] private AudioSource m_Audio;

    [SerializeField] private AudioClip m_Clip;
    [SerializeField] private AudioClip m_Death;

    [SerializeField] private float m_Health = 50f;

    
    public float Health
    {
        get
        {
            return m_Health;
        }
    }

    public void Death()
    {
        m_Audio.clip = m_Death;
        m_Audio.Play();
        Travis_ScriptHandle.Instance.TravisAim.UnlockTarget(gameObject);
        Destroy(gameObject);
    }

    public void GetDamage(float damage)
    {
        m_Health -= damage;
        m_Blood.Play();
        m_Audio.clip = m_Clip;
        m_Audio.Play();
        if (m_Health <= 0f) Death();
    }

    public void GetDamage(float damage, Vector3 hitPos)
    {
        GetDamage(damage);
    }

    public void ReactToDamage(Vector3 hitDir)
    {
        throw new System.NotImplementedException();
    }

    public int GetStateID()
    {
        return 0;
    }

    public void OnAwareEnter(Collider other)
    {
        throw new System.NotImplementedException();
    }

    public void OnAwareExit(Collider other)
    {
        throw new System.NotImplementedException();
    }
}
