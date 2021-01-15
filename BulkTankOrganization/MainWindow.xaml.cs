using System;
using System.Windows.Navigation;

namespace BulkTankOrganization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Source = new Uri("TankSelection.xaml", UriKind.Relative);
        }
    }
}

//http://alloy.cis.local/
//"TankSelection.xaml", UriKind.Relative
