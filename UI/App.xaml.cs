using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Data.Interfaces;
using ITService.Data.SqlServer;
using Domain;
using System.Linq;

namespace UI
{
    public partial class App : Application
    {
        private IRequestRepository _requestRepository;
        private ITServiceDbContext _dbContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // 1. Чтение конфигурации
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.database.json")
                    .Build();

                // 2. Создание DbContext
                var factory = new ITServiceDbContextFactory();
                _dbContext = factory.CreateDbContext(configuration);

                // 3. АВТОМАТИЧЕСКОЕ СОЗДАНИЕ БАЗЫ (без миграций)
                _dbContext.Database.EnsureCreated();

                // 4. Создание репозиториев
                _requestRepository = new RequestRepository(_dbContext);

                // 5. Заполнение тестовыми данными (если БД пустая)
                SeedInitData();

                // 6. Запуск главного окна
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании базы данных: {ex.Message}\n\nПриложение будет использовать временное хранилище.",
                    "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Warning);

                // Fallback на InMemory репозиторий
                var inMemoryRepo = new Data.InMemory.InMemoryRequestRepository();
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        private void SeedInitData()
        {
            // Проверяем, есть ли уже данные в БД
            if (_requestRepository.GetAll().Any())
            {
                return; // Данные уже есть, пропускаем заполнение
            }

            // Добавляем тестовые данные
            var testRequest = new Request(
                id: 0, // EF Core сам назначит ID
                date: DateTime.Now,
                tipe: "Компьютер",
                model: "Dell OptiPlex 7010",
                description: "Тестовая заявка",
                status: "Новая заявка",
                clientfullname: "Тестовый Клиент",
                clientphone: "+7 (999) 123-45-67",
                engineer: "",
                comments: ""
            );

            _requestRepository.Add(testRequest);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _dbContext?.Dispose();
            base.OnExit(e);
        }
    }
}