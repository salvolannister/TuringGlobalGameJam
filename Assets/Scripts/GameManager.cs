using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    public int _currentSceneIndex = 0;
    public int _currentPlayerDeath;
    
    private PlayerMovement _playerMovement;
    
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

    public PlayerMovement GetPlayerMovement()
    {
        if(_playerMovement == null)
        {
            GameObject PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
            if(PlayerGameObject != null )
            {
                 _playerMovement = PlayerGameObject.GetComponent<PlayerMovement>();
            }
           
            if(_playerMovement == null)
            {
                Debug.LogWarning("Couldn't find player movement");
            }
            
        }

        return _playerMovement;
    }
    /** Return -1 to signal something is not working */
    public int GetCurrentPlayerSteps()
    {
        PlayerMovement playerMovement = GetPlayerMovement();
        if (playerMovement != null)
        {
           return playerMovement.CurrentPlayerSteps;
        }

        return -1;
    }
}
