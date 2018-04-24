using System.Collections.Generic;
using System.Windows;
using MahApps.Metro.Controls;
using System.Windows.Controls.DataVisualization.Charting;

namespace PersonalXpns
{
    /// <summary>
    /// Interaction logic for ShowMonthReport.xaml
    /// </summary>
    public partial class ShowMonthReport : MetroWindow
    {
        public ShowMonthReport()
        {
            InitializeComponent();
        }
        private double incomingValue = 0.000001;
        private double OutgoingValue = 0.000003;
        private string TitleOfChart;
        public ShowMonthReport(double Income, double Outgo, string YearMonth)
        {
            InitializeComponent();
            incomingValue = Income;
            OutgoingValue = Outgo;
            TitleOfChart = YearMonth;
        }
        //private void LoadPieChartData()
        //{
        //    ((PieSeries)monthChart.Series[0]).ItemsSource =
        //        new KeyValuePair<string, double>[]{
        //    new KeyValuePair<string, double>("Project Manager", 12),
        //    new KeyValuePair<string, double>("CEO", 25),
        //    new KeyValuePair<string, double>("Software Engg.", 5),
        //    new KeyValuePair<string, double>("Team Leader", 6),
        //    new KeyValuePair<string, double>("Project Leader", 10),
        //    new KeyValuePair<string, double>("Developer", 4) };
        //}

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            ((PieSeries)monthChart.Series[0]).ItemsSource =
                new KeyValuePair<string, double>[]{
                    new KeyValuePair<string, double>("Income",incomingValue),
                    new KeyValuePair<string, double>("Outgoing",OutgoingValue) };
            monthChart.Title = TitleOfChart;
        }
    }
}
