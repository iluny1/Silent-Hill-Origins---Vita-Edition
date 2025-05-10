using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyAwareExt : MonoBehaviour {

    IEnemy enemy;

    private void Start()
    {
        enemy = transform.parent.GetComponent<IEnemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        enemy.OnAwareEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        enemy.OnAwareExit(other);
    }
}
