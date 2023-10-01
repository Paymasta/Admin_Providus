using PayMasta.Service.Account;
using PayMasta.Service.Article;
using PayMasta.Service.Employer;
using PayMasta.Service.Home;
using PayMasta.Service.ManageCategory;
using PayMasta.Service.ManageCms;
using PayMasta.Service.ManageNotifications;
using PayMasta.Service.Support;
using PayMasta.Service.ThirdParty;
using PayMasta.Service.Transactions;
using PayMasta.Service.UpdateProfileRequest;
using PayMasta.Service.User;
using PayMasta.Service.Withdrawals;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;
using Unity.Mvc5;

namespace PayMasta.Admin
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            container.RegisterType<IAccountService, AccountService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserService, UserService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmployerService, EmployerService>(new HierarchicalLifetimeManager());
            container.RegisterType<IDashboardService, DashboardService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUpdateProfileRequestService, UpdateProfileRequestService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISupportService, SupportService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITransactionsService, TransactionsService>(new HierarchicalLifetimeManager());
            container.RegisterType<IWithdrawalsService, WithdrawalsService>(new HierarchicalLifetimeManager());
            container.RegisterType<IThirdParty, ThirdPartyService>(new HierarchicalLifetimeManager());
            container.RegisterType<IManageCategoryService, ManageCategoryService>(new HierarchicalLifetimeManager());
            container.RegisterType<IManageNotificationsService, ManageNotificationsService>(new HierarchicalLifetimeManager());
            container.RegisterType<IManageCmsService, ManageCmsService>(new HierarchicalLifetimeManager());
            container.RegisterType<IArticleService, ArticleService>(new HierarchicalLifetimeManager());
            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}