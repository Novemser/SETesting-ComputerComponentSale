using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using WpfApplication1.Annotations;

namespace WpfApplication1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int _outLet;
        private int _mainFrame;
        private int _monitor;
        private string total;

        public MainWindow()
        {
            InitializeComponent();
            _totalValues = new ChartValues<double>();
            _passedValues = new ChartValues<double>();
            _failedValues = new ChartValues<double>();

            SeriesCollection = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "测试用例总数量",
                    Values = _totalValues
                },
                new RowSeries
                {
                    Title = "通过用例数量",
                    Values = _passedValues
                },
                new RowSeries
                {
                    Title = "未通过用例数量",
                    Values = _failedValues
                }
            };
            DataContext = this;
        }
        public void SetPassed(double val)
        {
            while (_passedValues.Count > 0)
            {
                _passedValues.RemoveAt(0);
            }

            _passedValues.Add(val);
        }
        public void SetFailed(double val)
        {
            while (_failedValues.Count > 0)
            {
                _failedValues.RemoveAt(0);
            }

            _failedValues.Add(val);
        }
        public void SetTotal(double val)
        {
            while (_totalValues.Count > 0)
            {
                _totalValues.RemoveAt(0);
            }

            _totalValues.Add(val);
        }

        public int OutLet
        {
            get { return _outLet; }
            set
            {
                _outLet = value;
                OnPropertyChanged(nameof(OutLet));
            }
        }

        public int MainFrame
        {
            get { return _mainFrame; }
            set
            {
                _mainFrame = value;
                OnPropertyChanged(nameof(MainFrame));
            }
        }

        public int Monitor
        {
            get { return _monitor; }
            set
            {
                _monitor = value;
                OnPropertyChanged(nameof(Monitor));
            }
        }

        public string TotalSale
        {
            get { return total; }
            set
            {
                total = value;
                OnPropertyChanged(nameof(TotalSale));
            }
        }

        private string[] GetSumStr(int OutLet, int MainFrame, int Monitor)
        {
            StringBuilder sb = new StringBuilder();
            string[] result = new string[2];
            if (OutLet < 1)
            {
                sb.Append("外设数量<1  ");
            }
            if (OutLet > 90)
            {
                sb.Append("外设数量>90  ");
            }
            if (MainFrame < 1)
            {
                sb.Append("主机数量<1  ");
            }
            if (MainFrame > 70)
            {
                sb.Append("主机数量>70  ");
            }
            if (Monitor < 1)
            {
                sb.Append("显示器数量<1  ");
            }
            if (Monitor > 80)
            {
                sb.Append("显示器数量>80  ");
            }
            if (sb.Length > 2)
            {
                result[0] = (-1).ToString();
                result[1] = sb.ToString();
                return result;
            }

            long sum = 25 * OutLet + 45 * MainFrame + 30 * Monitor;
            result[0] = sum + "$";
            return result;
        }
        private void Calculate(object sender, RoutedEventArgs e)
        {
            string[] res = GetSumStr(OutLet, MainFrame, Monitor);
            if (res[0].Equals("-1"))
            {
                TotalSale = res[1];
            }
            else
            {
                TotalSale = res[0];
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ChartValues<double> _totalValues;
        private ChartValues<double> _passedValues;
        private ChartValues<double> _failedValues;
        public SeriesCollection SeriesCollection { get; set; }

        private void BatchCalculate(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".csv";
            fileDialog.Filter = "csv file|*.csv";
            if (fileDialog.ShowDialog() == true)
            {
                var dataList = FileUtils.ReadDateList(fileDialog.FileName);
                if (null != dataList)
                {
                    int passed, failed;
                    passed = failed = 0;
                    foreach (var csvMapper in dataList)
                    {
                        string[] strRes = GetSumStr(csvMapper.OutLet, csvMapper.MainFrame, csvMapper.Monitor);
                        if (strRes[0].Equals("-1"))
                        {
                            failed++;
                        }
                        else
                        {
                            passed++;
                        }

                        csvMapper.Result = strRes[0];
                        csvMapper.Exception = strRes[1];
                    }

                    WriteResult(dataList);
                    SetFailed(failed);
                    SetPassed(passed);
                    SetTotal(failed + passed);
                }
            }
        }
        void WriteResult(List<CSVMapper> resultList)
        {
            var csvWriter = FileUtils.CreateCsvWriter(@"J:\OneDrive\Coding\ModernUISaleComputer\SaleResult.csv");
            csvWriter.WriteRecords(resultList);
            csvWriter.Dispose();
        }
    }
}
