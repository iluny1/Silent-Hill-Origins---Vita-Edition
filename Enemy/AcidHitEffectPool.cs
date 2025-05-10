using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidHitEffectPool : MonoBehaviour {


    public static AcidHitEffectPool Instance;
    [SerializeField] private GameObject m_AcidHitEffectPrefab;
    private readonly Queue<GameObject> m_Pool = new Queue<GameObject>();
    private const int POOL_SIZE = 10;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < POOL_SIZE; i++)
        {
            GameObject acidBall = Instantiate(m_AcidHitEffectPrefab);
            acidBall.SetActive(false);
            m_Pool.Enqueue(acidBall);
        }
    }

    public GameObject GetAcidHitEffect(Vector3 position)
    {
        GameObject acidHitEffect = m_Pool.Count > 0 ? m_Pool.Dequeue() : Instantiate(m_AcidHitEffectPrefab);
        acidHitEffect.transform.position = position;
        acidHitEffect.SetActive(true);
        acidHitEffect.GetComponent<ParticleSystem>().Play();
        FunctionTimer.Create(() => ReturnAcidHitEffect(acidHitEffect), acidHitEffect.GetComponent<ParticleSystem>().main.duration);
        return acidHitEffect;
    }

    public void ReturnAcidHitEffect(GameObject acidBall)
    {
        acidBall.SetActive(false);
        m_Pool.Enqueue(acidBall);
    }
}
