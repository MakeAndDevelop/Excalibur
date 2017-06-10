﻿using System;
using Excalibur.Tests.FormsCross.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Presenters;
using MvvmCross.Forms.ViewModels;
using MvvmCross.Platform;
using Xamarin.Forms;

namespace Excalibur.Tests.FormsCross
{
    public class CustomPresenter : MvxViewPresenter
    {
        private bool _hasNavigatedToLogin = false;
        private Application _mvxFormsApp;

        public Application MvxFormsApp
        {
            get { return _mvxFormsApp; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("MvxFormsApp cannot be null");
                }

                _mvxFormsApp = value;
            }
        }
        protected CustomPresenter()
        {
        }

        protected CustomPresenter(Application mvxFormsApp)
        {
            MvxFormsApp = mvxFormsApp;
        }


        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint)
            {
                if (MvxFormsApp.MainPage is MasterDetailPage mainPage)
                {
                    // Perform pop on the Detail Page and launch RootContentPageActivated if root has been reached
                    var navPage = mainPage.Detail as NavigationPage;
                    navPage.PopAsync();
                    if (navPage.Navigation.NavigationStack.Count == 1)
                        RootContentPageActivated();
                }
                else if (MvxFormsApp.MainPage is NavigationPage navPage)
                {
                    navPage.PopAsync();
                }
                else
                {
                    Mvx.TaggedTrace("MvxFormsPresenter:ChangePresentation()", "Oops! Don't know what to do");
                }
            }
        }



        public override void Show(MvxViewModelRequest request)
        {
            if (TryShowPage(request))
                return;

            Mvx.Error("Skipping request for {0}", request.ViewModelType.Name);
        }

        public override void Close(IMvxViewModel toClose)
        {
        }



        private bool TryShowPage(MvxViewModelRequest request)
        {
            var page = MvxPresenterHelpers.CreatePage(request);
            if (page == null)
                return false;

            var viewModel = MvxPresenterHelpers.LoadViewModel(request);

            SetupForBinding(page, viewModel, request);

            if (request.ViewModelType == typeof(LoginViewModel))
            {
                // just navigation to login
                _mvxFormsApp.MainPage = new NavigationPage(page);
                var loginMainPage = MvxFormsApp.MainPage as NavigationPage;
                CustomPlatformInitialization(loginMainPage);
                _hasNavigatedToLogin = true;
                return true;
            }

            if (request.ViewModelType == typeof(MainViewModel))
            {
                // Reset and use master detail

                // The ViewModel used should inherit from MvxMasterDetailViewModel, so we can create a new ContentPage for use in the Detail page
                var masterDetailViewModel = viewModel as MvxMasterDetailViewModel;
                if (masterDetailViewModel == null)
                    throw new InvalidOperationException("ViewModel should inherit from MvxMasterDetailViewModel<T>");

                Page rootContentPage = null;
                if (masterDetailViewModel.RootContentPageViewModelType != null)
                {
                    var rootContentRequest = new MvxViewModelRequest(masterDetailViewModel.RootContentPageViewModelType, null, null);

                    var rootContentViewModel = MvxPresenterHelpers.LoadViewModel(rootContentRequest);
                    rootContentPage = MvxPresenterHelpers.CreatePage(rootContentRequest);
                    SetupForBinding(rootContentPage, rootContentViewModel, rootContentRequest);
                }
                else
                    rootContentPage = new ContentPage();

                var navPage = new NavigationPage(rootContentPage);

                //Hook to Popped event to launch RootContentPageActivated if proceeds
                navPage.Popped += (sender, e) =>
                {
                    if (navPage.Navigation.NavigationStack.Count == 1)
                        RootContentPageActivated();
                };

                var mainPage = new MasterDetailPage
                {
                    Master = page,
                    Detail = navPage
                };

                _mvxFormsApp.MainPage = mainPage;
                if (!_hasNavigatedToLogin)
                {
                    CustomPlatformInitialization(mainPage);
                }
            }
            else
            {
                // Functionality for clearing the navigation stack before pushing to new Page (for example in a menu with multiple options)
                var mainPage = _mvxFormsApp.MainPage as MasterDetailPage;
                if (request.PresentationValues != null)
                {
                    if (request.PresentationValues.ContainsKey("NavigationMode") && request.PresentationValues["NavigationMode"] == "ClearStack")
                    {
                        mainPage.Detail.Navigation.PopToRootAsync();
                        if (Device.Idiom == TargetIdiom.Phone)
                            mainPage.IsPresented = false;
                    }
                }

                try
                {
                    var nav = mainPage.Detail as NavigationPage;

                    // calling this sync blocks UI and never navigates hence code continues regardless here
                    nav.PushAsync(page);
                }
                catch (Exception e)
                {
                    Mvx.Error("Exception pushing {0}: {1}\n{2}", page.GetType(), e.Message, e.StackTrace);
                    return false;
                }
            }

            return true;
        }


        private void SetupForBinding(Page page, IMvxViewModel viewModel, MvxViewModelRequest request)
        {
            var mvxContentPage = page as IMvxContentPage;
            if (mvxContentPage != null)
            {
                mvxContentPage.Request = request;
                mvxContentPage.ViewModel = viewModel;
            }
            else
            {
                page.BindingContext = viewModel;
            }

        }

        private void RootContentPageActivated()
        {
            var mainPage = Application.Current.MainPage as MasterDetailPage;
            (mainPage.Master.BindingContext as MvxMasterDetailViewModel)?.RootContentPageActivated();
        }

        protected virtual void CustomPlatformInitialization(NavigationPage mainPage)
        {
        }

        protected virtual void CustomPlatformInitialization(MasterDetailPage mainPage)
        {
        }
    }
}