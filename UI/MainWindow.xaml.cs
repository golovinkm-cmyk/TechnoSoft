using System.Windows;
using Data.InMemory;
using Data.Interfaces; //
using Domain;
using Services; //

namespace UI
{
    public partial class MainWindow : Window
    {
        //
        //
        private readonly IRequestRepository _repository = new InMemoryRequestRepository();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnListRequests_Click(object sender, RoutedEventArgs e)
        {
            var listWindow = new RequestListWindow(_repository);
            listWindow.Show();
            this.Close();
        }

        private void BtnAddRequest_Click(object sender, RoutedEventArgs e)
        {
            var newRequest = new Request();
            var editWindow = new EditRepairRequestWindow(_repository, newRequest);

            editWindow.RequestSaved += (s, args) =>
            {
                BtnListRequests_Click(sender, e);
            };

            editWindow.Show();
            this.Close();
        }

        //
        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            //
            // 1.
            var statisticsService = new StatisticsService(_repository);

            //
            // 2.
            var statsWindow = new StatisticsWindow(statisticsService);

            //
            // 3.
            statsWindow.Show();
            this.Close();
        }
    }
}