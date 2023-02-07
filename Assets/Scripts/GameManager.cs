using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    public int _currentSceneIndex = 0;
    public int _currentPlayerDeath;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _currentPlayerDeath = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void GameOver()
    {
        LevelManager.Get().GameOver();
    }

    public static void StartGame()
    {
        LevelManager.Get().LoadNextScene();
    }

    public void IncreaseDeathCounter()
    {
        Debug.Log("IncreaseDeathCounter " + _currentPlayerDeath);
        _currentPlayerDeath++;
        Debug.Log("IncreaseDeathCounter " + _currentPlayerDeath);
    }
}
