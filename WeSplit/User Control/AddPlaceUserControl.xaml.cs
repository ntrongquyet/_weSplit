using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddPlaceUserControl.xaml
    /// </summary>
    public partial class AddPlaceUserControl : UserControl
    {
        ObservableCollection<DD_DULICH> addressList = new ObservableCollection<DD_DULICH>();
        private class Temp
        {
            private string maDiaDanh;
            public string MaDiaDanh { get => maDiaDanh; set => maDiaDanh = value; }

            private string tenDiaDanh;
            public string Tendiadanh { get => tenDiaDanh; set => tenDiaDanh = value; }

            private string diaChi;
            public string DiaChi { get => diaChi; set => diaChi = value; }

            private string thongTin;
            public string ThongTin { get => thongTin; set => thongTin = value; }

        }
        public AddPlaceUserControl()
        {
            InitializeComponent();
        } 
        private void UserControl_Initialized(object sender, EventArgs e)
        {
            foreach (DD_DULICH itemTT in DataProvider.Ins.DB.DD_DULICH.ToList())
            {
                addressList.Add(itemTT);
            }
            loadList.ItemsSource = addressList.ToList();
        }
        private void addPlace(object sender, RoutedEventArgs e)
        {
            var id = idPlace.Text.Trim();
            var namePl = namePlace.Text.Trim();
            var nameAddr = nameAddress.Text.Trim();
            if(id.Length == 0)
            {
                MessageBox.Show("Chưa nhập Mã Địa Điểm");
            }
            else if (namePl.Length == 0)
            {
                MessageBox.Show("Chưa nhập Tên Địa Điểm");
            }
            else if(nameAddr.Length == 0)
            {
                MessageBox.Show("Chưa nhập Địa Chỉ");
            }
            else
            {
                var item = new Temp()
                {
                    MaDiaDanh = idPlace.Text,
                    Tendiadanh = namePlace.Text,
                    DiaChi = nameAddress.Text,
                    ThongTin = infomation.Text
                };
                loadAdd.Items.Add(item);
            }
            idPlace.Text = "";
            namePlace.Text = "";
            nameAddress.Text = "";
            infomation.Text = "";
        }

        private void confirm(object sender, RoutedEventArgs e)
        {
            DD_DULICH diadiem = new DD_DULICH()
            {
                MA_DIEMDEN = idPlace.Text.Trim(),
                TEN_DIEMDEN = namePlace.Text.Trim(),
                DCHI_DIEMDEN = nameAddress.Text.Trim(),
                THONGTIN_DD = infomation.Text.Trim()
            };
            DataProvider.Ins.DB.DD_DULICH.Add(diadiem);
            DataProvider.Ins.DB.SaveChanges();
            loadList.Items.Clear();
        }
    }
}
