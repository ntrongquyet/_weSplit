using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace WeSplit.User_Control
{
    /// <summary>
    /// Interaction logic for Add_UserControl.xaml
    /// </summary>
    public partial class Add_UserControl : UserControl
    {
        public Add_UserControl()
        {
            InitializeComponent();
        }

        private void addPlace_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void addTrip_Click(object sender, MouseButtonEventArgs e)
        {
            DataContext = new AddTripUserControl();
            this.Content = new AddTripUserControl();
        }

        private void addMember_Click(object sender, MouseButtonEventArgs e)
        {
            DataContext = new AddMemberUserControl();
            this.Content = new AddMemberUserControl();
        }

        private void addToTrip_Click(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
