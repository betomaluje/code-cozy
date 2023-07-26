using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseMenu : MonoBehaviour
{
    [SerializeField] protected PlayerInput _playerInput;
    [SerializeField] private bool waitForComplete = true;
    [SerializeField] protected SceneLoader sceneLoader;
    [SerializeField] private Slider loadingProgressBar;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private List<MenuPage> pages;

    [Header("Extras")] 
    [SerializeField] private Transform[] objectsToHide;

    private AsyncOperation _completedAsyncOperation;

    protected virtual void OnEnable()
    {
        sceneLoader.OnLoadingStart += OnLoadingStart;
        sceneLoader.OnLoadingProgress += OnLoadingProgress;
        sceneLoader.OnLoadingComplete += OnLoadingComplete;

        _playerInput.SubmitEvent += TryCompleteLoading;
        _playerInput.CancelEvent += TryCompleteLoading;
        _playerInput.ClickEvent += TryCompleteLoading;
        _playerInput.MovementEvent += TryCompleteLoading;
    }

    protected virtual void OnDisable()
    {
        sceneLoader.OnLoadingStart -= OnLoadingStart;
        sceneLoader.OnLoadingProgress -= OnLoadingProgress;
        sceneLoader.OnLoadingComplete -= OnLoadingComplete;

        _playerInput.SubmitEvent -= TryCompleteLoading;
        _playerInput.CancelEvent -= TryCompleteLoading;
        _playerInput.ClickEvent -= TryCompleteLoading;
        _playerInput.MovementEvent -= TryCompleteLoading;
    }
    
    protected void ShowPage(Type page)
    {
        foreach (var item in pages)
        {
            item.PageObject.gameObject.SetActive(page == item.GetType());
        }
    }

    protected void HidePage(Type page)
    {
        pages.FirstOrDefault(item => page == item.GetType())?.PageObject.gameObject.SetActive(false);
    }

    private void TryCompleteLoading(Vector2 movement)
    {
        TryCompleteLoading();
    }

    private void TryCompleteLoading()
    {
        if (_completedAsyncOperation == null || !waitForComplete ||
            !(Math.Abs(loadingProgressBar.value - 1) < .1f)) return;
        
        HidePage(typeof(LoadingPage));
        _completedAsyncOperation.allowSceneActivation = true;
    }

    private void OnLoadingStart()
    {
        foreach (var item in objectsToHide)
        {
            item.gameObject.SetActive(false);
        }

        ShowPage(typeof(LoadingPage));
        loadingText.text = "LOADING...";
    }

    private void OnLoadingProgress(float progress)
    {
        loadingProgressBar.value = progress;
    }

    private void OnLoadingComplete(AsyncOperation sceneAO)
    {
        loadingProgressBar.value = 1f;

        _completedAsyncOperation = sceneAO;

        if (waitForComplete)
        {
            loadingText.text = "PRESS ANY KEY TO CONTINUE";
        }
        else
        {
            sceneAO.allowSceneActivation = true;
        }
    }
}