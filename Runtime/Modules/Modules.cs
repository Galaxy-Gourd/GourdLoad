using System.Collections.Generic;
using GG.Data.Base;
using UnityEngine;

namespace GGLoad
{
    public static class Modules 
    {
        #region VARIABLES

        private static readonly List<IModule> _set = new List<IModule>();

        #endregion VARIABLES
        
        
        #region MODULES

        internal static void SetModule(IModule module)
        {
            _set.Add(module);
        }

        public static T Get<T>() where T : class, IModule
        {
            T result = default(T);
            foreach (IModule module in _set)
            {
                if (module is T modInterface)
                {
                    result = modInterface;
                    break;
                }
            }

            return result;
        }
        
        public static T Get<T>(string id) where T : class, IModule
        {
            T result = default(T);
            foreach (IModule module in _set)
            {
                if (module is T modInterface && module.ID == id)
                {
                    result = modInterface;
                    break;
                }
            }

            return result;
        }

        #endregion MODULES
        
        
        #region RESET

        /// <summary>
        /// Resets static values to prevent issues related to domain reloading
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            _set.Clear();
        }

        #endregion RESET
    }
}
