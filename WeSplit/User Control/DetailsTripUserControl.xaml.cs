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
using WeSplit.SQLData;

namespace WeSplit.User_Control
{
    /// <summary>
    /// Interaction logic for DetailsTripUserControl.xaml
    /// </summary>
    public partial class DetailsTripUserControl : UserControl
    {
        public DetailsTripUserControl()
        {
            InitializeComponent();
        }

        private void DetailsUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            table_THANHVIEN.ItemsSource = DataProvider.Ins.DB.THANHVIEN.ToList();
        }
    }
}
