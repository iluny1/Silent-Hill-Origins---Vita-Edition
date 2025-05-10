using UnityEngine;

public interface IHealth
{
    float Health { get; }

    void GetDamage(float damage);
    void GetDamage(float damage, Vector3 hitPos);
    void Death();
    void ReactToDamage(Vector3 hitDir);
}
