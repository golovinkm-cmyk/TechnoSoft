using System.Windows;
using Data.Interfaces;
using Domain;
using System;
using System.Windows.Controls; // Для ComboBoxItem

namespace UI
{
    public partial class EditRepairRequestWindow : Window
    {
        private readonly IRequestRepository _repository;
        private readonly Request _request;
        private readonly bool _isNewRequest;

        // Событие, которое сообщит главному окну, что мы что-то сохранили
        public event EventHandler RequestSaved;

        public EditRepairRequestWindow(IRequestRepository repository, Request request)
        {
            InitializeComponent();
            _repository = repository;
            _request = request;

            // Проверяем, это новая заявка или редактирование существующей
            _isNewRequest = (request.Id == 0);

            if (_isNewRequest)
            {
                Title = "Новая заявка";
                // Устанавливаем значения по умолчанию
                dpDateAdded.SelectedDate = DateTime.Now;
                cmbStatus.SelectedItem = cmbStatus.Items[0]; // "Новая заявка"
                txtNumber.Text = "Будет присвоен";
                txtNumber.IsEnabled = false;
                dpDateAdded.IsEnabled = false;
            }
            else
            {
                Title = $"Редактирование заявки №{_request.Id}";
                LoadRequestData();
                txtNumber.IsEnabled = false;
                dpDateAdded.IsEnabled = false;
            }
        }

        // Загрузка данных в поля формы
        private void LoadRequestData()
        {
            txtNumber.Text = _request.Id.ToString();
            dpDateAdded.SelectedDate = _request.Date;
            cmbEquipmentType.Text = _request.Tipe;
            txtEquipmentModel.Text = _request.Model;
            txtProblemDescription.Text = _request.Description;
            txtClientFullName.Text = _request.ClientFullName;
            txtClientPhone.Text = _request.ClientPhone;
            cmbStatus.Text = _request.Status;
            txtResponsibleEngineer.Text = _request.Engineer;
            txtComments.Text = _request.Comments;
        }

        // Кнопка "Сохранить"
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Простая валидация (проверка)
            if (string.IsNullOrWhiteSpace(cmbEquipmentType.Text) ||
                string.IsNullOrWhiteSpace(txtEquipmentModel.Text) ||
                string.IsNullOrWhiteSpace(txtProblemDescription.Text) ||
                string.IsNullOrWhiteSpace(txtClientFullName.Text) ||
                string.IsNullOrWhiteSpace(txtClientPhone.Text) ||
                string.IsNullOrWhiteSpace(cmbStatus.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля (*).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Переносим данные из полей в наш объект _request
            _request.Tipe = cmbEquipmentType.Text;
            _request.Model = txtEquipmentModel.Text;
            _request.Description = txtProblemDescription.Text;
            _request.Status = cmbStatus.Text;
            _request.ClientFullName = txtClientFullName.Text;
            _request.ClientPhone = txtClientPhone.Text;
            _request.Engineer = txtResponsibleEngineer.Text;
            _request.Comments = txtComments.Text;

            // Если заявка новая - добавляем, если старая - обновляем
            if (_isNewRequest)
            {
                _request.Date = dpDateAdded.SelectedDate ?? DateTime.Now; // Дата уже установлена, но на всякий случай
                _repository.Add(_request);
            }
            else
            {
                _repository.Update(_request);
            }

            // Сообщаем главному окну, что мы сохранились
            RequestSaved?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        // Кнопка "Отмена"
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Просто закрываем окно без сохранения
            this.Close();

            // Нужно вернуть пользователя в главное меню, но у нас нет на него ссылки.
            // Самый простой способ - открыть его заново.
            // (Логику возврата в меню можно улучшить)
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}