using System.Windows;
using Data.InMemory; 

namespace UI
{
    public partial class MainWindow : Window
    {
        
        private readonly InMemoryRequestRepository _repository = new InMemoryRequestRepository();

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
            BtnListRequests_Click(sender, e);
        }

        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Я ещё не сделал :(", "Информация",
                             MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}