using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCounterUI : MonoBehaviour
{
    public Text stepCount;
    public Text deathCount;

    private int _step = 0;
    private GameManager _gameManager;
 

    private void Awake()
    {
        _gameManager = GameManager.Get();
    
        if( _gameManager == null)
        {
            Debug.LogError("Can't find GameManager in the scene");
        }
    }
    void Start()
    {            
        deathCount.text = $"{_gameManager._currentPlayerDeath:00}";
        PlayerMovement playerMovement = _gameManager.GetPlayerMovement();
        playerMovement.OnPlayerMove += UpdateSteps;
    } 
    private void OnDestroy()
    {
        PlayerMovement playerMovement = _gameManager.GetPlayerMovement();
        if( playerMovement != null )
        {
            playerMovement.OnPlayerMove -= UpdateSteps;

        }
    }

    private void UpdateSteps()
    {
        _step++;
        stepCount.text = $"{_step:00}";
        Debug.Log("UpdateSteps " + _step);
    }

}
