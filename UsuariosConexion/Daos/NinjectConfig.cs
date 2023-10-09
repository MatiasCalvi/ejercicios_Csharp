using Ninject;

namespace Daos
{
    public static class NinjectConfig
    {
        public static IKernel CreateKernel()
        {
            IKernel kernel = new StandardKernel();

            kernel.Bind<IRepositorioUsuarios>().To<RepositorioUsuarios>();

            return kernel;
        }
    }
}

