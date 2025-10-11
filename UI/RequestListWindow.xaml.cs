using System.Windows;
using Data.Interfaces;
using Data.InMemory; 
using Domain;

namespace UI
{
    public partial class RequestListWindow : Window
    {
        
        private readonly IRequestRepository _repository;

        public RequestListWindow(IRequestRepository repository)
        {
            InitializeComponent();
            _repository = repository;
           
            LoadRequests();
        }

        
        public void LoadRequests()
        {
            var requests = _repository.GetAll();
            RequestsDataGrid.ItemsSource = requests;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
          
            var addWindow = new Window1(this, _repository);
            addWindow.ShowDialog();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is Request selectedRequest)
            {
               
                var editWindow = new Window1(this, _repository, selectedRequest);
                editWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите заявку для редактирования.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (RequestsDataGrid.SelectedItem is Request selectedRequest)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить заявку №{selectedRequest.Id}?", "Подтверждение удаления",
                                             MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (_repository.Delete(selectedRequest.Id))
                    {
                        MessageBox.Show("Заявка успешно удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadRequests(); 
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении заявки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите заявку для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadRequests();
        }
    }
}
