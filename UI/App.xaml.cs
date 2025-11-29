using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepairService.Data.SqlServer;
using Domain;
using System.IO;
using System.Windows;

namespace UI;

public partial class App : Application
{
    private IRequestRepository _requestRepository;
    private RepairServiceDbContext _dbContext;

    public static IRequestRepository RequestRepository { get; private set; }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        // 1. Чтение конфигурации
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.database.json")
            .Build();

        // 2. Создание DbContext
        var factory = new RepairServiceDbContextFactory();
        _dbContext = factory.CreateDbContext(configuration);

        // 3. Автоматическое применение миграций
        try
        {
            _dbContext.Database.Migrate();
            Console.WriteLine("Миграции успешно применены");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка миграции: {ex.Message}");
        }

        // 4. Создание репозиториев
        _requestRepository = new RequestRepository(_dbContext);
        RequestRepository = _requestRepository;

        // 5. Заполнение тестовыми данными
        SeedInitData();

        // 6. Создание главного окна
        var mainWindow = new MainWindow(_requestRepository);
        mainWindow.Show();
    }

    private void SeedInitData()
    {
        // Проверяем, есть ли уже данные в БД
        if (_requestRepository.GetAll().Any())
        {
            return; // Данные уже есть
        }

        // Добавляем тестовые данные
        var statuses = new[] { "Новая заявка", "В процессе ремонта", "Ожидание запчастей", "Готово к выдаче", "Завершена" };
        var engineers = new[] { "Петров С.А.", "Сидоров В.Н.", "Иванова Е.П.", "Кузнецов Д.И." };
        var clients = new[]
        {
            new { Name = "Иванов Иван Иванович", Phone = "+7 (903) 123-45-67" },
            new { Name = "Сидорова Елена Петровна", Phone = "+7 (926) 987-65-43" },
            new { Name = "Михайлов Артем Олегович", Phone = "+7 (916) 555-44-33" }
        };
        var models = new[]
        {
            ("Компьютер", "Dell OptiPlex 7010"),
            ("Принтер", "HP LaserJet Pro M404dn"),
            ("Ноутбук", "Lenovo ThinkPad X1"),
            ("Монитор", "Samsung Odyssey G7"),
            ("МФУ", "Kyocera Ecosys M2040dn")
        };

        var random = new Random();

        for (int i = 0; i < 20; i++) // Меньше записей для теста
        {
            var client = clients[random.Next(clients.Length)];
            var model = models[random.Next(models.Length)];
            var status = statuses[random.Next(statuses.Length)];

            var request = new Request(
                id: 0, // ID будет сгенерирован БД
                date: DateTime.Now.AddDays(-random.Next(0, 180)).AddHours(random.Next(-12, 12)),
                tipe: model.Item1,
                model: model.Item2,
                description: $"Тестовая проблема {i + 1}",
                status: status,
                clientfullname: client.Name,
                clientphone: client.Phone,
                engineer: status == "Новая заявка" ? "" : engineers[random.Next(engineers.Length)],
                comments: ""
            );

            _requestRepository.Add(request);
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _dbContext?.Dispose();
        base.OnExit(e);
    }
}