using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepsUI : MonoBehaviour
{
    private LevelManager levelManager;
    private int step = 0;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        levelManager = LevelManager.Get();
        levelManager.OnPlayerMove += UpdateUI;
    }

    private void UpdateUI()
    {
        step++;
        text.text = $"{step:00}";
    }

   
}
