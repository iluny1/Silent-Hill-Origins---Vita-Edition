using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBallPool : MonoBehaviour
{
    public static AcidBallPool Instance;
    [SerializeField] private GameObject m_AcidBallPrefab;
    private readonly Queue<GameObject> m_Pool = new Queue<GameObject>();
    private const int POOL_SIZE = 10;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < POOL_SIZE; i++)
        {
            GameObject acidBall = Instantiate(m_AcidBallPrefab);
            acidBall.SetActive(false);
            m_Pool.Enqueue(acidBall);
        }
    }

    public GameObject GetAcidBall(Vector3 position, Vector3 target, GameObject owner)
    {
        GameObject acidBall = m_Pool.Count > 0 ? m_Pool.Dequeue() : Instantiate(m_AcidBallPrefab);
        acidBall.transform.position = position;
        acidBall.SetActive(true);
        acidBall.GetComponent<AcidBall>().Initialize(target, position, owner);
        return acidBall;
    }

    public void ReturnAcidBall(GameObject acidBall)
    {
        ParticleSystem fx = acidBall.GetComponent<ParticleSystem>();
        fx.Stop();
        FunctionTimer.Create(() => { acidBall.SetActive(false); }, 2f); //MAGIC NUMBER (StartLifeTime of Prefab)
        m_Pool.Enqueue(acidBall);
    }
}
