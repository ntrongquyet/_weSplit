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
                if(itemTG.MA_CHUYENDI == MaCD)
                {
                    foreach (CHUYENDI itemCD in DataProvider.Ins.DB.CHUYENDI.ToList())
                    {
                        if (itemCD.MA_CHUYENDI == MaCD)
                        {
                            ChartValues<double> cost = new ChartValues<double>();
                            ChartValues<double> costMB = new ChartValues<double>();
                            ChartValues<double> costKS = new ChartValues<double>();
                            ChartValues<double> costTS = new ChartValues<double>();
                            cost.Add(Convert.ToDouble(itemTG.SOTIENCHI));
                            costMB.Add(Convert.ToDouble(itemCD.MAYBAY));
                            costKS.Add(Convert.ToDouble(itemCD.THUE_KS));
                            costTS.Add(Convert.ToDouble(itemCD.THUE_XE));
                            var temp = itemTG.SOTIENCHI;
                            if (temp != 0)
                            {
                                PieSeries series = new PieSeries
                                {
                                    Values = cost,
                                    Title = itemTG.HANGMUC,
                                };
                                SeriesCollectionKC.Add(series);
                            }
                            if (itemCD.THUE_KS != 0)
                            {
                                PieSeries seriesKS = new PieSeries
                                {
                                    Values = costKS,
                                    Title = "Khách sạn"
                                };
                                SeriesCollectionKC.Add(seriesKS);
                            }
                            if (itemCD.MAYBAY != 0)
                            {
                                PieSeries seriesMB = new PieSeries
                                {
                                    Values = costMB,
                                    Title = "Máy bay"
                                };
                                SeriesCollectionKC.Add(seriesMB);
                            }
                            if (itemCD.THUE_XE != 0)
                            {
                                PieSeries seriesTS = new PieSeries
                                {
                                    Values = costTS,
                                    Title = "Thuê xe"
                                };
                                SeriesCollectionKC.Add(seriesTS);
                            }
                        }
                    }
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
            var KCPlace = (from cd in DataProvider.Ins.DB.CHUYENDI
                           select new { cd.MA_CHUYENDI,cd.MAYBAY, cd.THUE_KS, cd.THUE_XE });
            var sumkc = KCPlace.Where(sumkchi => sumkchi.MA_CHUYENDI == MaCD).ToList();
            var sumAir = sumkc.Select(sumair => sumair.MAYBAY).Sum();
            var sumKS = sumkc.Select(sumks => sumks.THUE_KS).Sum();
            var sumTS = sumkc.Select(sumts => sumts.THUE_XE).Sum();
            var placeKC = (from tv in DataProvider.Ins.DB.THANHVIEN
                           join tg in DataProvider.Ins.DB.THAMGIA on tv.MATV equals tg.MATV
                           join cd in DataProvider.Ins.DB.CHUYENDI on tg.MACD equals cd.MA_CHUYENDI
                           join kc in DataProvider.Ins.DB.KHOANCHI on cd.MA_CHUYENDI equals kc.MA_CHUYENDI
                           select new { kc.SOTIENCHI, kc.MA_CHUYENDI }).Distinct();
            var sumtc = placeKC.Where(su => su.MA_CHUYENDI == MaCD).ToList();
            var sumKC = sumtc.Select(sum => sum.SOTIENCHI).Sum();
            var SUM = sumAir + sumKS + sumTS + sumKC;
            sumTC.Text = Convert.ToString(SUM);
            double mul = (double)sumKT - (double)SUM;
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
            // Hiển thị thông tin chi tiêu trước khi đi
            if (selectTrip.ToList()[0].THUE_XE > 0)
            {
                priceCar.Visibility = Visibility.Visible;
            }
            if (selectTrip.ToList()[0].THUE_KS > 0)
            {
                priceRoom.Visibility = Visibility.Visible;
            }
            if (selectTrip.ToList()[0].MAYBAY> 0)
            {
                pricePlane.Visibility = Visibility.Visible;
            }
            // Hiện thị nút add thành viên cho chuyến đi chưa kết thúc
            if (selectTrip.ToList()[0].TRANGTHAI == true) {
                addMember.Visibility = Visibility.Hidden;
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
            DataContext = new AddMemberUserControl(MaCD);
            this.Content = new AddMemberUserControl(MaCD);
        }

        private void editClick(object sender, RoutedEventArgs e)
        {
            DataContext = new AddTripUserControl(MaCD);
            this.Content = new AddTripUserControl(MaCD);

        }
    }
}
