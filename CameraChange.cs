using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour {

    [SerializeField] private Enums.OnTriggerEvent m_Event;

    [SerializeField] private bool m_OnLoading;
    [SerializeField] private bool m_Once;

    [SerializeField] private bool m_IsDynamicCamera;

    [SerializeField] private Camera m_OnEnter;
    [SerializeField] private Camera m_OnExit;
    [SerializeField] private bool m_OnExitIsDynamicCamera;

    [SerializeField] private Camera m_LastCam;

    [SerializeField] private Collider m_Collider;
    private bool m_WasDynamicCamera;

    

    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        switch (m_Event)
        {
            case Enums.OnTriggerEvent.OnEnter:
                {
                    bool isPlayerCameraLast;

                    if (m_OnLoading)
                    {
                        m_LastCam = other.GetComponentInChildren<Camera>();
                        isPlayerCameraLast = true;
                    }
                    else
                    {
                        m_LastCam = Camera.current;
                        isPlayerCameraLast = (m_LastCam == other.GetComponentInChildren<Camera>()) ? true : false;
                    }

                    m_WasDynamicCamera = other.GetComponent<Travis_Move>().IsDynamicCamera;

                    if (isPlayerCameraLast)                        
                        m_LastCam.enabled = other.GetComponent<AudioListener>().enabled = false;
                    else
                        m_LastCam.enabled = m_LastCam.GetComponent<AudioListener>().enabled = false;                    

                    m_OnEnter.enabled = m_OnEnter.GetComponent<AudioListener>().enabled = true;
                    other.GetComponent<Travis_Move>().ChangeCurrentCamera(m_OnEnter, m_IsDynamicCamera);
                    break;
                }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (m_Event)
        {
            case Enums.OnTriggerEvent.OnEnter:
                {
                    m_OnEnter.enabled = m_OnEnter.GetComponent<AudioListener>().enabled = false;
                    m_LastCam.enabled = other.GetComponent<AudioListener>().enabled = true;
                    other.GetComponent<Travis_Move>().ChangeCurrentCamera(m_LastCam, m_WasDynamicCamera);
                    m_Collider.enabled = m_Once ? false : true;
                    break;
                }
        }
    }
}
