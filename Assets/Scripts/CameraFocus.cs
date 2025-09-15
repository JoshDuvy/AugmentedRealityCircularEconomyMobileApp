using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CameraFocus : MonoBehaviour
{
    //All code entirely from Vuforia documentation https://library.vuforia.com/content/vuforia-library/en/articles/Solution/Working-with-the-Camera.html

    void Start()
    {
        var vuforia = VuforiaARController.Instance;
        vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        vuforia.RegisterOnPauseCallback(OnPaused);
    }

    private void OnVuforiaStarted()
    {
        CameraDevice.Instance.SetFocusMode(
            CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }

    private void OnPaused(bool paused)
    {
        if (!paused) // resumed
        {
            // Set again autofocus mode when app is resumed
            CameraDevice.Instance.SetFocusMode(
                CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
    }

    //Code end from Vuforia
}
