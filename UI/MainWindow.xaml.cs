using System.Windows;
using Data.InMemory; // Создаем экземпляр конкретной реализации здесь

namespace UI
{
    public partial class MainWindow : Window
    {
        // Создаем единственный экземпляр репозитория в памяти (Singleton для лабораторной)
        private readonly InMemoryRequestRepository _repository = new InMemoryRequestRepository();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Кнопка "Список заявок"
        private void BtnListRequests_Click(object sender, RoutedEventArgs e)
        {
            // Переход к форме списка
            var listWindow = new RequestListWindow(_repository);
            listWindow.Show();
            this.Close(); // Закрываем главное меню
        }

        // Кнопка "Добавить новую заявку" - логичнее сразу перейти к списку
        private void BtnAddRequest_Click(object sender, RoutedEventArgs e)
        {
            BtnListRequests_Click(sender, e);
        }

        private void BtnStatistics_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функционал статистики в разработке", "Информация",
                             MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}