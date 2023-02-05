using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public Button startPlaying;
    public Button exit;

    private void OnEnable()
    {
        startPlaying.onClick.AddListener(StartGame);
        exit.onClick.AddListener(QuitGame); 
    }

    private void OnDisable()
    {
        startPlaying.onClick.RemoveListener(StartGame);
        exit.onClick.RemoveListener(QuitGame);
    }

    public void StartGame()
    {
        Debug.Log("START GAME");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Objects/Ui_Generic");
        LevelManager.Get().LoadNextScene();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
