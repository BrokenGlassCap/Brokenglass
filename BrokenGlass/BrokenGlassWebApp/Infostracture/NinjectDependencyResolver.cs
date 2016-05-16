using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrokenGlassWebApp.Infostracture
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel m_kernel;
        public NinjectDependencyResolver()
        {
            m_kernel = new StandardKernel();
            AddBindings();
        }

        public NinjectDependencyResolver(IKernel kernel)
        {
            m_kernel = kernel;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return m_kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return m_kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {

        }
    }
}