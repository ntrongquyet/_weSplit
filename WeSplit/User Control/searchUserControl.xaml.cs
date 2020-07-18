using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for searchUserControl.xaml
    /// </summary>
    public partial class searchUserControl : UserControl
    {
        public searchUserControl()
        {
            InitializeComponent();
        }
        ObservableCollection<CHUYENDI> placeList = new ObservableCollection<CHUYENDI>();
        int num = 0;
        //Lọc Tiếng Việt
        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        //Search
        private void Search_button(object sender, RoutedEventArgs e)
        {
            //Cảnh báo không nhập dữ liệu
            if (Search.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn chưa nhập","Lỗi",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            //Cảnh báo không chọn lọc
            else if(futureSelect.IsChecked == false && oldSelect.IsChecked == false && nameSelect.IsChecked == false)
            {
                MessageBox.Show("Thiếu điều kiện lọc bạn ơi !!!!!","Cảnh báo",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
            else if (oldSelect.IsChecked == true && futureSelect.IsChecked == true && nameSelect.IsChecked == true)
            {
                MessageBox.Show("Không thể chọn tất cả", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            //Search theo đã đi và tên thành viên
            else if (oldSelect.IsChecked == true)
            {
                //Search theo đã đi và tên thành viên
                if (nameSelect.IsChecked == true)
                {
                    var name = (from tg in DataProvider.Ins.DB.THAMGIA
                                join tv in DataProvider.Ins.DB.THANHVIEN on tg.MATV equals tv.MATV
                                join cd in DataProvider.Ins.DB.CHUYENDI on tg.MACD equals cd.MA_CHUYENDI
                                select new { tv.TENTV, cd.TEN_CHUYENDI, cd.MA_CHUYENDI, cd.TRANGTHAI }).Distinct().ToList();
                    var strings = name.Where(thanhvien => convertToUnSign3(thanhvien.TENTV.ToLower()).Contains(convertToUnSign3(Search.Text.ToLower())));
                    var stsr = strings.Where(p => p.TRANGTHAI == true).ToList();
                    if (stsr.Count() != 0)
                    {
                        listPlace.ItemsSource = stsr.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy","Thông báo",MessageBoxButton.OK,MessageBoxImage.Information);
                    }
                }
                else if (futureSelect.IsChecked == true)
                {
                    var strings = placeList.Where(diadiem => convertToUnSign3(diadiem.TEN_CHUYENDI.ToLower()).Contains(convertToUnSign3(Search.Text.ToLower())));
                    if (strings.Count() != 0)
                    {
                        listPlace.ItemsSource = strings.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                //Chỉ search theo đã đi
                else
                {
                    var stsr = placeList.Where(p => p.TRANGTHAI == true).ToList();
                    var strings = stsr.Where(diadiem => convertToUnSign3(diadiem.TEN_CHUYENDI.ToLower()).Contains(convertToUnSign3(Search.Text.ToLower())));
                    if (strings.Count() != 0)
                    {
                        listPlace.ItemsSource = strings.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            //Search theo chưa đi và tên thành viên
            else if (futureSelect.IsChecked == true)
            {
                //Search theo chưa đi và tên thành viên
                if (nameSelect.IsChecked == true)
                {
                    var name = (from tg in DataProvider.Ins.DB.THAMGIA
                                join tv in DataProvider.Ins.DB.THANHVIEN on tg.MATV equals tv.MATV
                                join cd in DataProvider.Ins.DB.CHUYENDI on tg.MACD equals cd.MA_CHUYENDI
                                select new { tv.TENTV, cd.TEN_CHUYENDI, cd.MA_CHUYENDI, cd.TRANGTHAI }).Distinct().ToList();
                    var strings = name.Where(thanhvien => convertToUnSign3(thanhvien.TENTV.ToLower()).Contains(convertToUnSign3(Search.Text.ToLower())));
                    var stsr = strings.Where(p => p.TRANGTHAI == false).ToList();
                    if (stsr.Count() != 0)
                    {
                        listPlace.ItemsSource = stsr.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else if (oldSelect.IsChecked == true)
                {
                    var strings = placeList.Where(diadiem => convertToUnSign3(diadiem.TEN_CHUYENDI.ToLower()).Contains(convertToUnSign3(Search.Text.ToLower())));
                    if (strings.Count() != 0)
                    {
                        listPlace.ItemsSource = strings.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                //Chỉ search theo chưa đi
                else
                {
                    var stsr = placeList.Where(p => p.TRANGTHAI == false).ToList();
                    var strings = stsr.Where(diadiem => convertToUnSign3(diadiem.TEN_CHUYENDI.ToLower()).Contains(convertToUnSign3(Search.Text.ToLower())));
                    if (strings.Count() != 0)
                    {
                        listPlace.ItemsSource = strings.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            //Search theo tên thành viên
            else if (nameSelect.IsChecked == true)
            {
                var name = (from tg in DataProvider.Ins.DB.THAMGIA
                            join tv in DataProvider.Ins.DB.THANHVIEN on tg.MATV equals tv.MATV
                            join cd in DataProvider.Ins.DB.CHUYENDI on tg.MACD equals cd.MA_CHUYENDI
                            select new { tv.TENTV, cd.TEN_CHUYENDI, cd.MA_CHUYENDI, cd.TRANGTHAI }).Distinct().ToList();
                var strings = name.Where(thanhvien => (convertToUnSign3(thanhvien.TENTV.ToLower())).Contains(convertToUnSign3(Search.Text.ToLower())));
                if (strings.Count() != 0)
                {
                    listPlace.ItemsSource = strings.ToList();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            int num = listPlace.Items.Count;
            numbers.Text = Convert.ToString(num);
        }
        //Load số tìm kiếm
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (CHUYENDI itemTT in DataProvider.Ins.DB.CHUYENDI.ToList())
            {
                placeList.Add(itemTT);
            }
            //Hiển số kết quả tìm kiếm
            numbers.Text = Convert.ToString(num);
        }
        //Click để xem chi tiết
        private void listPlace_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var data = listPlace.SelectedItem as CHUYENDI;
            if (data != null)
            {
                DataContext = new DetailsTripUserControl(data.MA_CHUYENDI);
                this.Content = new DetailsTripUserControl(data.MA_CHUYENDI);
            }
            else
            {
                var MACD = listPlace.SelectedItem.ToString();
                MACD = MACD.Split(',')[2];
                MACD = MACD.Substring(MACD.IndexOf("CD"));
                DataContext = new DetailsTripUserControl(MACD);
                this.Content = new DetailsTripUserControl(MACD);
            }
        }
    }
}
