using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPhysics : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 p = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        var collid = Physics2D.OverlapCircleAll(p, 0.1f);

        foreach(var c in collid)
        {
            Debug.Log(c.name + " " + LayerMask.LayerToName(gameObject.layer));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
