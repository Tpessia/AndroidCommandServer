using AndroidCommandServer.Services;
using AndroidCommandServer.ViewModels;
using System;
using Xamarin.Forms;

namespace AndroidCommandServer.Views
{
    public partial class MainPage : ContentPage
    {
        public MainViewModel MainViewModel => BindingContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel(
                DependencyService.Get<IHttpServer>(),
                DependencyService.Get<ICommandHandler>()
            );
        }

        #region Lifecycle

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //MainViewModel.Subscribe<string>("Intent", async (sender, args) =>
            //{
            //    try
            //    {
            //        await MainViewModel.RefreshUsbList();
            //    }
            //    catch (Exception ex)
            //    {
            //        MainViewModel.ShowError(ex);
            //    }
            //});
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            //MainViewModel.Unsubscribe<string>("Intent");

            MainViewModel.Disconnect();
        }

        #endregion

        #region Actions

        private void OnStartClicked(object sender, EventArgs e)
        {
            try
            {
                var endpoint = MainViewModel.GetEndpoint();
                MainViewModel.Connect(endpoint);
            }
            catch (Exception ex)
            {
                MainViewModel.ShowError(ex);
            }
        }

        private void OnStopClicked(object sender, EventArgs e)
        {
            try
            {
                MainViewModel.Disconnect();
            }
            catch (Exception ex)
            {
                MainViewModel.ShowError(ex);
            }
        }

        #endregion
    }
}
