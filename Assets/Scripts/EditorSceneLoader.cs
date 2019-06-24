#if UNITY_EDITOR
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.Callbacks;

/// <summary>
/// On Unity Editor play mode loads first build index scene if is not already loaded.
/// </summary>
public static class EditorSceneLoader
{
    /// <summary>
    /// If scene with build index 0 is not already loaded, loads it in additive mode.
    /// </summary>
    [PostProcessScene]
    public static void OnPostprocessScene()
    {
        if (BuildPipeline.isBuildingPlayer == false)
        {
            // check if the scene with build index zero is already loaded
            for (int i = 0; i < SceneManager.sceneCount; i += 1)
            {
                if (SceneManager.GetSceneAt(i).buildIndex == 0)
                {
                    return;
                }
            }

            // load scene with build index zero in additive mode
            SceneManager.LoadScene(0, LoadSceneMode.Additive);
        }
    }
}
#endif