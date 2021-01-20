using MvvmCross;
using MvvmCross.ViewModels;
using MvvmCross.Navigation;
using System;
using MvvmCross.IoC;
using Autofac;
using tabApp.Core.Services.Interfaces;

namespace tabApp.Core
{
    public class App : MvxApplication
    {

        public override void Initialize()
        {
            CreatableTypes()
               .EndingWith("Service")
               .AsInterfaces()
               .RegisterAsLazySingleton();


            CreatableTypes()
                .EndingWith("Client")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            // register the appstart object

            RegisterCustomAppStart<AppStart>();
        }
    }
}
