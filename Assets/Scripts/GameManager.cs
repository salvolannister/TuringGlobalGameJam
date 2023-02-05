using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    public int _currentSceneIndex = 1;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Init Game Manager");
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
