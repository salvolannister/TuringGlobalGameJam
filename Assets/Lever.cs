using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private GameObject gate;
    [SerializeField, Layer] private int defaultLayer;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            gate.layer = defaultLayer;
            gate.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
    }
}
