using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public int targetScene;

    //Script that on being pressed by a user will load the next screen
    //Code adapted from https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
    //and code adapted from https://docs.unity3d.com/ScriptReference/SceneManagement.Scene-buildIndex.html
    public void userHasClicked()
    {
        SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
    }
    //end of code from unity docs   
}
