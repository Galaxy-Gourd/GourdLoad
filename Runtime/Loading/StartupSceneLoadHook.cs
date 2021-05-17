using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GG.Load
{
    public class StartupSceneLoadHook : MonoBehaviour
    {
        #region VARIABLES

        [Header("References")] 
        [SerializeField] private List<GameObject> _persistentSpawns;

        /// <summary>
        /// The path in the assets folder pointing to the load hook prefab; this must be present in project
        /// in order to redirect to the game scene.
        /// </summary>
        private const string CONST_LoadingObjectResourcesPath = "Loading/PF_ProjectLoadHook";

        #endregion VARIABLES
        
        
        #region INITIALIZATION

        private void Awake()
        {
            SpawnPersistentObjects();
            LoadProjectLoadHook();
        }

        #endregion INITIALIZATION


        #region LOAD

        private void SpawnPersistentObjects()
        {
            foreach (GameObject obj in _persistentSpawns)
            {
                GameObject g = Instantiate(obj);
                DontDestroyOnLoad(g);
            }
        }
        
        private void LoadProjectLoadHook()
        {
            // Instantiate the loading object if it has been defined in the project
            GameObject hookObj = Resources.Load<GameObject>(CONST_LoadingObjectResourcesPath);
            if (hookObj != null)
            {
                GameObject loader = Instantiate(hookObj);
                if (loader.TryGetComponent(out IProjectStartupLoader startup))
                {
                    StartCoroutine(nameof(StartupSceneLoadTime), startup);
                    return;
                }
            }
            
            // If we get this far, we did not find a load hook in the assets folder - continue loading game scene
            LoadGameSceneFromStartupScene();
        }

        /// <summary>
        /// Routes loading through the project-specific instance of IProjectStartupLoader; this allows for
        /// project-specfiic loading behavior whilst maintaining the core loading functionality of this package
        /// </summary>
        private IEnumerator StartupSceneLoadTime(IProjectStartupLoader loader)
        {
            // Wait until the project-specific load process is complete
            yield return loader.LoadStartup();
            
            LoadGameSceneFromStartupScene();
        }

        private static void LoadGameSceneFromStartupScene()
        {
            // Load next scene
            SceneManager.LoadScene(StartupSceneRedirect.TargetLoadScene);
        }

        #endregion LOAD
    }
}