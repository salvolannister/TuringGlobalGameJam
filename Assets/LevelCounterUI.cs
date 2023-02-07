using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCounterUI : MonoBehaviour
{
    public Text stepCount;
    public Text deathCount;
    private int step = 0;
    private GameManager gameManager;
    private LevelManager levelManager;

    private void Awake()
    {
        gameManager = GameManager.Get();
    }
    // Start is called before the first frame update
    void Start()
    {            
        deathCount.text = $"{gameManager._currentPlayerDeath:00}";
        levelManager = LevelManager.Get();
        levelManager.OnPlayerMove += UpdateSteps;
    } 

    private void UpdateSteps()
    {
        step++;
        stepCount.text = $"{step:00}";
        Debug.Log("UpdateSteps " + step);
    }
}
