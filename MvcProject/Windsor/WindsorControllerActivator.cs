using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace MvcProject.Windsor
{

    public class WindsorControllerActivator : IHttpControllerActivator
    {
        private readonly IWindsorContainer container;

        public WindsorControllerActivator(IWindsorContainer container) => this.container = container;

        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            var controller = (IHttpController)container.Resolve(controllerType);
            return controller;
        }
    }
}