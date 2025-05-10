using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travis_Interaction : MonoBehaviour {

    private readonly Vector3 centerOffsetChest = new Vector3(0, 1f, 0);
    private readonly Vector3 centerOffsetUp = new Vector3(0, 2f, 0);

    public Transform m_CurrentInteraction { get; private set; }

    public void Interact()
    {
        Vector3 fwdPoint = transform.position + (transform.forward * 0.35f);
        Debug.DrawLine(fwdPoint, fwdPoint + centerOffsetUp, Color.cyan);

        Collider[] cols = Physics.OverlapCapsule(fwdPoint,
            fwdPoint + centerOffsetUp, 0.2f, 1 << 10);

        if (cols.Length == 1)
        {
            cols[0].GetComponent<IInteractable>().Interact();
            m_CurrentInteraction = cols[0].transform;
        }
        else if (cols.Length > 1)
        {
            IInteractable interactable = null;
            float lastDis = float.MaxValue;
            Vector3 playerPoint = fwdPoint + centerOffsetChest;

            foreach (Collider col in cols)
            {
                if (lastDis > Vector3.Distance(col.ClosestPoint(playerPoint), playerPoint))
                {
                    interactable = col.GetComponent<IInteractable>();
                    m_CurrentInteraction = col.transform;
                }
            }

            interactable.Interact();
            
        }
    }

    public void FinishEnemy(Transform enemy)
    {        
        Travis_ScriptHandle.Instance.TravisMove.Moveable = false;
        Vector3 fwd = enemy.position - transform.position; fwd.y = 0;
        transform.forward = fwd;
        Travis_ScriptHandle.Instance.TravisAnimation.AnimationFinishMove();

        FunctionTimer.Create(() => { Travis_ScriptHandle.Instance.TravisMove.Moveable = true; },
            Travis_ScriptHandle.Instance.TravisAnimation.AnimationGetLength());
    }
}
