﻿using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

// start level soundtrack
public class LevelManager : Manager<LevelManager>
{
    [SerializeField] [Tooltip("Loading page")] private Canvas _loadingScreen;
    [SerializeField] [Tooltip("SoundTrack Name")] private String _eventName;

    private FMOD.Studio.EventInstance _fmodStudioInstance;
    private GameManager _gameManager;

    public Action OnLevelFinished;

    #region UnityMethods
    void Start()
    {
        _gameManager = GameManager.Get();

        //Start game soundtrack
        //TODO: add longer soundtrack
        StartSountrack(_eventName);

#if !UNITY_STANDALONE
        var cam = GetComponent<PixelPerfectCamera>();
        if (cam != null)
            cam.enabled = false;
#endif

    }

    #endregion
    /* Set the game environment to load the next scene: 
     * reset steps count, stop soundtrack music, and increase level index
     */
    public void LoadNextScene()
    {
        
        StopSountrack();
        _gameManager._currentSceneIndex++;
        Debug.LogFormat("LoadNextScene - scene index: " + _gameManager._currentSceneIndex);
        StartCoroutine(StartLoadCoroutine());
        OnLevelFinished?.Invoke();
    }

    /* Reload Current Scene */
    public void ResetCurrentScene()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Objects/Ui_Restart");
        _gameManager.IncreaseDeathCounter();
        Debug.LogFormat("ResetCurrentScene: " + _gameManager._currentPlayerDeath);
        StopSountrack();
        StartCoroutine(RestartLoadCoroutine());
    }

    public void GameOver()
    {
        _gameManager.IncreaseDeathCounter();
        Debug.LogFormat("GameOver: " + _gameManager._currentPlayerDeath);
        StopSountrack();
        StartCoroutine(RestartLoadCoroutine());
    }

    private void StartSountrack(String eventName)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogWarning("Set event name in the editor", this);
            return;
        }

        _fmodStudioInstance = FMODUnity.RuntimeManager.CreateInstance("event:/" + eventName);
        _fmodStudioInstance.start();
    }

    private void StopSountrack()
    {
        _fmodStudioInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _fmodStudioInstance.release();
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
        AsyncOperation operation = SceneManager.LoadSceneAsync("Level" + _gameManager._currentSceneIndex);
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

