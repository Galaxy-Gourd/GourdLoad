using UnityEngine;
using UnityEngine.SceneManagement;

namespace GG.Load
{
    /// <summary>
    /// Entry point for project loading - LoadStart uses the
    /// [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] call to auto-hook
    /// into Unity load process and redirect into the startup scene.
    /// </summary>
    public static class StartupSceneRedirect
    {
        #region Variables

        /// <summary>
        /// We use this to store the scene we need to load after startup
        /// </summary>
        public static int TargetLoadScene { get; private set; }

        /// <summary>
        /// Scenes appended with this prefix will NOT trigger the startup scene
        /// </summary>
        private const string CONST_TestSceneNamePrefix = "_Test";

        #endregion Variables


        #region Load

        /// <summary>
        /// Initializes the project loading process
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BeforeSceneLoad()
        {
            Scene sourceScene = SceneManager.GetActiveScene();
            if (!sourceScene.name.Contains(CONST_TestSceneNamePrefix))
            {
                LoadStartupScene(sourceScene.buildIndex);
            }
            else
            {
                Debug.Log("Test scene detected; skipping start");
            }
        }
        
        /// <summary>
        /// Loads the startup scene, which will load the core systems in turn.
        /// </summary>
        private static void LoadStartupScene(int currentScene)
        {
            if (currentScene != 0)
            {
                TargetLoadScene = currentScene;
                SceneManager.LoadScene(0);
                return;
            }
            
            // If we started from the startup scene, fallback to auto-loading scene 1 (startup scene should be scene 0)
            TargetLoadScene = 1;
        }

        #endregion Load
    }
}