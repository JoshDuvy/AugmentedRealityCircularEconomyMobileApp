using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionObjectSingular : MonoBehaviour
{
    private float timeCheck;

    void Start()
    {
        //Destory Gameobject after time passed
        Destroy(gameObject, 3);
    }

    void FixedUpdate()
    {
        //Move pollution particle upwards at a constant rate
        //Code from Unity Doc https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
        timeCheck += Time.deltaTime;
        if (timeCheck >= 0.01f)
        {
            timeCheck = 0.0f;
            //End of Code from Unity Doc

            //move pollution particles upwards
            gameObject.transform.position += new Vector3(0.0f, 0.01f, 0.0f);
        }
    }
}
