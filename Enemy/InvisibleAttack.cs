using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleAttack : MonoBehaviour {
       
    private SphereCollider m_Collider;

    private void Awake()
    {
        m_Collider = GetComponent<SphereCollider>();
    }

    public void DeactivateCollider()
    {
        m_Collider.enabled = false;
    }

    public void ActivateCollider()
    {
        m_Collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IHealth>().GetDamage(20f, transform.position);
            DeactivateCollider();
        }
    }

}
