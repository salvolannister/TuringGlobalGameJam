using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private GameObject gate;
    [SerializeField, Layer] private int defaultLayer;
    [SerializeField, Layer] private int wallLayer;
    [SerializeField] private bool open;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (open)
            {
                open = false;
                gate.layer = wallLayer;
                gate.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
            }
            else
            {
                open = true;
                gate.layer = defaultLayer;
                gate.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
