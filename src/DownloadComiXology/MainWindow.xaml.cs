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
using WatiN.Core;

namespace DownloadComiXology
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ComiXology _c;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _c = new ComiXology(txtUrl.Text, txtPassword.Text);
            _c.Run();

            MessageBox.Show("Finished adding to cart - you can now check out in the browser window\n\nBEWARE: Closing this application first might close your browser window!", "All Done!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
