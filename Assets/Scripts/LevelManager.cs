using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : Manager<LevelManager>
{
    [SerializeField][Tooltip("nome della traccia da inserire")] private String _eventName;

    private long _currentSteps;       
    private FMOD.Studio.EventInstance instance;
    GameManager gameManager;

    public UnityEvent _updateStepsEvent = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Get();
        _currentSteps = 0;

        _updateStepsEvent.AddListener(UpdateStepsEvenet);
        //qui parte la sountrack del gioco
        //sostituire con una traccia più lunga
        StartSountrack(_eventName);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Reset"))
            ResetCurrentScene();
        //Solo per test, Da rimuore in build
        if(Input.GetKeyDown(KeyCode.E))
            LoadNextScene();
        if(Input.GetKeyDown(KeyCode.R))
            ResetCurrentScene();
    }
    
    //azzera numero di passi
    //carica scena successiva
    private void LoadNextScene()
    {
        ResetCurrentSteps();
        StopSountrack();
        gameManager._currentSceneIndex++;
        SceneManager.LoadScene("Scena"+ gameManager._currentSceneIndex);           
    }

    //azzera numero di passi
    //ricarica la scena corrente
    public void ResetCurrentScene()
    {
        ResetCurrentSteps();
        StopSountrack();
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

    private void StartSountrack(String eventName)
    {
        if(string.IsNullOrEmpty(eventName))
        {
            Debug.LogError("Set event name in the editor", this);
            return;
        }
       
        instance = FMODUnity.RuntimeManager.CreateInstance("event:/"+eventName);
        instance.start();
    }

    private void StopSountrack()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }
}

