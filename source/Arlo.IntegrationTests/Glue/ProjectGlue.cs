using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Glue;
using Arlo.SDK.Services;
using Autofac;
using AutoMapper;
using EventBot.SupportLibrary;
using Microsoft.ApplicationInsights.DataContracts;
using SearchIndexer.SupportLibrary;
using Xamling.Azure.Glue;
using XamlingCore.NET.Glue;
using XamlingCore.Portable.Contract.Config;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Portable.Glue;
using XamlingCore.Portable.Model.Other;
using WebConfig = Arlo.IntegrationTests.Impl.WebConfig;


namespace Arlo.IntegrationTests.Glue
{
    public class ProjectGlue : GlueBase
    {
        public IContainer Container { get; private set; }

        public IContainer Init()
        {
            base.Init();

            Builder.RegisterModule<DefaultXCoreModule>();
            Builder.RegisterModule<DefaultNETModule>();
            Builder.RegisterModule<AzureModule_NonWeb>();
            Builder.RegisterModule<SdkModule>();
            Builder.RegisterModule<SupportModule>();

            Builder.RegisterType<ArloSdkSearchIndexService>().As<IArloSdkSearchIndexService>();
            Builder.RegisterType<AzureSearchIndexingService>().As<IAzureSearchIndexingService>();

            Builder.RegisterType<WebConfig>().As<IConfig>();

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<XSeverityLevel, SeverityLevel>();
            });

            Container = Builder.Build();
            ContainerHost.Container = Container;

            return Container;
        }
    }
}
