using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private GameObject[] gates;
    [SerializeField, Layer] private int defaultLayer;
    [SerializeField, Layer] private int wallLayer;
    [SerializeField] private bool[] opens;
    [SerializeField] private Sprite leftLever;
    [SerializeField] private Sprite rightLever;
    private SpriteRenderer spriteRenderer;
    private bool right=false;
    // Start is called before the first frame update
    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = leftLever;
        for (int i = 0; i < gates.Length; i++)
            if (opens[i]) {
                OpenGate(gates[i]);
            }
    }

        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (right)
            {
                spriteRenderer.sprite = leftLever;
                right = false;
            }
            else
            {
                spriteRenderer.sprite = rightLever;
                right = true;
            }
            for (int i = 0; i < gates.Length; i++)
            {
                if (opens[i]) {
                    CloseGate(gates[i]);
                    opens[i] = false;
                }

                else {
                    OpenGate(gates[i]);
                    opens[i] = true;
                }

            }
        }
    }

    private void OpenGate(GameObject g) {
        g.layer = defaultLayer;
        SpriteRenderer[] trs = g.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer t in trs) {
            Debug.Log(t.name);
            if (t.name == "Open")
                t.color=new Color(1,1,1,1);
            else
                t.color = new Color(1, 1, 1, 0);
        }
    }

    private void CloseGate(GameObject g)
    {
        g.layer = wallLayer;
        SpriteRenderer[] trs = g.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer t in trs)
        {
            if (t.name == "Close")
                t.color = new Color(1, 1, 1, 1);
            else
                t.color = new Color(1, 1, 1, 0);
        }
    }
}
