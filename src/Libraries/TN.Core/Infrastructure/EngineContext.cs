using System.Runtime.CompilerServices;

namespace TN.Core.Infrastructure
{
    /// <summary>
    /// 提供对引擎的单例实例的访问
    /// </summary>
    public class EngineContext
    {
        #region Methods

        /// <summary>
        /// 创建引擎的静态实例
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Create()
        {
            return Singleton<IEngine>.Instance ?? (Singleton<IEngine>.Instance = new NcEngine());
        }

        /// <summary>
        /// 将静态引擎实例设置为提供的引擎。使用此方法提供自定义的引擎实现。
        /// </summary>
        /// <param name="engine"></param>
        /// <remarks></remarks>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// 获取用于访问服务的单例引擎
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Create();
                }

                return Singleton<IEngine>.Instance;
            }
        }

        #endregion Properties
    }
}