using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShotExample : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Footsteps");
        }
    }
}