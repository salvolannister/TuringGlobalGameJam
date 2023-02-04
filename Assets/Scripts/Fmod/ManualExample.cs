using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualExample : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            instance = FMODUnity.RuntimeManager.CreateInstance("event:/Example");
            instance.start();
            instance.release();
        }
    }
}