using System.Collections.ObjectModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace Notes.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "Notes";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home12 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
            new NavigationViewItem()
            {
                Content = "Work",
                Icon = new SymbolIcon { Symbol = SymbolRegular.BuildingDesktop24 },
                TargetPageType = typeof(Views.Pages.DataPage)
            },
             new NavigationViewItem()
            {
                Content = "Family",
                Icon = new SymbolIcon { Symbol = SymbolRegular.PeopleCommunity24 },
                TargetPageType = typeof(Views.Pages.FamilyPage)
            },
              new NavigationViewItem()
            {
                Content = "For me",
                Icon = new SymbolIcon { Symbol = SymbolRegular.ShieldLock28},
                TargetPageType = typeof(Views.Pages.ForMePage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
