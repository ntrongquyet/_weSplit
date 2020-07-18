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
            //Search theo đã đi và tên thành viên
            if (oldSelect.IsChecked == true)
            {
                //Search theo đã đi và tên thành viên
                if (nameSelect.IsChecked==true)
                {
                    var name = (from tg in DataProvider.Ins.DB.THAMGIA
                                join tv in DataProvider.Ins.DB.THANHVIEN on tg.MATV equals tv.MATV
                                join cd in DataProvider.Ins.DB.CHUYENDI on tg.MACD equals cd.MA_CHUYENDI
                                select new { tv.TENTV, cd.TEN_CHUYENDI, cd.MA_CHUYENDI, cd.TRANGTHAI });
                    var strings = name.Where(thanhvien => (thanhvien.TENTV.ToLower()).Contains((Search.Text.ToLower())));
                    var stsr = strings.Where(p => p.TRANGTHAI == true).ToList();
                    if (stsr.Count() != 0)
                    {
                        listPlace.ItemsSource = stsr.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Không có");
                    }
                }
                else if(futureSelect.IsChecked == true)
                {
                    var strings = placeList.Where(diadiem => convertToUnSign3(diadiem.TEN_CHUYENDI.ToLower()).Contains(convertToUnSign3(Search.Text.ToLower())));
                    if (strings.Count() != 0)
                    {
                        listPlace.ItemsSource = strings.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Không có");
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
                        MessageBox.Show("Không có");
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
                                select new { tv.TENTV, cd.TEN_CHUYENDI, cd.MA_CHUYENDI, cd.TRANGTHAI });
                    var strings = name.Where(thanhvien => (thanhvien.TENTV.ToLower()).Contains((Search.Text.ToLower())));
                    var stsr = strings.Where(p => p.TRANGTHAI == false).ToList();
                    if (stsr.Count() != 0)
                    {
                        listPlace.ItemsSource = stsr.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Không có");
                    }
                }
                else if(oldSelect.IsChecked == true)
                {
                    var strings = placeList.Where(diadiem => convertToUnSign3(diadiem.TEN_CHUYENDI.ToLower()).Contains(convertToUnSign3(Search.Text.ToLower())));
                    if (strings.Count() != 0)
                    {
                        listPlace.ItemsSource = strings.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Không có");
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
                        MessageBox.Show("Không có");
                    }
                }  
            }
            //Search theo tên thành viên
            else if (nameSelect.IsChecked == true)
            {
                var name = (from tg in DataProvider.Ins.DB.THAMGIA
                            join tv in DataProvider.Ins.DB.THANHVIEN on tg.MATV equals tv.MATV
                            join cd in DataProvider.Ins.DB.CHUYENDI on tg.MACD equals cd.MA_CHUYENDI
                            select new { tv.TENTV, cd.TEN_CHUYENDI,cd.MA_CHUYENDI,cd.TRANGTHAI }).ToList();
                var strings = name.Where(thanhvien => (convertToUnSign3(thanhvien.TENTV.ToLower())).Contains(convertToUnSign3(Search.Text.ToLower())));
                if(strings.Count() != 0)
                {
                    listPlace.ItemsSource = strings.ToList();
                }
                else
                {
                    MessageBox.Show("Không có");
                }
            }    
            //Search theo tất cả trường hợp khi không click chọn
            else
            {
                var strings = placeList.Where(diadiem => convertToUnSign3(diadiem.TEN_CHUYENDI.ToLower()).Contains(convertToUnSign3(Search.Text.ToLower())));
                var name = (from tg in DataProvider.Ins.DB.THAMGIA
                            join tv in DataProvider.Ins.DB.THANHVIEN on tg.MATV equals tv.MATV
                            join cd in DataProvider.Ins.DB.CHUYENDI on tg.MACD equals cd.MA_CHUYENDI
                            select new { tv.TENTV, cd.TEN_CHUYENDI, cd.MA_CHUYENDI, cd.TRANGTHAI });
                var str = name.Where(thanhvien => thanhvien.TENTV.ToLower().Contains(Search.Text.ToLower()));
                if (strings.Count() != 0)
                {
                    listPlace.ItemsSource = strings.ToList();
                }
                else if (str.Count() != 0)
                {
                    listPlace.ItemsSource = str.ToList();
                }    
                else
                {
                    MessageBox.Show("Không có");
                }
            }
            int num = listPlace.Items.Count;
            numbers.Text = Convert.ToString(num);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (CHUYENDI itemTT in DataProvider.Ins.DB.CHUYENDI.ToList())
            {
                placeList.Add(itemTT);
            }
            listPlace.ItemsSource = placeList.ToList();
            numbers.Text = Convert.ToString(num);
        }

        private void listPlace_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //var data = listPlace.SelectedItem.ToString();
            ////DataContext = new DetailsTripUserControl(d);
            ////this.Content = new DetailsTripUserControl(data.MA_CHUYENDI);
        }
    }
}
