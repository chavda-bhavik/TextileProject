using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Repository.Pattern.DataContext;
using Pal.Entities.Models;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using Pal.Services;
using PalWeb.Controllers;

namespace PalWeb.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container

                .RegisterType<IDataContextAsync, PalContext>(new PerRequestLifetimeManager())
                .RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerRequestLifetimeManager())
				 .RegisterType<IRepositoryAsync<AuditLog>, Repository<AuditLog>>(new PerRequestLifetimeManager())
				 .RegisterType<IRepositoryAsync<tblLists>, Repository<tblLists>>(new PerRequestLifetimeManager())
                 .RegisterType<IRepositoryAsync<JobworkParty>, Repository<JobworkParty>>(new PerRequestLifetimeManager())

                 .RegisterType<IAuditLogService, AuditLogService>(new PerRequestLifetimeManager())
				 .RegisterType<IJobworkPartyService, JobworkPartyService>(new PerRequestLifetimeManager())
				 
			//.RegisterType<IRegisterServices, RegisterServices>(new PerRequestLifetimeManager())
			;

			container.RegisterType<AccountController>(
				new InjectionConstructor());
			// container.RegisterType<RolesAdminController>(new InjectionConstructor());
			container.RegisterType<RolesAdminController>(new InjectionConstructor());
			container.RegisterType<UsersAdminController>(new InjectionConstructor());
		}
    }
}
