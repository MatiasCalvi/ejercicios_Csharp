using Datos;
using Datos.Interfaces;
using Ninject;

namespace Daos
{
    public static class NinjectConfig
    {
        public static IKernel CreateKernel()
        {
            IKernel kernel = new StandardKernel();

            kernel.Bind<IDaoBD>().To<DaoBD>();

            return kernel;
        }
    }
}

