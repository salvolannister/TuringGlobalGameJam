using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class LevelManager : Manager<LevelManager>
{
    [SerializeField] [Tooltip("Loading page")] private Canvas _loadingScreen;
    [SerializeField] [Tooltip("SoundTrack Name")] private String _eventName;

    private long _currentSteps;
    private FMOD.Studio.EventInstance fmodStudioInstance;
    GameManager gameManager;

    public Action OnPlayerMove;
    public Action OnLevelFinished;

    void Start()
    {
        gameManager = GameManager.Get();
        _currentSteps = 0;

        //Start game soundtrack
        //TODO: add longer soundtrack
        StartSountrack(_eventName);

#if !UNITY_STANDALONE
        var cam = GetComponent<PixelPerfectCamera>();
        if (cam != null)
            cam.enabled = false;
#endif
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ResetCurrentScene();
    }

    /* Set the game environment to load the next scene: 
     * reset steps count, stop soundtrack music, and increase level index
     */
    public void LoadNextScene()
    {
        ResetCurrentSteps();
        StopSountrack();
        gameManager._currentSceneIndex++;
        Debug.LogFormat("LoadNextScene - scene index: " + gameManager._currentSceneIndex);
        StartCoroutine(StartLoadCoroutine());
        OnLevelFinished?.Invoke();
    }

    /* Reload Current Scene */
    public void ResetCurrentScene()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Objects/Ui_Restart");
        gameManager.IncreaseDeathCounter();
        Debug.LogFormat("ResetCurrentScene: " + gameManager._currentPlayerDeath);
        ResetCurrentSteps();
        StopSountrack();
        StartCoroutine(RestartLoadCoroutine());
    }

    public void GameOver()
    {
        gameManager.IncreaseDeathCounter();
        Debug.LogFormat("GameOver: " + gameManager._currentPlayerDeath);
        ResetCurrentSteps();
        StopSountrack();
        StartCoroutine(RestartLoadCoroutine());
    }

    private void ResetCurrentSteps()
    {
        _currentSteps = 0;
    }

    public void UpdateStepsEvent()
    {
        _currentSteps++;
        Debug.Log("Steps Increased");
        OnPlayerMove?.Invoke();
    }

    public long RetrieveCurrentSteps()
    {
        return _currentSteps;
    }

    private void StartSountrack(String eventName)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogWarning("Set event name in the editor", this);
            return;
        }

        fmodStudioInstance = FMODUnity.RuntimeManager.CreateInstance("event:/" + eventName);
        fmodStudioInstance.start();
    }

    private void StopSountrack()
    {
        fmodStudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        fmodStudioInstance.release();
    }

    IEnumerator RestartLoadCoroutine()
    {
        yield return new WaitForSeconds(1);
        _loadingScreen.gameObject.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreenCoroutine(1, 1));
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        while (!operation.isDone)
        {
            yield return null;
        }
        yield return StartCoroutine(FadeLoadingScreenCoroutine(0, 1));
        yield return new WaitForSeconds(3);
        _loadingScreen.gameObject.SetActive(false);
    }
    IEnumerator StartLoadCoroutine()
    {
        _loadingScreen.gameObject.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreenCoroutine(1, 1));
        AsyncOperation operation = SceneManager.LoadSceneAsync("Level" + gameManager._currentSceneIndex);
        while (!operation.isDone)
        {
            yield return null;
        }
        yield return StartCoroutine(FadeLoadingScreenCoroutine(0, 1));
        yield return new WaitForSeconds(3);
        _loadingScreen.gameObject.SetActive(false);
    }
    IEnumerator FadeLoadingScreenCoroutine(float targetValue, float duration)
    {
        float startValue = _loadingScreen.GetComponentInChildren<Image>().color.a;
        float time = 0;
        var tempColor = _loadingScreen.GetComponentInChildren<Image>().color;
        while (time < duration)
        {
            tempColor.a = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        tempColor.a = targetValue;
        _loadingScreen.GetComponentInChildren<Image>().color = tempColor;
    }
}

