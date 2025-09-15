using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandfillTruckScript : MonoBehaviour
{
    public Vector3 MovementPosition;

    //When truck is created point and move towards the associated landfill gameobject and delete when it reaches the landfill
    //Code from Unity Documentation https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html
    void Update()
    {
        //Move towards landfill
        transform.position = Vector3.MoveTowards(transform.position, MovementPosition, (0.1f * Time.deltaTime));

        //When reaching landfill delete gameobject
        if (transform.position == MovementPosition)
        {
            //end of code from unity documentation
            Destroy(gameObject);
        }

        //Point truck model in direction of travel to avoid 'driving' backwards
        //code from unity documentation https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (MovementPosition - transform.position), 5.0f, 0.0f));

    }
    //end of code from unity doc
}
