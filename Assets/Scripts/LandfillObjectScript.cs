using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* LandfillObjectScript - Attacted to landfill objects
 * 
 * Delete landfill models after a certain time has passed
 */

public class LandfillObjectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Destory Gameobject after time passed to stop multiple objects stacking
        Destroy(gameObject, 0.5f);
    }
}
