using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug_Positions : MonoBehaviour {

    public static Debug_Positions Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI m_SceneValue;
    [SerializeField] private TextMeshProUGUI m_PPosValue;
    [SerializeField] private TextMeshProUGUI m_PForwardValue;
    [SerializeField] private TextMeshProUGUI m_CPosValue;
    [SerializeField] private TextMeshProUGUI m_CForwardValue;
    [SerializeField] private TextMeshProUGUI m_CNameValue;
    [SerializeField] private TextMeshProUGUI m_InputValue;

    // Use this for initialization
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        SetSceneName();
    }

    // Update is called once per frame
    void Update()
    {
        SetInput();
        SetCameraData();
    }

    private void SetSceneName()
    {
        m_SceneValue.text = SceneManager.GetActiveScene().name;
    }

    private void SetInput()
    {
        m_InputValue.text = $"X: {Input.GetAxis("Horizontal")}; Y:{Input.GetAxis("Vertical")}";
    }

    public void SetPlayerData(Vector3 position, Vector3 forward)
    {
        m_PPosValue.text = position.ToString();
        m_PForwardValue.text = forward.ToString();
    }

    private void SetCameraData()
    {
        m_CPosValue.text = Camera.main.transform.position.ToString();
        m_CForwardValue.text = Camera.main.transform.forward.ToString();
        m_CNameValue.text = Camera.main.name;
    }
}
