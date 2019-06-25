using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Data.Interfaces;
using Data.Managers;

namespace MvcProject.Windsor
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
           // container.Register(Component.For<FileDbContext>().LifestyleTransient());
            container.Register(Component.For<IFileLayer>().ImplementedBy<FileLayer>().LifestyleTransient());
        }
    }
}