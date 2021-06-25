using GG.Data.Base;
using GG.Data.Unity;

namespace GGLoad
{
    public interface IModulesInitializationListener : IComponent
    {
        #region METHODS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        void OnModuleInitialized(DataModuleInitialization data);

        #endregion METHODS
    }
}