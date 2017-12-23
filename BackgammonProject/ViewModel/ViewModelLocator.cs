using BackgammonProject.Infra;
using BackgammonProject.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
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
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //ViewModels
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<ContactsViewModel>();

            //Services
            SimpleIoc.Default.Register<ILoginService, MockupLoginService>();

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

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
