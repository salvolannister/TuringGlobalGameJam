using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{

    private Button restartButton;

    private void Awake()
    {
        restartButton = GetComponent<Button>();
    }
    private void OnEnable()
    {
        restartButton.onClick.AddListener(RestartScene);
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveListener(RestartScene);
    }

    private void RestartScene()
    {
        // Play Sound
        LevelManager.Get().ResetCurrentScene();
    }

    
}
