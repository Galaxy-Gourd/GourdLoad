using System;
using GGDataBase;
using GGDataUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GGLoad
{
    [DefaultExecutionOrder(-500)]
    public class ModulesInstaller : MonoBehaviour
    {
        #region VARIABLES

        [SerializeField] private DataConfigSOModule[] _moduleData;

        #endregion VARIABLES


        #region INITIALIZATION

        private void Awake()
        {
            // Instantiate modules object
            foreach (DataConfigSOModule data in _moduleData)
            {
                data.Install(new Action<DataModuleInitialization>[] {Modules.OnModuleInit});
            }
            
            Modules.ModuleLoadComplete();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        #endregion INITIALIZATION
        

        #region SCENE

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Modules.ClearModules();
        }

        #endregion SCENE
    }
}
