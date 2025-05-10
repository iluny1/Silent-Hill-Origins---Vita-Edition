using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    Enums.Enemies Type { get; }
    Enums.ThinkStates ThinkState { get; }

    int GetStateID();
    void OnAwareEnter(Collider other);
    void OnAwareExit(Collider other);

}
