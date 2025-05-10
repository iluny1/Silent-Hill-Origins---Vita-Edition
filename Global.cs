using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

    public static Global Instance;

    private bool m_FirstLaunch = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SetFirstLaunchFalse() { m_FirstLaunch = false; }

    public bool GetFirstLaunch() { return m_FirstLaunch; }
}
