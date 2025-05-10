using UnityEngine;

public class NavPoint : MonoBehaviour {

    [SerializeField] private NavPoint[] m_Neighbours;

    public NavPoint GetNextNeighbour()
    {
        if (m_Neighbours.Length > 1)
            return m_Neighbours[Random.Range(0, m_Neighbours.Length - 1)];
        else
            return m_Neighbours[0];
    }

    public Vector2 GetCoordinates()
    {
        return new Vector2(transform.position.x, transform.position.z);
    }
}
