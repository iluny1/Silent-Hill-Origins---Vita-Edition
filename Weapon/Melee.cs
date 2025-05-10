using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Melee : WeaponBase {

    

    private float m_Health;
    [SerializeField] private float m_DamageToWeapon;
    [SerializeField] private float m_DefaultHealth;


    private bool m_Charged;
    private float m_ChargeTime;
    [SerializeField] private float m_DefaultChargeTime;

    protected bool m_WaitingInput = false;


    [SerializeField] private Collider m_Collider;

    protected virtual void Start()
    {
        m_ChargeTime = m_DefaultChargeTime;
        m_Collider = GetComponent<Collider>();
        //m_Collider.enabled = false;
    }

    private void Update()
    {
        /*if (m_WaitingInput)
        {
            if (!m_Charged)
            {
                m_ChargeTime -= Time.deltaTime;
                if (m_ChargeTime <= 0f) m_Charged = true; 
            }
        }*/
    }

    public override void PrepareFire()
    {
        /* m_ChargeTime = m_DefaultChargeTime;
         m_WaitingInput = true;*/
        Travis_ScriptHandle.Instance.TravisAnimation.AnimationWeaponFireCharging();
    }

    public override void Fire()
    {
        //m_WaitingInput = false;
        if (!m_Allow) return;
        m_Collider.enabled = true;
        Travis_ScriptHandle.Instance.TravisAnimation.AnimationWeaponFireUncharge();
        Travis_ScriptHandle.Instance.TravisAnimation.AnimationWeaponFire((int)Type);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IEnemy>() != null)
        {
            other.GetComponent<IHealth>().GetDamage(m_Damage, transform.position);
            //m_Collider.enabled = false;
        }
    }

    public override void ColliderOff()
    {
        m_Collider.enabled = false;
        m_Allow = false;
    }

    public override void ComboColliderActivate()
    {
        m_Collider.enabled = true;
    }

    public float GetDamage()
    {
        return m_Damage;
    }

    public bool CheckAllowance()
    {
        if (!m_Allow) return false;
        else return true;
    }

    public override void WeaponDefaultState()
    {
        m_Allow = true;
    }

    public override int GetWeaponType()
    {
        return (int)Type;
    }

    public override void Reload()
    {
        return;
    }
}
