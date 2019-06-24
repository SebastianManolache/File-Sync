using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcProject.Windsor
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly WindsorContainer container;

        public WindsorControllerFactory(WindsorContainer container) => this.container = container;

        protected override IController GetControllerInstance(RequestContext context, Type controllerType)
        {
            try
            {
                return container.Resolve(controllerType) as IController;
            }
            catch
            {
                context.HttpContext.RewritePath("/");
                controllerType = GetControllerType(context, "Error");
                context.RouteData.Values["controller"] = "Error";
                context.RouteData.Values["action"] = "NotFound";
                context.RouteData.Values["area"] = "";
                return base.GetControllerInstance(context, controllerType);
            }
        }
    }
}