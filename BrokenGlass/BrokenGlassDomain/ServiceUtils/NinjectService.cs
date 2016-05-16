using BrokenGlassDomain.DataLayer;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrokenGlassDomain.ServiceUtils
{
    public class NinjectService
    {
        private IKernel m_kernel;
        private static NinjectService m_instance;

        public NinjectService()
        {
            m_kernel = new StandardKernel();
            AddBindings();
        }

        public static NinjectService Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new NinjectService();
                }
                return m_instance;
            }
        }

        public T GetService<T>()
        {
            return m_kernel.Get<T>();
        }

        private void AddBindings()
        {
            m_kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
        }
    }
}
