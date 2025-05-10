using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fists : Melee {

    new readonly Enums.WeaponTypes Type = Enums.WeaponTypes.Fists;
    [SerializeField] Collider m_ColliderAdd;

    [SerializeField] GameObject FistAddPrefab;

    private bool m_SecondHit = false;

    protected override void Start()
    {
        base.Start();
        FistsAdd add = Instantiate(FistAddPrefab, Travis_ScriptHandle.Instance.TravisAim.GetArmAdd()).GetComponent<FistsAdd>();
        add.SetAdd(this);
    }

    public override void Fire()
    {
        if (!m_SecondHit)
        {
            base.Fire();
            Travis_ScriptHandle.Instance.TravisMove.Moveable = false;
        }
        else
        {
            m_WaitingInput = false;
            if (!m_Allow) return;
            Travis_ScriptHandle.Instance.TravisAnimation.AnimationWeaponFire((int)Type);
            Travis_ScriptHandle.Instance.TravisMove.Moveable = false;
        }

    }

    public override void ColliderOff()
    {
        if (!m_SecondHit)
        {
            base.ColliderOff();
            m_SecondHit = true;
            m_ColliderAdd.enabled = true;
        }
        else
        {
            m_ColliderAdd.enabled = false;
            m_Allow = false;
            m_SecondHit = false;
        }
    }

    public override void ComboColliderActivate()
    {
        m_ColliderAdd.enabled = true;
    }

    public override void WeaponDefaultState()
    {
        base.WeaponDefaultState();
        m_SecondHit = false;
    }

    public void SetAdd(Collider colAdd)
    {
        m_ColliderAdd = colAdd;
    }

    public void UnsetAdd()
    {
        Destroy(m_ColliderAdd.gameObject);
    }
}
