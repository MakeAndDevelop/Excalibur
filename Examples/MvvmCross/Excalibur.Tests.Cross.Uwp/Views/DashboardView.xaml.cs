﻿using MvvmCross.WindowsUWP.Views;

namespace Excalibur.Tests.Cross.Uwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
	[MvxRegion("PageContent")]
    public sealed partial class DashboardView
    {
        public DashboardView()
        {
            this.InitializeComponent();
        }
    }
}
