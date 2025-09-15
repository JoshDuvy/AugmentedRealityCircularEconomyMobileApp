using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFound : MonoBehaviour
{
    //Script to set objects tag to found when in view of camera
    //Adapted from Unity documentation https://docs.unity3d.com/ScriptReference/GameObject-tag.html

    public void FoundObject()
    {
        tag = "FoundObject";
    }

    public void LostObject()
    {
        tag = "LostObject";
    }
    //end of adapted code from unity doc
}
