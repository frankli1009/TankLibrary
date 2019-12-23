using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TankLibrary.Domain.Abstract;
using TankLibrary.Domain.Concrete;

namespace TankLibrary.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelP)
        {
            kernel = kernelP;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            throw new NotImplementedException();
        }
        private void AddBindings()
        {
            // my bindings go here

            // mock binding
            //Mock<ITankRepository> mock = new Mock<ITankRepository>();
            //mock.Setup(m => m.Tanks).Returns(new List<Tank>
            //{
            //    new Tank { Id = 1, Name = "M1 Abram", Stage=1, Weight = 54 },
            //    new Tank { Id = 2, Name = "M1A1 Abram", Stage=2, Weight = 57 },
            //    new Tank { Id = 3, Name = "M11 Abram", Stage=3, Weight = 64 },
            //    new Tank { Id = 4, Name = "M1A3 Abram", Stage=2, Weight = 67 },
            //    new Tank { Id = 5, Name = "M12 Abram", Stage=3, Weight = 44 },
            //    new Tank { Id = 6, Name = "M1A4 Abram", Stage=4, Weight = 47 },
            //    new Tank { Id = 7, Name = "M13 Abram", Stage=2, Weight = 34 },
            //    new Tank { Id = 8, Name = "M1A5 Abram", Stage=4, Weight = 37 },
            //    new Tank { Id = 9, Name = "M14 Abram", Stage=3, Weight = 74 },
            //    new Tank { Id = 10, Name = "M1A6 Abram", Stage=4, Weight = 77 },
            //    new Tank { Id = 11, Name = "M15 Abram", Stage=4, Weight = 59 },
            //    new Tank { Id = 12, Name = "M1A7 Abram", Stage=2, Weight = 63 },
            //    new Tank { Id = 13, Name = "M1A2 Abram", Stage=4, Weight = 65 }
            //});

            //kernel.Bind<ITankRepository>().ToConstant(mock.Object);

            // real binding to dbcontext
            kernel.Bind<ITankRepository>().To<TankLibTankRepository>();
        }
    }
}