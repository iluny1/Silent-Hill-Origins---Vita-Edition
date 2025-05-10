using UnityEngine.SceneManagement;
using UnityEngine;

public class Travis_Health : MonoBehaviour, IHealth
{
    [SerializeField] private Camera m_PlayerCamera;
    [SerializeField] private AudioClip m_ClipDeath;
    private const float MAX_HEALTH = 100f;

    public float Health { get; private set; }

    private void Start()
    {
        Health = MAX_HEALTH;
    }

    public void Death()
    {
        Travis_ScriptHandle.Instance.TravisMove.Moveable = false;
        Travis_ScriptHandle.Instance.TravisAnimation.AnimationDeath();
        GetComponent<Travis_GruntSFX>().PlaySpecialSFX(m_ClipDeath);
        FunctionTimer.Create(() => { DeathAnimation(); }, 5f);
    }

    public void DeathAnimation()
    {
        GameObject VFS = GameObject.FindGameObjectWithTag("VideoOnly");
        m_PlayerCamera.enabled = false;
        m_PlayerCamera.GetComponent<AudioListener>().enabled = false;
        VFS.GetComponent<Camera>().enabled = true;
        VFS.GetComponent<AudioListener>().enabled = true;
        VFS.GetComponent<VideoFS>().PlayVideo("GOMOVV");
        FunctionTimer.Create(() => { SceneManager.LoadScene("MainMenu"); }, 21f);
    }

    public void GetDamage(float damage, Vector3 hitPos)
    {
        Health -= damage;

        if (Health <= 0)
            Death();
        else
            ReactToDamage(hitPos - transform.position);
    }

    public void GetDamageQTE(float damage)
    {
        Health -= damage;

        if (Health <= 0)
            Health = 1;
    }

    public void ReactToDamage(Vector3 hitDir)
    {
        if (Vector3.Angle(transform.forward, hitDir) < 90f)
            Travis_ScriptHandle.Instance.TravisAnimation.AnimationHit(false);
        else
            Travis_ScriptHandle.Instance.TravisAnimation.AnimationHit(true);
    }

    public void GetDamage(float damage)
    {
        return;
    }
}
