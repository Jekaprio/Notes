using Notes.ViewModels.Pages;
using System.Windows.Documents;
using System.Windows.Input;
using Wpf.Ui.Controls;
using System.Windows;

namespace Notes.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*System.Windows.MessageBoxButton button = System.Windows.MessageBoxButton.OK;
            System.Windows.MessageBox.Show("This is a message.", "Message", button);*/

        }

       
    }
}


