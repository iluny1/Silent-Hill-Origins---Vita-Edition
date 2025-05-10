using UnityEngine;

public class Travis_Animation : MonoBehaviour {
       
    private Animator m_Animator;
    private Enums.WeaponTypes m_WeaponBase;

    private float m_MovementState = 0f;
    private float m_MovementStateTarget = 0f;

    private float m_MoveX_Current = 0f;
    private float m_MoveZ_Current = 0f;
    private float m_MoveX_Target = 0f;
    private float m_MoveZ_Target = 0f;

    private bool m_LockOn;
    private float m_LockOnWeight = 0f;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (m_MovementState != m_MovementStateTarget)
        {
            m_MovementState = Mathf.MoveTowards(m_MovementState, m_MovementStateTarget, 0.1f);
            m_Animator.SetFloat("MoveState", m_MovementState);
        }

        if (m_MoveX_Current != m_MoveX_Target)
        {
            m_MoveX_Current = Mathf.MoveTowards(m_MoveX_Current, m_MoveX_Target, 0.15f);
            m_Animator.SetFloat("AimMoveX", m_MoveX_Current);
        }

        if (m_MoveZ_Current != m_MoveZ_Target)
        {
            m_MoveZ_Current = Mathf.MoveTowards(m_MoveZ_Current, m_MoveZ_Target, 0.15f);
            m_Animator.SetFloat("AimMoveZ", m_MoveZ_Current);
        }

        if (m_LockOn && m_LockOnWeight != 1)
        {
            m_LockOnWeight = Mathf.MoveTowards(m_LockOnWeight, 1, 0.05f);
            m_Animator.SetLayerWeight(1, m_LockOnWeight);
        }
        else if (!m_LockOn && m_LockOnWeight != 0)
        {
            m_LockOnWeight = Mathf.MoveTowards(m_LockOnWeight, 0, 0.05f);
            m_Animator.SetLayerWeight(1, m_LockOnWeight);
        }
        
    }

    public void SetWeaponBase(Enums.WeaponTypes weapon) { m_WeaponBase = weapon; }
    public Enums.WeaponTypes GetWeaponBase() { return m_WeaponBase; }

    public void SetMovementState(Enums.MovementStates state)
    {
        switch (state)
        {
            case Enums.MovementStates.Idle: m_MovementStateTarget = 0f; break;
            case Enums.MovementStates.Walk: m_MovementStateTarget = 1f; break;
            case Enums.MovementStates.RunExhaust: m_MovementStateTarget = 2f; break;
            case Enums.MovementStates.Run: m_MovementStateTarget = 3f; break; 
        }
    }

    public void ToggleAim(bool state, int weaponType)
    {
        m_Animator.SetInteger("WeaponType", weaponType);
        m_Animator.SetBool("WeaponReady", state);
    }

    public void SetWeaponType(int weaponType)
    {
        m_Animator.SetInteger("WeaponType", weaponType);
    }

    public void ToggleLowerBodyWeight(bool state)
    {
        m_LockOn = state;
    }

    public void SetAnimationAimMove(float moveX, float moveZ)
    {
        m_MoveX_Target = moveX;
        m_MoveZ_Target = moveZ;
    }

    public void AnimationWeaponFire(int weaponType)
    {
        if (weaponType < (int)Enums.WeaponTypes.Pistol) ToggleLowerBodyWeight(false);
        m_Animator.SetTrigger("Fire");
    }

    public void AnimationWeaponFireCharging()
    {
        m_Animator.SetBool("Charging", true);
    }

    public void AnimationWeaponReload()
    {
        m_Animator.SetTrigger("Reload");
    }

    public void AnimationFinishMove()
    {
        m_Animator.SetTrigger("Finish");
    }

    public float AnimationGetLength()
    {
        return m_Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    public void AnimationWeaponFireUncharge()
    {
        m_Animator.SetBool("Charging", false);
    }

    public void AnimationPickup(bool onTable)
    {
        m_Animator.SetTrigger("PickUp");
        m_Animator.SetBool("OnTable", onTable);
    }

    public void SetPained(bool state)
    {
        m_Animator.SetBool("Pained", state);
    }

    public void SetPainting(bool state)
    {
        m_Animator.SetBool("Painting", state);
    }

    public void AnimationHit(bool fromBack)
    {        
        m_Animator.SetTrigger("Hit");

        switch (fromBack)
        {
            case true: m_Animator.SetFloat("Direction", 0); break;
            default: m_Animator.SetFloat("Direction", 1); break;
        }
    }

    public void AnimationTurnAround()
    {
        m_Animator.SetTrigger("TurnAround");
    }

    public void AnimationMirror()
    {
        m_Animator.SetTrigger("Mirror");
    }

    public void AnimationDeath()
    {
        m_Animator.SetTrigger("Death");
    }

    public void AnimationQTE(Enums.Enemies enemy)
    {
        switch (enemy)
        {
            case Enums.Enemies.StraightJacket:
                {
                    m_Animator.SetTrigger("QTE_SJ");
                    break;
                }
        }
    }

    public void AnimationQTE_Hit()
    {
        m_Animator.SetTrigger("QTE_Hit");
    }

    public void AnimationQTE_Miss()
    {
        m_Animator.SetTrigger("QTE_Miss");
    }

    public void PlayGunFX()
    {
        Travis_ScriptHandle.Instance.TravisAim.CurrentWeapon.GetComponent<Gun>().PlayShootFX();
        Travis_ScriptHandle.Instance.TravisInteraction.m_CurrentInteraction.GetComponent<IEnemyAnimation>().TriggerAnimation("Flinch");
    }
}
