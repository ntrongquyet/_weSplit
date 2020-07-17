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
using System.Data.SqlClient;

namespace WeSplit.User_Control
{
    /// <summary>
    /// Interaction logic for AddMemberUserControl.xaml
    /// </summary>
    public partial class AddMemberUserControl : UserControl
    {
        public AddMemberUserControl()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var cd = DataProvider.Ins.DB.CHUYENDI.ToList();
            foreach (CHUYENDI item in cd)
            {
                if (item.TRANGTHAI == false)
                {
                    trip.Items.Add(item);
                }
            }
            
        }
        private void addMember_Click(object sender, RoutedEventArgs e)
        {
            var name = nameMember.Text.Trim();
            var phone = phoneNumber.Text.Trim();
            if (name.Length == 0)
            {
                MessageBox.Show("Không đễ trống tên thành viên");
            }
            else if (phone.Length == 0)
            {
                MessageBox.Show("Không để trống số điện thoại");

            }else if(DataProvider.Ins.DB.THANHVIEN.Any(mem => mem.SDT == phone))
            {
                MessageBox.Show("Số điện thoại đã trùng");
            }else
            {
                string MTV =$"TV{DataProvider.Ins.DB.THANHVIEN.Count() + 1}";

                var mem = new THANHVIEN()
                {
                    MATV = MTV,
                    TENTV = name,
                    SDT = phone
                };
                nameMember.Text = default;
                phoneNumber.Text = default;
                DataProvider.Ins.DB.THANHVIEN.Add(mem);
                DataProvider.Ins.DB.SaveChanges();
                member.ItemsSource = DataProvider.Ins.DB.THANHVIEN.ToList();
            }
        }


        private void trip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var idCD = trip.SelectedItem as CHUYENDI;
            tableMemberofTrip.ItemsSource = (from tg in DataProvider.Ins.DB.THAMGIA join tv in DataProvider.Ins.DB.THANHVIEN on tg.MATV equals tv.MATV select new { tg.TIENTHU, tv.TENTV, tg.MACD })
                            .Where(tg => tg.MACD == idCD.MA_CHUYENDI).ToList();
            var listinCD = (from tv in DataProvider.Ins.DB.THANHVIEN join tg in DataProvider.Ins.DB.THAMGIA on tv.MATV equals tg.MATV select new { tv.MATV, tv.TENTV, tg.MACD})
                            .Where(tg => tg.MACD == idCD.MA_CHUYENDI);
            var listFilter = (from data in listinCD select new { data.MATV, data.TENTV });
            member.ItemsSource = (from tv in DataProvider.Ins.DB.THANHVIEN select new { tv.MATV, tv.TENTV }).Except(listFilter).ToList();
        }
        private void addMemberToTrip_Click(object sender, RoutedEventArgs e)
        {
           
        }

    }
}
