using System.Collections;

namespace GGLoad
{
    /// <summary>
    /// This can be implemented by a project class and attached to the project loader prefab to provide
    /// project-specific loading functionality within the startup scene. Do your loading business and then return,
    /// at which point StartupSceneRedirect will take over and load the first game scene.
    /// </summary>
    public interface IProjectStartupLoader
    {
        /// <summary>
        /// Returns when the project loader has determined that the first game scene is ready to be loaded.
        /// </summary>
        IEnumerator LoadStartup();
    }
}