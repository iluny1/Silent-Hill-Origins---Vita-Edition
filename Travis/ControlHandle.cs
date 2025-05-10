using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHandle : MonoBehaviour {
    	
	// Update is called once per frame
	void Update ()
    {
        /*foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                // Handle the pressed key
                Debug.Log("Key pressed: " + keyCode.ToString());
                break; // Exit the loop once a key is detected
            }
        }*/

        if (Input.GetButtonDown("Flashlight")) Travis_ScriptHandle.Instance.TravisFlashlight.ToogleFlashlight();

        if (Input.GetButtonDown("ResetCamera")) Travis_ScriptHandle.Instance.TravisResetCamera.ResetCamera();

        if (Input.GetButtonDown("Start")) Travis_ScriptHandle.Instance.TravisDebug.ToogleDebugMenu();

        if (Input.GetButtonDown("Aim")) Travis_ScriptHandle.Instance.TravisAim.Aim();

        if (Input.GetButtonUp("Aim")) Travis_ScriptHandle.Instance.TravisAim.AimCease();

        if (!Input.GetButton("Aim") && Input.GetButtonDown("Use")) Travis_ScriptHandle.Instance.TravisInteraction.Interact();

        if (Input.GetButton("Aim") && Input.GetButtonDown("Use")) Travis_ScriptHandle.Instance.TravisAim.Shoot();

        if (Input.GetButton("Aim") && Input.GetButtonUp("Use")) Travis_ScriptHandle.Instance.TravisAim.Shoot();

        if (Input.GetButton("Aim") && Input.GetButtonDown("Reload")) Travis_ScriptHandle.Instance.TravisAim.Reload();

    }
}
