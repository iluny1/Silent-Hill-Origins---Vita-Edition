using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAnimation {

    void SetWalkAnimation();

    void SetIdleAnimation();

    void SetRunAnimation();

    void SetFastRunAnimation();

    void TriggerAnimation(string triggerName);

    void TriggerQTEAnimation();

    void TriggerQTEHitAnimation();

    void TriggerQTEMissAnimation();

    void ActivateAttackCollider();
    void DeactivateAttackCollider();
}
