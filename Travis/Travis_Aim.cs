using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travis_Aim : MonoBehaviour {

	public bool IsAiming { get; private set; }
    public bool Locked { get; private set; }

    [SerializeField] SphereCollider m_LockOnCollider;

    public WeaponBase CurrentWeapon { get; private set; }
    private bool m_PressedFire = false;

    [SerializeField] private Transform m_ArmMain;
    [SerializeField] private Transform m_ArmAdd;
    [SerializeField] private Transform m_SeePoint;

    public Transform m_Enemy { get; private set; }
    private Vector3 m_ForwardTarget = new Vector3 (0, 0, 0);

    private void Start()
    {
        CurrentWeapon = transform.GetComponentInChildren<WeaponBase>();
    }

    private void Update()
    {
        if (!IsAiming) return;

        Debug.DrawLine(m_SeePoint.position, (m_Enemy != null ? m_Enemy.position : m_SeePoint.position) + Vector3.up, Color.red);

        if (m_Enemy != null && Locked)
        {
            Ray ray = new Ray(m_SeePoint.position, (m_Enemy.position - m_SeePoint.position) + Vector3.up);
            RaycastHit raycast;

            if (Physics.Raycast(ray, out raycast, 50f, (1 << 0) | (1 << 10)))
            {
                Debug.Log($"Raycast hitted {raycast.transform.name} at {raycast.point} - ID: {raycast.transform.GetInstanceID()}");
                Debug.DrawRay(raycast.point, raycast.normal, Color.green, 0.1f);

                if ((raycast.transform.parent != null
                    && raycast.transform.parent.GetComponent<IEnemy>() != null) || raycast.transform.GetComponent<IEnemy>() != null)
                {
                    LockDirection();
                    transform.forward = Vector3.Slerp(transform.forward, m_ForwardTarget, 0.1f);
                    return;
                }
            }

            UnlockTarget(m_Enemy.gameObject);
        }
    }

    public void Aim()
    {
        m_LockOnCollider.enabled = true;
        IsAiming = true;
        m_PressedFire = false;
        Travis_ScriptHandle.Instance.TravisAnimation.ToggleAim(true, CurrentWeapon.GetWeaponType());
        Travis_ScriptHandle.Instance.TravisAnimation.ToggleLowerBodyWeight(true);
    }

    public void AimCease()
    {
        m_LockOnCollider.enabled = false;
        m_PressedFire = false;
        IsAiming = false;

        if(m_Enemy != null)
            UnlockTarget(m_Enemy.gameObject);

        Travis_ScriptHandle.Instance.TravisAnimation.ToggleAim(false, CurrentWeapon.GetWeaponType());
        Travis_ScriptHandle.Instance.TravisAnimation.ToggleLowerBodyWeight(false);
        Travis_ScriptHandle.Instance.TravisAnimation.SetAnimationAimMove(0, 0);
    }

    public void LockTarget(Transform enemy)
    {
        if (Locked) return;
        m_Enemy = enemy;
        Ray ray = new Ray(m_ArmMain.position, (m_Enemy.position - m_ArmMain.position) + Vector3.up);
        RaycastHit raycast;

        if (Physics.Raycast(ray, out raycast, 50f, (1 << 0) | (1 << 10)) && 
            ((raycast.transform.parent != null && raycast.transform.parent.GetComponent<IEnemy>() != null)
                || raycast.transform.GetComponent<IEnemy>() != null))
            Locked = true;
        else
            m_Enemy = null;
        Debug.Log($"Locked Target - {enemy.gameObject.name}\n GameObject ID - {enemy.gameObject.GetInstanceID()}\n Position - {enemy.position}");

    }

    public void UnlockTarget(GameObject enemy)
    {
        if (m_Enemy != null && enemy.GetInstanceID() != m_Enemy.gameObject.GetInstanceID()) return;
        m_Enemy = null;
        Locked = false;
        Debug.Log($"Unlocked Target - {enemy.name}\n GameObject ID - {enemy.GetInstanceID()}\n Position - {enemy.transform.position}");
    }

    public void LockDirection()
    {
        if (m_Enemy == null)
        {
            Locked = false;
            return;
        }
            
        m_ForwardTarget = m_Enemy.position - transform.position;
        m_ForwardTarget.y = 0;
    }

    public Vector3 LockDirectionVector()
    {
        if (m_Enemy == null) return Vector3.zero;

        Vector3 forward = m_Enemy.position - transform.position;
        forward.y = 0;
        return forward;
    }

    public void Shoot()
    {
        if (!m_PressedFire)
            CurrentWeapon.PrepareFire();
        else
            CurrentWeapon.Fire();

        m_PressedFire = !m_PressedFire;
    }

    public void Reload()
    {
        //Travis_ScriptHandle.Instance.TravisAnimation.SetWeaponType(m_CurrentWeapon.GetWeaponType());
        CurrentWeapon.Reload();
    }

    public void WeaponColliderOff()
    {
        CurrentWeapon.ColliderOff();
    }

    public void WeaponComboColliderActivate()
    {
        CurrentWeapon.ComboColliderActivate();
    }

    public Transform GetArmAdd()
    {
        return m_ArmAdd;
    }    

    public void WeaponDefaultState()
    {
        CurrentWeapon.WeaponDefaultState();
    }

    public void BackToIdleAim()
    {
        Travis_ScriptHandle.Instance.TravisAnimation.ToggleLowerBodyWeight(true);
        Travis_ScriptHandle.Instance.TravisMove.Moveable = true;
    }
}
