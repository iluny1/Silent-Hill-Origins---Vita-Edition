using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travis_DebugMenu : MonoBehaviour {

    [SerializeField] private Canvas DebugMenu;

    public void ToogleDebugMenu()
    {
        DebugMenu.enabled = !DebugMenu.enabled;
    }
}
