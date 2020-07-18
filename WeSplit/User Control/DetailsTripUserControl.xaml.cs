using LiveCharts;
using LiveCharts.Wpf;
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
        private string MaCD;

        public SeriesCollection SeriesCollection { get; }
        public SeriesCollection SeriesCollectionKC { get; }


        public DetailsTripUserControl(string mA_CHUYENDI)
        {
            InitializeComponent();
            this.MaCD = mA_CHUYENDI;
            //Vẽ biểu đồ khoản thu các thành viên
            SeriesCollection = new SeriesCollection();
            foreach (THAMGIA itemTG in DataProvider.Ins.DB.THAMGIA.ToList())
            {
                if (itemTG.MACD == MaCD)
                {
                    ChartValues<double> cost = new ChartValues<double>();
                    cost.Add(Convert.ToDouble(itemTG.TIENTHU));
                    PieSeries series = new PieSeries
                    {
                        Values = cost,
                        Title = itemTG.THANHVIEN.TENTV,
                    };
                    SeriesCollection.Add(series);
                }
            }
            //Vẽ biểu đồ khoản chi trong chuyến đi
            SeriesCollectionKC = new SeriesCollection();
            foreach (KHOANCHI itemTG in DataProvider.Ins.DB.KHOANCHI.ToList())
            {
                if (itemTG.MA_CHUYENDI == MaCD)
                {
                    ChartValues<double> cost = new ChartValues<double>();
                    cost.Add(Convert.ToDouble(itemTG.SOTIENCHI));
                    PieSeries series = new PieSeries
                    {
                        Values = cost,
                        Title = itemTG.HANGMUC,
                    };
                    SeriesCollectionKC.Add(series);
                }
            }
        }

        private void DetailsUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var queryTrip = from trip in DataProvider.Ins.DB.CHUYENDI
                            where trip.MA_CHUYENDI == MaCD
                            select trip;
            table_THANHVIEN.ItemsSource = (from tg in DataProvider.Ins.DB.THAMGIA join tv in DataProvider.Ins.DB.THANHVIEN on tg.MATV equals tv.MATV select new { tv.TENTV, tv.SDT, tg.TIENTHU, tg.MACD })
                                            .Where(tg => tg.MACD == MaCD).ToList();
            var selectTrip = (from cd in DataProvider.Ins.DB.CHUYENDI
                              where cd.MA_CHUYENDI == MaCD
                              select cd);
            route.ItemsSource = selectTrip.ToList()[0].LOTRINH;
            DataContext = selectTrip.ToList()[0];
            //Khoản thu CĐ
            var placeKT = (from tg in DataProvider.Ins.DB.THAMGIA
                           join tv in DataProvider.Ins.DB.THANHVIEN on tg.MATV equals tv.MATV
                           select new { tv.TENTV, tv.SDT, tg.TIENTHU, tg.MACD });
            var sumtt = placeKT.Where(tg => tg.MACD == MaCD).ToList();
            var sumKT = sumtt.Select(s => s.TIENTHU).Sum();
            sumTT.Text = Convert.ToString(sumKT);
            // Khoản chi CĐ
            var placeKC = (from tv in DataProvider.Ins.DB.THANHVIEN
                           join tg in DataProvider.Ins.DB.THAMGIA on tv.MATV equals tg.MATV
                           join cd in DataProvider.Ins.DB.CHUYENDI on tg.MACD equals cd.MA_CHUYENDI
                           join kc in DataProvider.Ins.DB.KHOANCHI on cd.MA_CHUYENDI equals kc.MA_CHUYENDI
                           select new { kc.SOTIENCHI, kc.MA_CHUYENDI }).Distinct();
            var sumtc = placeKC.Where(su => su.MA_CHUYENDI == MaCD).ToList();
            var sumKC = sumtc.Select(sum => sum.SOTIENCHI).Sum();
            sumTC.Text = Convert.ToString(sumKC);
            double mul = (double)sumKT - (double)sumKC;
            if (mul < 0)
            {
                confirmMoney.Text = "Thiếu " + Convert.ToString(Math.Abs(mul));
            }
            else if (mul > 0)
            {
                confirmMoney.Text = "Dư " + Convert.ToString(mul);
            }
            else if (mul == 0)
            {
                confirmMoney.Text = Convert.ToString(mul);
            }
        }

        private void show_Click(object sender, RoutedEventArgs e)
        {

            if (route_trip.Visibility == Visibility.Visible)
            {
                route_trip.Visibility = Visibility.Collapsed;
            }
            else
            {
                route_trip.Visibility = Visibility.Visible;
            }
        }

        private void click_AddMembers(object sender, MouseButtonEventArgs e)
        {
            DataContext = new AddMemberUserControl();
            this.Content = new AddMemberUserControl();
        }
    }
}
