using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : Manager<LevelManager>
{
    private long _currentSteps;
    private int _currentSceneIndex;
    public UnityEvent _updateStepsEvent = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        _currentSteps = 0;
        _currentSceneIndex = 1;
        _updateStepsEvent.AddListener(UpdateStepsEvenet);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Reset"))
            ResetCurrentScene();
    }
    
    //azzera numero di passi
    //carica scena successiva
    private void LoadNextScene()
    {
        ResetCurrentSteps();
        _currentSceneIndex++;
        SceneManager.LoadScene("Scena"+_currentSceneIndex, LoadSceneMode.Additive);           
    }

    //azzera numero di passi
    //ricarica la scena corrente
    private void ResetCurrentScene()
    {
        ResetCurrentSteps();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetCurrentSteps()
    {
        _currentSteps = 0;
    }

    public void UpdateStepsEvenet()
    {
        _currentSteps++;
        //Output message to the console
        Debug.Log("passi aumentati");
    }

    //fornisce il numero corrente di passi
    public long RetrieveCurrentSteps()
    {
        return _currentSteps;
    }
}

