using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    public static LoadLevel Instance;

    public string m_Scene;

    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Travis_Move>().Moveable = false;
            LoadNewScene(m_Scene);
        }
    }

    public void LoadNewScene(string scene)
    {
        GameObject.FindGameObjectWithTag("FadeFX").GetComponent<FadeInOut>().StartFade(1f, Color.clear, Color.black, true, 1f);

        FunctionTimer.Create(() => SceneManager.LoadScene(scene), 2f);
    }
}
