﻿using Excalibur.Cross.ViewModels;
using Excalibur.Tests.Cross.Core.Services.Interfaces;

namespace Excalibur.Tests.Cross.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ILoginService _loginService;
        private string _email;
        private string _password;
        private bool _isLoading;

        public LoginViewModel(ILoginService loginService)
        {
            _loginService = loginService;

            Email = "TestUser";
            Password = "YouCantSeeMe";
            IsLoading = false;
        }

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }


    }
}