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

namespace AthameWpfTest
{
    /// <summary>
    /// Interaction logic for AlbumView.xaml
    /// </summary>
    public partial class AlbumView
    {
        public AlbumView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty AlbumProperty = DependencyProperty.Register(
            "Album", typeof(Album), typeof(AlbumView), new PropertyMetadata(default(Album)));

        public Album Album
        {
            get { return (Album) GetValue(AlbumProperty); }
            set { SetValue(AlbumProperty, value); }
        }
    }
}
