﻿using BackgammonProject.Pages;
using BackgammonProject.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgammonProject.ViewModel
{
    public class ViewModelLocator
    {

        public const string LoginPageKey = "LoginPage";
        public const string ContactsPageKey = "ContactsPage";
        public const string ChatPageKey = "ChatPage";


        public ViewModelLocator()
        {

            var nav = new NavigationService();
            nav.Configure(LoginPageKey, typeof(LoginPage));
            nav.Configure(ContactsPageKey, typeof(ContactsPage));
            nav.Configure(ChatPageKey, typeof(ChatPage));

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //Services
            SimpleIoc.Default.Register<INavigationService>(() => nav);
            SimpleIoc.Default.Register<IDialogService, DialogService>();

            //ViewModels
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<ContactsViewModel>();
            SimpleIoc.Default.Register<ChatViewModel>();

        }

        public LoginViewModel LoginVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }

        public ContactsViewModel ContactsVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ContactsViewModel>();
            }
        }

        public ChatViewModel ChatVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ChatViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
