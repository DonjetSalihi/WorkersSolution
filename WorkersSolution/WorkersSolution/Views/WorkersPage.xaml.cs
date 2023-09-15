using WorkersSolution.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkersSolution.Views
{
	public partial class WorkersPage : ContentPage
	{
        WorkersViewModel viewModel;

        public WorkersPage()
		{
			InitializeComponent ();

            BindingContext = viewModel = Startup.ServiceProvider?.GetService<WorkersViewModel>() ?? new WorkersViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Workers.Count == 0)
                viewModel.LoadWorkersCommand.Execute(null);
        }
    }
}
