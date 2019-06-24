using ClassLibrary1.Managers;
using Data.Interfaces;
using System;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace MvcProject
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IFileLayer, FileLayer>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}