using System;
using System.Collections.Generic;
using GGDataBase;
using GGDataUnity;
using UnityEngine;

namespace GGLoad
{
    [DefaultExecutionOrder(-500)]
    public class ModulesInitialization : SystemMono<IModulesInitializationListener>
    {
        #region VARIABLES

        [SerializeField] private DataConfigSOModule[] _moduleData;

        // We store he init data from modules to later broadcast to any listeners
        private readonly List<DataModuleInitialization> _initData = new List<DataModuleInitialization>();
        
        #endregion VARIABLES


        #region INITIALIZATION

        private void Awake()
        {
            // Instantiate modules object
            foreach (DataConfigSOModule data in _moduleData)
            {
                Modules.SetModule(data.Install(new Action<DataModuleInitialization>[] {OnModuleInit}));
            }
        }

        private void OnModuleInit(DataModuleInitialization data)
        {
            _initData.Add(data);
        }

        #endregion INITIALIZATION


        #region REGISTRATION

        public override void AddComponent(IModulesInitializationListener component)
        {
            base.AddComponent(component);
            
            foreach (DataModuleInitialization data in _initData)
            {
                component.OnModuleInitialized(data);
            }
        }

        #endregion REGISTRATION
    }
}
