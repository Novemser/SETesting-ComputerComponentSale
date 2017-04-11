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
using WpfApplication1.Annotations;

namespace WpfApplication1
{
    /// <summary>
    /// DefactPercentControl.xaml 的交互逻辑
    /// </summary>
    public partial class DefactPercentControl : UserControl, INotifyPropertyChanged
    {
        public Func<ChartPoint, string> PointLabel { get; set; }
        public DefactPercentControl()
        {
            InitializeComponent();

            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            _passed = new ChartValues<double>();
            _failed = new ChartValues<double>();
            _passed.Add(0);
            _failed.Add(0);

            DataContext = this;
        }
        private ChartValues<double> _passed;
        private ChartValues<double> _failed;

        public void SetValues(double passed, double failed)
        {
            _passed.RemoveAt(0);
            _failed.RemoveAt(0);
            _passed.Add(passed);
            _failed.Add(failed);
        }

        public ChartValues<double> Passed
        {
            get { return _passed; }
            set
            {
                _passed = value;
                OnPropertyChanged(nameof(Passed));
            }
        }

        public ChartValues<double> Failed
        {
            get { return _failed; }
            set
            {
                _failed = value;
                OnPropertyChanged(nameof(Failed));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
