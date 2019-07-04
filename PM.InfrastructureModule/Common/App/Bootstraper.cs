using JetBrains.Annotations;
using Unity;

namespace PM.InfrastructureModule.Common.App
{
    /// <summary>
    /// Init Unity
    /// </summary>
    [UsedImplicitly]
    public class Bootstraper
    {
        /// <summary>
        /// Return Instance
        /// </summary>
        public static UnityContainer Init()
        {
            var unityContainer = new UnityContainer();
            return unityContainer;
        }
    }
}