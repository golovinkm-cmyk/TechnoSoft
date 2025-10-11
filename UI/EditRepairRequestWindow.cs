using System.Windows;
using Data.Interfaces; 
using Domain;

namespace UI
{
    public partial class Window1 : Window
    {
        private Request _currentRequest;
        private readonly IRequestRepository _repository;
        private readonly RequestListWindow _parentListWindow;

       
        public Window1(RequestListWindow parent, IRequestRepository repository, Request request = null)
        {
            InitializeComponent();
            _parentListWindow = parent;
            _repository = repository;
            _currentRequest = request;

           
            if (_currentRequest != null)
            {
                Title = "Редактирование заявки №" + _currentRequest.Id;
                FillFormData();
            }
            else
            {
                Title = "Добавление новой заявки";
                dpDateAdded.SelectedDate = DateTime.Now;
                cmbStatus.SelectedIndex = 0; 
                
                txtNumber.Text = "Автоматически";
            }
        }

        private void FillFormData()
        {
            
            txtNumber.Text = _currentRequest.Id.ToString();
            dpDateAdded.SelectedDate = _currentRequest.Date;
            cmbEquipmentType.Text = _currentRequest.Tipe;
            txtEquipmentModel.Text = _currentRequest.Model;
            txtProblemDescription.Text = _currentRequest.Description;
            txtClientFullName.Text = _currentRequest.ClientFullName;
            txtClientPhone.Text = _currentRequest.ClientPhone;
            cmbStatus.Text = _currentRequest.Status;
            txtResponsibleEngineer.Text = _currentRequest.Engineer;
            txtComments.Text = _currentRequest.Comments;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                bool isNew = _currentRequest == null;

                
                Request requestToSave = _currentRequest ?? new Request();

               
                requestToSave.Date = dpDateAdded.SelectedDate ?? DateTime.Now;
                requestToSave.Tipe = cmbEquipmentType.Text;
                requestToSave.Model = txtEquipmentModel.Text.Trim();
                requestToSave.Description = txtProblemDescription.Text.Trim();
                requestToSave.ClientFullName = txtClientFullName.Text.Trim();
                requestToSave.ClientPhone = txtClientPhone.Text.Trim();
                requestToSave.Status = cmbStatus.Text;
                requestToSave.Engineer = txtResponsibleEngineer.Text.Trim();
                requestToSave.Comments = txtComments.Text.Trim();

                if (isNew)
                {
                  
                    _repository.Add(requestToSave);
                    MessageBox.Show($"Новая заявка №{requestToSave.Id} успешно добавлена!", "Успех",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                   
                    _repository.Update(requestToSave);
                    MessageBox.Show($"Заявка №{requestToSave.Id} успешно обновлена!", "Успех",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }

                
                _parentListWindow.LoadRequests();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                                 MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private bool ValidateForm()
        {
            

            if (string.IsNullOrWhiteSpace(cmbEquipmentType.Text))
            {
                MessageBox.Show("Выберите тип оборудования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbEquipmentType.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEquipmentModel.Text))
            {
                MessageBox.Show("Введите модель оборудования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEquipmentModel.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProblemDescription.Text))
            {
                MessageBox.Show("Введите описание проблемы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtProblemDescription.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtClientFullName.Text))
            {
                MessageBox.Show("Введите ФИО клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtClientFullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtClientPhone.Text))
            {
                MessageBox.Show("Введите номер телефона", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtClientPhone.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbStatus.Text))
            {
                MessageBox.Show("Выберите статус заявки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbStatus.Focus();
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Отменить изменения и закрыть форму?", "Подтверждение",
                                 MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}