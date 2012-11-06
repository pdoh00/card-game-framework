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
            MemoryTableProjection tableProjection = new MemoryTableProjection();
            MemoryPlayerProjection playerProjection = new MemoryPlayerProjection();
            MemoryEventBus eventBus = new MemoryEventBus();
            MemoryCommandBus commandBus = new MemoryCommandBus();
            MemoryEventStore eventStore = new MemoryEventStore(eventBus);

            Repository<Table> tableRepository = new Repository<Table>(eventStore);
            Repository<OFCP_Game> gameRepository = new Repository<OFCP_Game>(eventStore);
            TableCommandHandler tableCommandHandler = new TableCommandHandler(tableRepository);
            GameCommandHandler gameCommandHandler = new GameCommandHandler(tableProjection, playerProjection, gameRepository);

            var hubCtx = GlobalHost.ConnectionManager.GetHubContext<PokerServer>();
            ClientChannel channel = new ClientChannel(hubCtx);
            GameEventHandler gameEventHandler = new GameEventHandler(channel); //needs IClientChannel
            TableEventHandler tableEventHandler = new TableEventHandler(channel, tableProjection, commandBus, playerProjection);

            commandBus.Register(tableCommandHandler);
            commandBus.Register(gameCommandHandler);

            eventBus.Register(gameEventHandler);
            eventBus.Register(tableEventHandler);
            //end bootstrap

            //TODO: This is just to create the 1 table for now.  eventually the table should be created and
            //posted in the lobby.
            commandBus.Send(new CreateNewTableCommand(Game.OFCP.Games.OFCP_Game.OFCP_GAME_TYPE));

            //get builder
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            //register instances
            builder.RegisterInstance<ITableProjection>(tableProjection).SingleInstance();
            builder.RegisterInstance<IPlayerProjection>(playerProjection).SingleInstance();
            builder.RegisterInstance<ICommandBus>(commandBus).SingleInstance();

            //set up container and resolvers
            var container = builder.Build();
            DependencyResolver.SetResolver(new Autofac.Integration.Mvc.AutofacDependencyResolver(container));
            GlobalHost.DependencyResolver = new SignalR.Autofac.AutofacDependencyResolver(container);
            RouteTable.Routes.MapHubs();


            //mvc registrations
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}