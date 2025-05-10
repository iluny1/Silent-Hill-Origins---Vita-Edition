using UnityEngine;

public class FistsAdd : MonoBehaviour {

    Fists m_MainObject;
    Collider collider;

    public void SetAdd(Fists main)
    {
        m_MainObject = main;
        collider = GetComponent<Collider>();
        m_MainObject.SetAdd(collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IEnemy>() != null)
        {
            other.GetComponent<IHealth>().GetDamage(m_MainObject.GetDamage(), transform.position);
            collider.enabled = false;
        }
    }
}
