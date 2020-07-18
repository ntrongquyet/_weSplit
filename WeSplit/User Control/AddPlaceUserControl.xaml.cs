using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        public AddPlaceUserControl()
        {
            InitializeComponent();
        }
        private void UserControl_Initialized(object sender, EventArgs e)
        {
            loadList.ItemsSource = DataProvider.Ins.DB.DD_DULICH.ToList();
        }
        private void addPlace(object sender, RoutedEventArgs e)
        {
            if (namePlace.Text.Trim().Length == 0)
            {
                MessageBox.Show("Chưa nhâp tên địa danh");
            }
            else if (nameAddress.Text.Trim().Length == 0)
            {
                MessageBox.Show("Chưa nhâp địa chỉ");
            }
            else if (path.Length == 0)
            {
                MessageBox.Show("Ảnh đại diện địa điểm chưa có kìa!!!!");
            }
            else
            {
                var info = new FileInfo(path);
                string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
                baseFolder += "Image\\dd\\";
                string name = Guid.NewGuid().ToString();
                File.Copy(path, baseFolder + name + info.Extension);
                var dd = new DD_DULICH()
                {
                    MA_DIEMDEN = $"DD{DataProvider.Ins.DB.DD_DULICH.ToList().Count + 1}",
                    TEN_DIEMDEN = namePlace.Text.Trim(),
                    DCHI_DIEMDEN = nameAddress.Text.Trim(),
                    THONGTIN_DD = infomation.Text.Trim(),
                };
                DataProvider.Ins.DB.DD_DULICH.Add(dd);
                DataProvider.Ins.DB.SaveChanges();
                var ha = new HINHANH_DDDL()
                {
                    MA_ANH_DD = "HACD" + DataProvider.Ins.DB.HINHANH_DDDL.Count() + 1,
                    TENANH_DD = "dd\\" + name,
                    DD_DULICH = dd.MA_DIEMDEN,

                };
                DataProvider.Ins.DB.HINHANH_DDDL.Add(ha);
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show($"Thêm thành công địa điểm{dd.TEN_DIEMDEN}");
            }
            namePlace.Text = default;
            nameAddress.Text = default;
            loadList.ItemsSource = DataProvider.Ins.DB.DD_DULICH.ToList();
        }
        string path = "";
        private void imageDD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (ofd.ShowDialog() == true)
            {
                path = ofd.FileName;
            }
            if (path != "")
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(path);
                logo.EndInit();
                imageDD.Source = logo;
            }

        }
    }
}
