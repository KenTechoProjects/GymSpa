using APICore.Application.GymSpa.Connectors;
using APICore.Application.GymSpa.Services;
using APICore.Application.Member.Interface;
using APICore.Application.Member.Service;
using APICore.Application.NightLife.Interface;
using APICore.Application.NightLife.Service;
using APICore.Application.Superadmin.Interface;
using APICore.Application.Superadmin.Service;
using APICore.Application.SuperMember.Interface;
using APICore.Application.SuperMember.Service;
using APICore.Application.Vendor.Interface;
using APICore.Application.Vendor.Service;
using AppCore.Application.TransactionNotification;
using AppCore.Application.TransactionNotification.Service;
using Domain.Application.GymSpa.DTO;
using Microsoft.Extensions.DependencyInjection;
using Notification.Interface;
using Notification.Service;
using RestSharp;
using SharedService.Interface;
using SharedService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Interface;
using Utilities.Service;
using Utilities.SystemActivity.Interface;
using Utilities.SystemActivity.Service;

namespace PlayNetworkAPI.Extensions
{
    public static class PNAServiceCollection
    {
        public static IServiceCollection PNAIocContainer(this IServiceCollection services)
        {
           // services.AddTransient<IBuyerservice, Buyerservice>();
           services.AddTransient<ISystemActivityService,SystemActivityService> ();
           services.AddTransient< ISharedService, SharedServiceExtented> ();
           services.AddTransient< ISuperAdminModuleService, SuperAdminModuleService > ();
           services.AddTransient < ISuperMemberService, SuperMemberService > ();
           services.AddTransient < IMemberService,MemberService > ();
            services.AddTransient<IRestResponse, RestResponse>();
            services.AddTransient < UIHttpClient, UHttpClient > ();
            services.AddTransient< ITransactionNotification, TransactionNotificationService > ();
            services.AddTransient< INotificationServices, NotificationServices> ();
            services.AddTransient<INightLifeService, NightLifeService>();
            services.AddTransient<Ivendorservice,Vendorservice> ();
            services.AddTransient<IDapperr,Dapperr> ();
            services.AddScoped<IGetEntities, GetEntities>();
            services.AddScoped<ISaveEntities, SaveEntities>();
            services.AddScoped<IGymSpaService<ServerResponse>, GymSpaService>();
            services.AddScoped<IConnectionStrings, ConnectionStrings>();
            return services;
        }
    }
}
