using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for DialogAddImage.xaml
    /// </summary>
    public partial class DialogAddImage : Window
    {
        public DialogAddImage()
        {
            InitializeComponent();
        }
        private List<string> listPathImage = new List<string>();

        private void confirmButton(object sender, RoutedEventArgs e)
        {

        }
        private void browserButton(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (ofd.ShowDialog() == true)
            {
                foreach (string item in ofd.FileNames)
                {
                    listImages.Items.Add(item);
                    listPathImage.Add(item);

                }
            }
        }

        private void imageDelete(object sender, RoutedEventArgs e)
        {

        }
    }
}
