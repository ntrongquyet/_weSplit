using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for AddTripUserControl.xaml
    /// </summary>
    public partial class AddTripUserControl : UserControl
    {
        string MaCD = "";
        int _STT = 0;
        int idKC = DataProvider.Ins.DB.KHOANCHI.Count();
        public AddTripUserControl()
        {
            InitializeComponent();
            var CHUYENDI = DataProvider.Ins.DB.CHUYENDI.ToList();
            int num = CHUYENDI.Count + 1;
            MaCD = "CD" + num;
        }
        List<DD_DUNGCHAN> DUNGCHAN = DataProvider.Ins.DB.DD_DUNGCHAN.ToList();
        List<LOTRINH> listRoadMap = new List<LOTRINH>();
        private class Temp
        {
            private string noiDungChan;

            public string NoiDungChan { get => noiDungChan; set => noiDungChan = value; }

            private string moTa;
            public string MoTa { get => moTa; set => moTa = value; }

            private string chiPhi;
            public string ChiPhi { get => chiPhi; set => chiPhi = value; }

            private string diaDiem;
            public string DiaDiem { get => diaDiem; set => diaDiem = value; }

        }
        private void UserControl_Initialized(object sender, EventArgs e)
        {
            destination.ItemsSource = DataProvider.Ins.DB.DD_DULICH.ToList();
            province.ItemsSource = DataProvider.Ins.DB.DD_DUNGCHAN.ToList();
            checkout.BlackoutDates.AddDatesInPast();
            checkin.BlackoutDates.AddDatesInPast();
        }
        private void addRoadMap_Click(object sender, RoutedEventArgs e)
        {
            var wp = wayPoint.Text.Trim();
            var des = description.Text.Trim();
            if (price.IsEnabled == false)
            {
                var prc = "0";
            }
            if (price.IsEnabled == true)
            {

                if (price.Text.Trim().Length == 0)
                {
                    MessageBox.Show($"Chưa nhập chi phí cho {des}");
                }

            }
            if (wp.Length == 0)
            {
                MessageBox.Show("Chưa nhập nơi dừng chân");
            }
            else if (des.Length == 0)
            {
                MessageBox.Show("Chưa nhập mô tả");
            }
            else if (province.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn tỉnh thành");
            }
            else
            {
                _STT += 1;
                var dddc = DataProvider.Ins.DB.DD_DUNGCHAN.ToList();
                var lt = new LOTRINH
                {
                    STT = _STT,
                    MA_CHUYENDI = MaCD,
                    MA_DD_DUNGCHAN = dddc[province.SelectedIndex].MA_DD_DUNGCHAN,
                    DD_DUNGCHAN = wp,
                    MOTA = des
                };
                DataProvider.Ins.DB.LOTRINH.Add(lt);
                if (price.Text.Trim().Length != 0)
                {
                    var prc = Convert.ToDouble(price.Text.Trim());
                    idKC++;
                    var kc = new KHOANCHI
                    {
                        MA_KCHI = "KC" + idKC,
                        MA_CHUYENDI = MaCD,
                        STT_DC = _STT,
                        MA_DD_DUNGCHAN = dddc[province.SelectedIndex].MA_DD_DUNGCHAN,
                        SOTIENCHI = prc,
                        HANGMUC = des,
                        GHI_CHU = null

                    };
                    DataProvider.Ins.DB.KHOANCHI.Add(kc);
                }
                var item = new Temp()
                {
                    NoiDungChan = wayPoint.Text,
                    MoTa = description.Text,
                    ChiPhi = price.Text,
                    DiaDiem = dddc[province.SelectedIndex].TEN_DD_DUNGCHAN
                };
                roadMapsList.Items.Add(item);
            }
            

            wayPoint.Text = "";
            description.Text = "";
            province.SelectedItem = default;
            MaterialDesignFilledTextFieldTextBoxEnabledComboBox.IsChecked = false;
            price.Text = "";
        }
        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            var dd = DataProvider.Ins.DB.DD_DULICH.ToList();

            if (rentOfRoom.Text == "")
            {
                rentOfRoom.Text = "0";
            }
            if (rentOfPlane.Text == "")
            {
                rentOfPlane.Text = "0";
            }
            if (rentOfCar.Text == "")
            {
                rentOfCar.Text = "0";
            }
            if (destination.SelectedIndex == -1)
            {
                MessageBox.Show("Chưa chọn điểm đến");
            }
            else if (nameTrip.Text.Trim().Length == 0)
            {
                MessageBox.Show("Tên chuyến đi không được bỏ trống");
            }
            else if (checkin.SelectedDate.ToString().Length == 0 || checkout.SelectedDate.ToString().Length == 0)
            {
                MessageBox.Show("Ngày đi ngày về không dược bỏ trống");
            }
            else if (roadMapsList.Items.Count < 1)
            {
                MessageBox.Show("Chuyến đi này chưa có lộ trình");
            }
            else
            {
                CHUYENDI chuyendi = new CHUYENDI()
                {
                    MA_CHUYENDI = MaCD,
                    TEN_CHUYENDI = nameTrip.Text.Trim(),
                    DIEMDEN = dd[destination.SelectedIndex].MA_DIEMDEN,
                    NGAYDI = checkin.SelectedDate.Value,
                    NGAYVE = checkout.SelectedDate.Value,
                    THUE_KS = Int32.Parse(rentOfRoom.Text.Trim()),
                    THUE_XE = Int32.Parse(rentOfCar.Text.Trim()),
                    MAYBAY = Int32.Parse(rentOfPlane.Text.Trim()),
                    TRANGTHAI = false,
                };
                DataProvider.Ins.DB.CHUYENDI.Add(chuyendi);
                DataProvider.Ins.DB.SaveChanges();
                nameTrip.Text = "";
                destination.SelectedItem = default;
                checkin.SelectedDate = default;
                checkout.SelectedDate = default;
                car.IsChecked = false;
                room.IsChecked = false;
                plane.IsChecked = false;
                roadMapsList.Items.Clear();
            }

        }
        private void car_Checked(object sender, RoutedEventArgs e)
        {
            if (car.IsChecked == true)
            {
                rentOfCar.IsEnabled = true;
            }
            else
            {
                rentOfCar.IsEnabled = false;
            }
        }
        private void room_Checked(object sender, RoutedEventArgs e)
        {
            if (room.IsChecked == true)
            {
                rentOfRoom.IsEnabled = true;
            }
            else
            {
                rentOfRoom.IsEnabled = false;
            }
        }
        private void plane_Checked(object sender, RoutedEventArgs e)
        {
            if (plane.IsChecked == true)
            {
                rentOfPlane.IsEnabled = true;
            }
            else
            {
                rentOfPlane.IsEnabled = false;
            }

        }
        private void checkin_Changed(object sender, RoutedEventArgs e)
        {
            checkout.SelectedDate = default;
            checkout.BlackoutDates.Clear();
            if (checkin.SelectedDate != default)
            {
                var date = checkin.SelectedDate.Value;
                checkout.BlackoutDates.AddDatesInPast();
                checkout.BlackoutDates.Add(new CalendarDateRange(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), new DateTime(date.Year, date.Month, date.Day - 1)));
            }
            
        }
    }
}