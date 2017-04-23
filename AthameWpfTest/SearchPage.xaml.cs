using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Athame.PluginAPI.Service;

namespace AthameWPF
{
    /// <summary>
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private void ByUrlSecMenuOption_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void MainFrame_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void TempAlbum_OnLoaded(object sender, RoutedEventArgs e)
        {
            TempAlbum.Album = MockDataGen.GenerateAlbum();
        }
    }
}
