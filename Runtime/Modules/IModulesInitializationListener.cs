using GGDataBase;

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