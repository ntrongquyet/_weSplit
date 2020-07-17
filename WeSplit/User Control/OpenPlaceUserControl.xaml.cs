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
using LiveCharts.Charts;
using LiveCharts.Wpf;

namespace WeSplit.User_Control
{
    /// <summary>
    /// Interaction logic for OpenPlaceUserControl.xaml
    /// </summary>
    public partial class OpenPlaceUserControl : UserControl
    {
        private CHUYENDI newplace;
        public OpenPlaceUserControl(CHUYENDI openplace)
        {
            InitializeComponent();
            this.newplace = openplace;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = newplace;
            
        }
    }
}
