using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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
        private const string CONST_LoadHookPrefabAddress = "ProjectLoadHook";

        #endregion VARIABLES
        
        
        #region INITIALIZATION

        private void Awake()
        {
            SpawnPersistentObjects();
            StartCoroutine(nameof(LoadProjectLoadHook));
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
        
        private IEnumerator LoadProjectLoadHook()
        {
            // Load project loader prefab using defined address
            AsyncOperationHandle<GameObject> loadHandle = 
                Addressables.LoadAssetAsync<GameObject>(CONST_LoadHookPrefabAddress);
            yield return loadHandle;
            
            if(loadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject loader = Instantiate(loadHandle.Result);
                if (loader.TryGetComponent(out IProjectStartupLoader startup))
                {
                    StartCoroutine(nameof(StartupSceneLoadTime), startup);
                }
            }
            else
            {
                // If we get this far, we did not find a load hook - continue loading game scene
                LoadGameSceneFromStartupScene();
            }
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