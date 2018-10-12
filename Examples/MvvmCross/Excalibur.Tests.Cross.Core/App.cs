using Excalibur.Tests.Cross.Core.Services;
using System.Collections.Generic;
using System.Reflection;
using Excalibur.Base.Storage;
using Excalibur.Cross.Business;
using Excalibur.Cross.Language;
using Excalibur.Cross.ObjectConverter;
using Excalibur.Cross.Presentation;
using Excalibur.Cross.Services;
using Excalibur.Cross.Storage;
using Excalibur.Tests.Cross.Core.Services.Interfaces;
using Excalibur.Tests.Cross.Core.State;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using MvvmCross;
using Excalibur.Providers.File;
using Excalibur.Tests.Cross.Core.ViewModels;

namespace Excalibur.Tests.Cross.Core
{
    public class App : Excalibur.Cross.ExApp
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            base.Initialize();

            var appStart = Mvx.IoCProvider.ConstructAndRegisterSingleton<IMvxAppStart, AppStart>();

            RegisterTextResources();
            RegisterAppStart(appStart);

            var state = Mvx.IoCProvider.Resolve<IApplicationState>();
            state.InitAndLoadAsync().GetAwaiter().GetResult();
        }

        public override void RegisterDependencies()
        {
            // Application Things
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IApplicationState, ApplicationState>();
            Mvx.IoCProvider.ConstructAndRegisterSingleton<ISyncService, SyncService>();

            // User
            Mvx.IoCProvider.RegisterType<IObjectStorageProvider<int, Domain.LoggedInUser>, ObjectAsFileStorageProvider<int, Domain.LoggedInUser>>();

            Mvx.IoCProvider.RegisterType<IObjectMapper<Domain.LoggedInUser, Observable.LoggedInUser>, BaseObjectMapper<Domain.LoggedInUser, Observable.LoggedInUser>>();
            Mvx.IoCProvider.RegisterType<IObjectMapper<Observable.LoggedInUser, Observable.LoggedInUser>, BaseObjectMapper<Observable.LoggedInUser, Observable.LoggedInUser>>();

            Mvx.IoCProvider.RegisterType<Business.Interfaces.ILoggedInUser, Business.LoggedInUser>();

            Mvx.IoCProvider.RegisterType<IServiceBase<Domain.LoggedInUser>, LoggedInUserService>();

            Mvx.IoCProvider.ConstructAndRegisterSingleton<ISinglePresentation<int, Observable.LoggedInUser>, BaseSinglePresentation<int, Domain.LoggedInUser, Observable.LoggedInUser>>();

            // User
            Mvx.IoCProvider.RegisterType<IObjectStorageProvider<int, Domain.User>, ObjectAsFileStorageProvider<int, Domain.User>>();

            Mvx.IoCProvider.RegisterType<IObjectMapper<Domain.User, Observable.User>, BaseObjectMapper<Domain.User, Observable.User>>();
            Mvx.IoCProvider.RegisterType<IObjectMapper<Observable.User, Observable.User>, BaseObjectMapper<Observable.User, Observable.User>>();

            Mvx.IoCProvider.RegisterType<IListBusiness<int, Domain.User>, BaseListBusiness<int, Domain.User>>();

            Mvx.IoCProvider.RegisterType<IServiceBase<IList<Domain.User>>, UserService>();

            Mvx.IoCProvider.ConstructAndRegisterSingleton<IListPresentation<int, Observable.User, Observable.User>, BaseListPresentation<int, Domain.User, Observable.User, Observable.User>>();

            // Todo
            Mvx.IoCProvider.RegisterType<IObjectStorageProvider<int, Domain.Todo>, ObjectAsFileStorageProvider<int, Domain.Todo>>();

            Mvx.IoCProvider.RegisterType<IObjectMapper<Domain.Todo, Observable.Todo>, BaseObjectMapper<Domain.Todo, Observable.Todo>>();
            Mvx.IoCProvider.RegisterType<IObjectMapper<Observable.Todo, Observable.Todo>, BaseObjectMapper<Observable.Todo, Observable.Todo>>();

            Mvx.IoCProvider.RegisterType<IListBusiness<int, Domain.Todo>, BaseListBusiness<int, Domain.Todo>>();

            Mvx.IoCProvider.RegisterType<IServiceBase<IList<Domain.Todo>>, TodoService>();

            Mvx.IoCProvider.ConstructAndRegisterSingleton<Presentation.Interfaces.ITodo, Presentation.Todo>();
        }

        private void RegisterTextResources()
        {
            var assemblies = new List<Assembly>()
            {
                GetType().GetTypeInfo().Assembly,
            };
            ExTextProvider.InitializeAndCreateBuilder(assemblies, "AppResources/Text", "Excalibur.Tests", "Shared");
        }
    }
}
