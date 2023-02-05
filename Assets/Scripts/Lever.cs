using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private GameObject[] gates;
    [SerializeField, Layer] private int defaultLayer;
    [SerializeField, Layer] private int wallLayer;
    [SerializeField] private bool[] opens;
    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < gates.Length; i++)
            if (opens[i]) {
                gates[i].layer = defaultLayer;
                gates[i].GetComponentInChildren<SpriteRenderer>().color = Color.white;

            }


    }

        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < gates.Length; i++)
            {
                if (opens[i])
                {
                    opens[i] = false;
                    gates[i].layer = wallLayer;
                    gates[i].GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
                    StartCoroutine(ExampleCoroutine());
                }
                else
                {
                    opens[i] = true;
                    gates[i].layer = defaultLayer;
                    gates[i].GetComponentInChildren<SpriteRenderer>().color = Color.white;
                    StartCoroutine(ExampleCoroutine());
                }
            }
        }
    }

    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Objects/Lever");
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.5f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Objects/Gate_Slide");
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}
