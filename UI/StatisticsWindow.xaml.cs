using System.Windows;
using Services;
using Data.Interfaces;
using System;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UI
{
    public partial class StatisticsWindow : Window, INotifyPropertyChanged
    {
        private readonly StatisticsService _statisticsService;

        private DateTime? _startDate;
        private DateTime? _endDate;
        private PlotModel _statusPlotModel;
        private PlotModel _engineerPlotModel;
        private PlotModel _monthPlotModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public PlotModel StatusPlotModel
        {
            get => _statusPlotModel;
            set
            {
                if (_statusPlotModel != value)
                {
                    _statusPlotModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public PlotModel EngineerPlotModel
        {
            get => _engineerPlotModel;
            set
            {
                if (_engineerPlotModel != value)
                {
                    _engineerPlotModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public PlotModel MonthPlotModel
        {
            get => _monthPlotModel;
            set
            {
                if (_monthPlotModel != value)
                {
                    _monthPlotModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public StatisticsWindow(StatisticsService statisticsService)
        {
            InitializeComponent();
            _statisticsService = statisticsService;
            DataContext = this;
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            var filter = new RequestFilter
            {
                StartDate = StartDate,
                EndDate = EndDate
            };

            LoadStatusChart(filter);
            LoadEngineerChart(filter);
            LoadMonthChart(filter);

            // Теперь свойства уведомляют об изменениях через сеттеры
            // Не нужно вызывать InvalidatePlot вручную, так как обновление происходит через привязку данных
        }

        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            LoadStatistics();
        }

        private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
        {
            StartDate = null;
            EndDate = null;

            // Если у вас есть DatePicker элементы с именами dpStartDate и dpEndDate
            if (dpStartDate != null) dpStartDate.SelectedDate = null;
            if (dpEndDate != null) dpEndDate.SelectedDate = null;

            LoadStatistics();
        }

        private void BtnBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        // --- Методы загрузки диаграмм ---

        private void LoadStatusChart(RequestFilter filter)
        {
            var data = _statisticsService.GetRequestsByStatus(filter);
            var plotModel = new PlotModel { Title = "Заявки по статусам" };

            var pieSeries = new PieSeries
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.5,
                AngleSpan = 360,
                StartAngle = 0
            };

            foreach (var item in data)
            {
                pieSeries.Slices.Add(new PieSlice(item.Status, item.Count) { IsExploded = false });
            }

            plotModel.Series.Add(pieSeries);
            StatusPlotModel = plotModel; // Установка свойства вызовет уведомление
        }

        private void LoadEngineerChart(RequestFilter filter)
        {
            var data = _statisticsService.GetRequestsByEngineer(filter);
            var plotModel = new PlotModel { Title = "Загрузка инженеров" };

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Title = "Инженеры"
            };

            foreach (var item in data)
            {
                categoryAxis.Labels.Add(item.EngineerName);
            }
            plotModel.Axes.Add(categoryAxis);

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MinimumPadding = 0.1,
                MaximumPadding = 0.1,
                Title = "Количество заявок"
            });

            var barSeries = new BarSeries
            {
                Title = "Количество заявок",
                FillColor = OxyColor.FromRgb(79, 129, 189)
            };

            foreach (var item in data)
            {
                barSeries.Items.Add(new BarItem { Value = item.Count });
            }

            plotModel.Series.Add(barSeries);
            EngineerPlotModel = plotModel; // Установка свойства вызовет уведомление
        }

        private void LoadMonthChart(RequestFilter filter)
        {
            var data = _statisticsService.GetRequestsByMonth(filter);
            var plotModel = new PlotModel { Title = "Динамика заявок по месяцам" };

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Angle = -15,
                Title = "Месяцы"
            };

            foreach (var item in data)
            {
                categoryAxis.Labels.Add(item.GetMonthName());
            }
            plotModel.Axes.Add(categoryAxis);

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Количество заявок",
                MinimumPadding = 0.1,
                MaximumPadding = 0.1
            });

            var lineSeries = new LineSeries
            {
                Title = "Количество заявок",
                Color = OxyColor.FromRgb(79, 129, 189),
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerFill = OxyColor.FromRgb(79, 129, 189)
            };

            for (int i = 0; i < data.Count; i++)
            {
                lineSeries.Points.Add(new DataPoint(i, data[i].Count));
            }

            plotModel.Series.Add(lineSeries);
            MonthPlotModel = plotModel; // Установка свойства вызовет уведомление
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}