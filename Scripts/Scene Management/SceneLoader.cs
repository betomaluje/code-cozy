using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Action OnLoadingStart;
    public Action<float> OnLoadingProgress;
    public Action<AsyncOperation> OnLoadingComplete;

    // the actual percentage while scene is fully loaded
    private const float LOAD_READY_PERCENTAGE = 0.9f;

    private AsyncOperation sceneAO;

    #region SCENE LOADING

    public void RestartCurrentScene()
    {
        OnLoadingStart?.Invoke();
        var sceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(DoRestartScene(sceneName));
    }

    private IEnumerator DoRestartScene(string sceneName)
    {
        yield return UnloadingSceneRealProgress(sceneName);
        StartCoroutine(LoadingSceneRealProgress(sceneName));
    }
    
    public void ChangeScene(string sceneName)
    {
        OnLoadingStart?.Invoke();
        StartCoroutine(LoadingSceneRealProgress(sceneName));
    }
    
    private IEnumerator UnloadingSceneRealProgress(string sceneName)
    {
        yield return SceneManager.UnloadSceneAsync(sceneName);
    }

    private IEnumerator LoadingSceneRealProgress(string sceneName)
    {
        yield return new WaitForSecondsRealtime(1);
        sceneAO = SceneManager.LoadSceneAsync(sceneName);

        // disable scene activation while loading to prevent auto load
        sceneAO.allowSceneActivation = false;

        while (!sceneAO.isDone)
        {
            OnLoadingProgress?.Invoke(sceneAO.progress);

            if (sceneAO.progress >= LOAD_READY_PERCENTAGE)
            {
                OnLoadingComplete?.Invoke(sceneAO);
                Time.timeScale = 1;
            }

            yield return null;
        }
    }
    #endregion
}
