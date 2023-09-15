using System;
using WorkersSolution.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkersSolution
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Startup.Init();

            MainPage = new WorkersPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
