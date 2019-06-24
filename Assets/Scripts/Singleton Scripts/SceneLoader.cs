using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    [Space(order = 0), Header("Scenes build index", order = 1)]
    [SerializeField, Tooltip("Build index of the main menu scene.")]
    private int startingSceneIndex = 2;

    [SerializeField, Tooltip("Build index of the loading scene.")]
    private int loadingSceneIndex = 1;
    
    [SerializeField, Tooltip("Build index of the gameplay scene.")]
    private int gameSceneIndex = 3;


    private int activeSceneIndex = 0;


    [SerializeField]
    private float delayBetweenScenes = 1.5f;


    public Action OnSceneLoaded;
    public Action OnGameSceneLoaded;



    public int StartingSceneIndex { get { return startingSceneIndex; } }
    public int LoadingSceneIndex { get { return loadingSceneIndex; } }
    public int GameSceneIndex { get { return gameSceneIndex; } }
    public int ActiveSceneIndex { get { return activeSceneIndex; } }



    private void Start()
    {
        // checks if there is only one scene loaded and loads the starting scene (main menu)
        if (SceneManager.sceneCount <= 1)
        {
            StartCoroutine(WaitLoadScene());
        }
    }

    public void LoadSceneAsync(int sceneToLoadIndex)
    {
        Debug.Assert(sceneToLoadIndex > 0 && sceneToLoadIndex < SceneManager.sceneCountInBuildSettings);

        StartCoroutine(WaitLoadSceneAsync(sceneToLoadIndex));
    }

    // load scene async
    private IEnumerator WaitLoadSceneAsync(int sceneIndex)
    {
        // stop input update
        InputManager.Instance.Activate(false);


        // load loading scene and activate it
        AsyncOperation loadAsyncOperation = SceneManager.LoadSceneAsync(loadingSceneIndex, LoadSceneMode.Additive);

        loadAsyncOperation.allowSceneActivation = true;
        Scene scena = SceneManager.GetSceneByBuildIndex(loadingSceneIndex);
        scena.GetRootGameObjects();

        yield return new WaitUntil(() => loadAsyncOperation.isDone);

        yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(loadingSceneIndex));

        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == loadingSceneIndex);


        // unload current scene
        AsyncOperation unloadAsyncOperation = SceneManager.UnloadSceneAsync(activeSceneIndex);

        activeSceneIndex = loadingSceneIndex;

        yield return new WaitUntil(() => unloadAsyncOperation.isDone);


        // load next scene
        loadAsyncOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        loadAsyncOperation.allowSceneActivation = false;

        yield return new WaitUntil(() => loadAsyncOperation.progress >= 0.9f);

        yield return new WaitForSeconds(delayBetweenScenes);


        // activate input update and wait for the player input
        InputManager.Instance.Activate(true);

        OnSceneLoaded?.Invoke();

        yield return new WaitUntil(() => InputManager.Instance.ActionButtonP1);

        loadAsyncOperation.allowSceneActivation = true;

        yield return new WaitForSeconds(delayBetweenScenes);

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));

        activeSceneIndex = sceneIndex;

        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == activeSceneIndex);


        // check if the current loaded scene is game scene and launch a message
        if (activeSceneIndex == gameSceneIndex)
        {
            OnGameSceneLoaded?.Invoke();
        }

        // unload loading scene
        SceneManager.UnloadSceneAsync(loadingSceneIndex);
    }


    // loading start scene
    private IEnumerator WaitLoadScene()
    {
        SceneManager.LoadScene(startingSceneIndex, LoadSceneMode.Additive);

        yield return null;
        yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(startingSceneIndex));

        activeSceneIndex = startingSceneIndex;
    }


    // register a method to an event
    public void RegisterOnEventAction(Action actionEvent, Action actionToRegister)
    {
        actionEvent += actionToRegister;
    }

    // unregister a method to an event
    public void UnregisterOnEventAction(Action actionEvent, Action actionToRegister)
    {
        actionEvent -= actionToRegister;
    }
}
