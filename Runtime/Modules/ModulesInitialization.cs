using GG.Data.Unity;
using UnityEngine;

namespace GG.Load
{
    [DefaultExecutionOrder(-500)]
    internal class ModulesInitialization : MonoBehaviour
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
                Modules.SetModule(data.Install());
            }
        }

        #endregion INITIALIZATION
    }
}
