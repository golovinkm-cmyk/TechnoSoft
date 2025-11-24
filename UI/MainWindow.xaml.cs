using Data.InMemory;
using Data.Interfaces; //
using Domain;
using ITService.Data.SqlServer;
using Microsoft.Extensions.Configuration;
using Services; //
using System.IO;
using System.Windows;

namespace UI
{
    public partial class MainWindow : Window
    {
        private readonly IRequestRepository _repository;

        public MainWindow()
        {
            // Получаем репозиторий через Dependency Injection
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.database.json")
                .Build();

            var factory = new ITServiceDbContextFactory();
            var context = factory.CreateDbContext(configuration);
            _repository = new RequestRepository(context);

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