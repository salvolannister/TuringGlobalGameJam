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
    [SerializeField] [Tooltip("nome della traccia da inserire")] private String _eventName;

    private long _currentSteps;
    private FMOD.Studio.EventInstance instance;
    GameManager gameManager;

    public Action OnPlayerMove;
    public Action OnLevelFinished;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Get();
        _currentSteps = 0;

        //qui parte la sountrack del gioco
        //sostituire con una traccia più lunga
        StartSountrack(_eventName);

#if UNITY_STANDALONE == false
        var cam = GetComponent<PixelPerfectCamera>();
        if (cam != null)
            cam.enabled = false;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Reset"))
            ResetCurrentScene();
        if (Input.GetKeyDown(KeyCode.R))
            ResetCurrentScene();
    }

    //azzera numero di passi
    //carica scena successiva
    public void LoadNextScene()
    {
        ResetCurrentSteps();
        StopSountrack();
        gameManager._currentSceneIndex++;
        StartCoroutine(StartLoad());
        OnLevelFinished?.Invoke();
    }

    //azzera numero di passi
    //ricarica la scena corrente
    public void ResetCurrentScene()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Objects/Ui_Restart");
        ResetCurrentSteps();
        StopSountrack();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        ResetCurrentSteps();
        StopSountrack();
        StartCoroutine(RestartLoad());
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
        OnPlayerMove?.Invoke();
    }

    //fornisce il numero corrente di passi
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

        instance = FMODUnity.RuntimeManager.CreateInstance("event:/" + eventName);
        instance.start();
    }

    private void StopSountrack()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }

    IEnumerator RestartLoad()
    {
        yield return new WaitForSeconds(1);
        _loadingScreen.gameObject.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 1));
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        while (!operation.isDone)
        {
            yield return null;
        }
        yield return StartCoroutine(FadeLoadingScreen(0, 1));
        yield return new WaitForSeconds(3);
        _loadingScreen.gameObject.SetActive(false);
    }
    IEnumerator StartLoad()
    {
        _loadingScreen.gameObject.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 1));
        AsyncOperation operation = SceneManager.LoadSceneAsync("Level" + gameManager._currentSceneIndex);
        while (!operation.isDone)
        {
            yield return null;
        }
        yield return StartCoroutine(FadeLoadingScreen(0, 1));
        yield return new WaitForSeconds(3);
        _loadingScreen.gameObject.SetActive(false);
    }
    IEnumerator FadeLoadingScreen(float targetValue, float duration)
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

