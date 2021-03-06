﻿using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms.Xaml;

namespace Excalibur.Tests.FormsCross.Core.Ui.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, WrapInNavigationPage = true, NoHistory = true)]
    public partial class DashboardPage
    {
        public DashboardPage()
        {
            InitializeComponent();
        }
    }
}
