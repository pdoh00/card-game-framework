using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Game.OFCP;
using Game.OFCP.EventHandlers;
using Game.OFCP.GameCommandHandlers;
using Game.OFCP.Games;
using Game.OFCP.TableCommandHandlers;
using Game.OFCP.TableCommands;
using Infrastructure;
using OFCP.Server.Hubs;
using SignalR;
using SignalR.Autofac;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using Game.OFCP.GameCommands;
using Game.OFCP.Events;
using SignalR.Hubs;

//using SignalR.Hosting.AspNet;
//using SignalR.Infrastructure;

namespace OFCP.Server
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //boot strap
            var container = Bootstrap();
            var commandBus = container.Resolve<ICommandBus>();
            //TODO: This is just to create the 1 table for now.  eventually the table should be created and
            //posted in the lobby.
            commandBus.Send(new CreateNewTableCommand(Game.OFCP.Games.OFCP_Game.OFCP_GAME_TYPE));

            //var customResolver = new SignalR.Autofac.AutofacDependencyResolver(container);
            GlobalHost.DependencyResolver = new SignalR.Autofac.AutofacDependencyResolver(container);
            RouteTable.Routes.MapHubs(new SignalR.Autofac.AutofacDependencyResolver(container));

            //mvc registrations
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static IContainer Bootstrap()
        {
            //get builder
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            //This line is key.  if you change the DependencyResolver you also have
            //to tell it how to resolve the hub.
            builder.RegisterType<PokerServer>().InstancePerLifetimeScope();

            builder.RegisterType<MemoryTableProjection>()
                .As<ITableProjection>()
                .SingleInstance();

            builder.RegisterType<MemoryPlayerProjection>()
                .As<IPlayerProjection>()
                .PropertiesAutowired(PropertyWiringFlags.AllowCircularDependencies)
                .SingleInstance();

            builder.RegisterType<MemoryCommandBus>()
                .AsImplementedInterfaces()
                .SingleInstance();
            
            builder.RegisterType<MemoryEventBus>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<MemoryEventStore>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<TableCommandHandler>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<GameCommandHandler>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<TableEventHandler>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<GameEventHandler>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<SignalRClientChannel>()
                .As<IClientChannel>()
                .SingleInstance();

            builder.RegisterType<Repository<Table>>()
                .As<IRepository<Table>>();

            builder.RegisterType<Repository<OFCP_Game>>()
                .As<IRepository<OFCP_Game>>();

            builder.RegisterType<SignalRPlayerConnectionMap>()
                .As<IPlayerConnectionMap>()
                .SingleInstance();

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<PokerServer>();
            builder.Register<IHubContext>(c => hubContext);

            var container = builder.Build();
            return container;
        }
    }
}