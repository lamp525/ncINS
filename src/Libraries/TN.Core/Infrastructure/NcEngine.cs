using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace TN.Core.Infrastructure
{
    public class NcEngine : IEngine
    {
        #region Properties

        public ContainerManager ContainerManager { get; private set; }

        #endregion Properties

        #region Utilities

        /// <summary>
        /// 注册依赖
        /// </summary>
        protected virtual void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            //注册依赖
            var typeFinder = new WinAppTypeFinder();
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();

            //注册其它程序中的依赖
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes)
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));

            //排序
            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in drInstances)
                dependencyRegistrar.Register(builder, typeFinder);

            var container = builder.Build();
            this.ContainerManager = new ContainerManager(container);
        }

        #endregion Utilities

        #region Methods

        public void Initialize()
        {
            RegisterDependencies();
        }

        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion Methods
    }
}