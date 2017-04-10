using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Services;
using Autofac;
using XamlingCore.Portable.Contract.Downloaders;
using System.Reflection;
using System.Xml.Serialization;
using Arlo.SDK.Contract;
using Arlo.SDK.Impl;
using Arlo.SDK.Repo;
using Arlo.SDK.Repo.Base;
using Arlo.SDK.Services.System;
using XamlingCore.Portable.Contract.Serialise;
using XamlingCore.Portable.Model.Contract;

namespace Arlo.SDK.Glue
{
    public class SdkModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(SdkModule).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Repo"))
                .AsImplementedInterfaces()
                .SingleInstance().PropertiesAutowired();
            
            builder.RegisterGeneric(typeof(ArloRepoBase<>))
                .As(typeof(IArloRepoBase<>));

               builder.RegisterGeneric(typeof(GenericWebRepo<>))
              .As(typeof(IGenericWebRepo<>))

              .InstancePerLifetimeScope();

            builder.RegisterType<TransferConfigService>().As<IHttpTransferConfigService>().SingleInstance();

            builder.RegisterType<XmlEntitySerialiser>().As<IEntitySerialiser>().SingleInstance();

            base.Load(builder);
        }
    }
}
