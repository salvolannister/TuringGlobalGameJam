using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oil : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>()?.CheckAndPerformLastMovement();
            FMODUnity.RuntimeManager.PlayOneShot("event:/Objects/Oil_Slide");
        }            
    }
}
