using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour {

    [SerializeField] protected Enums.WeaponTypes Type;
    [SerializeField] protected string NameDisplay;
    [SerializeField] protected float m_Damage;
    protected bool m_Allow = true;
    [SerializeField] private bool m_MoveableOnHit;

    public abstract void PrepareFire();
    public abstract void Fire();
    public abstract void Reload();
    public abstract void ColliderOff();
    public abstract void WeaponDefaultState();
    public abstract void ComboColliderActivate();
    public abstract int GetWeaponType();
}
