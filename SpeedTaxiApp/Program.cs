using SpeedTaxiApp;
using System;
using System.Text.Json;

class Program
{
    static string file = "taxiData.json";

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "🚕 SPEED TAXI SERVICE";

        Console.Clear();
        DrawLogo();
        LoadingProgressBar("Завантаження сервісу");

        TaxiService service = LoadData();
        Console.Clear();

        while (true)
        {
            DrawMenuHeader();
            DrawMenuOptions();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" Ваш вибір: ");
            Console.ResetColor();

            string option = Console.ReadLine();
            Console.WriteLine();

            try
            {
                switch (option)
                {
                    case "1": AddTariff(service); break;
                    case "2": AddClient(service); break;
                    case "3": AddTrip(service); break;
                    case "4": ShowClientCost(service); break;
                    case "5": ShowProfit(service); break;
                    case "6": SaveData(service, true); break;
                    case "0":
                        SaveData(service, true);
                        PrintMessage("Вихід із програми...", ConsoleColor.Cyan);
                        return;

                    default:
                        PrintMessage("❗️ Невідомий пункт меню.", ConsoleColor.Red);
                        break;
                }
            }
            catch (Exception ex)
            {
                PrintMessage($"⚠️ Помилка: {ex.Message}", ConsoleColor.Red);
            }

            Console.WriteLine("\nНатисніть Enter, щоб продовжити...");
            Console.ReadLine();
            Console.Clear();
        }
    }

    // ====================== ASCII ЛОГОТИП ======================

    static void DrawLogo()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(" ███████╗██████╗ ███████╗███████╗██████╗     ████████╗ █████╗ ██╗██╗");
        Console.WriteLine(" ██╔════╝██╔══██╗██╔════╝██╔════╝██╔══██╗    ╚══██╔══╝██╔══██╗██║██║");
        Console.WriteLine(" █████╗  ██████╔╝█████╗  █████╗  ██████╔╝       ██║   ███████║██║██║");
        Console.WriteLine(" ██╔══╝  ██╔══██╗██╔══╝  ██╔══╝  ██╔══██╗       ██║   ██╔══██║██║██║");
        Console.WriteLine(" ██║     ██║  ██║███████╗███████╗██║  ██║       ██║   ██║  ██║██║███████╗");
        Console.WriteLine(" ╚═╝     ╚═╝  ╚═╝╚══════╝╚══════╝╚═╝  ╚═╝       ╚═╝   ╚═╝  ╚═╝╚═╝╚══════╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    // ====================== ASCII МЕНЮ ======================

    static void DrawMenuHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║               🚕 SPEED TAXI              ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");
        Console.ResetColor();
    }

    static void DrawMenuOptions()
    {
        Console.WriteLine(" 1. ➕ Додати тариф");
        Console.WriteLine(" 2. 👤 Зареєструвати клієнта");
        Console.WriteLine(" 3. 🚖 Додати поїздку");
        Console.WriteLine(" 4. 📊 Показати вартість поїздок клієнта");
        Console.WriteLine(" 5. 💰 Показати загальний прибуток");
        Console.WriteLine(" 6. 💾 Зберегти дані вручну");
        Console.WriteLine(" 0. ❌ Вихід");
        Console.WriteLine("────────────────────────────────────────────");
    }

    static void PrintMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    // ====================== МЕТОДИ МЕНЮ ======================

    static void AddTariff(TaxiService service)
    {
        Console.Write(" Категорія авто: ");
        string cat = Console.ReadLine();

        Console.Write(" Ціна за км: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
            throw new Exception("Ціна повинна бути додатним числом!");

        service.AddTariff(new Tariff(cat, price));

        SaveData(service);
        PrintMessage("✔️ Тариф успішно додано!", ConsoleColor.Green);
    }

    static void AddClient(TaxiService service)
    {
        Console.Write(" Прізвище клієнта: ");
        string ln = Console.ReadLine();

        service.RegisterClient(ln);

        SaveData(service);
        PrintMessage("✔️ Клієнта зареєстровано!", ConsoleColor.Green);
    }

    static void AddTrip(TaxiService service)
    {
        Console.Write(" Прізвище клієнта: ");
        string ln = Console.ReadLine();

        Console.Write(" Категорія авто: ");
        string cat = Console.ReadLine();

        Console.Write(" Дистанція (км): ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal dist) || dist <= 0)
            throw new Exception("Дистанція повинна бути додатним числом!");

        service.AddTrip(ln, cat, dist);

        CarAnimation(); // 🚕 анімація руху

        SaveData(service);
        PrintMessage("✔️ Поїздку додано!", ConsoleColor.Green);
    }

    static void ShowClientCost(TaxiService service)
    {
        Console.Write(" Прізвище клієнта: ");
        string ln = Console.ReadLine();

        var client = service.Clients.FirstOrDefault(c => c.LastName == ln);

        if (client == null)
        {
            PrintMessage("❗️ Клієнта не знайдено!", ConsoleColor.Red);
            return;
        }

        PrintMessage($"💳 Загальна сума поїздок: {client.GetTotalCost()} грн", ConsoleColor.Green);
    }

    static void ShowProfit(TaxiService service)
    {
        PrintMessage($"💰 Загальний прибуток: {service.GetProfit()} грн", ConsoleColor.Yellow);
    }


    // ====================== JSON ======================

    static TaxiService LoadData()
    {
        if (!File.Exists(file))
            return new TaxiService();

        try
        {
            string json = File.ReadAllText(file);
            return JsonSerializer.Deserialize<TaxiService>(json) ?? new TaxiService();
        }
        catch
        {
            PrintMessage("⚠️ Помилка читання JSON. Створено нову базу.", ConsoleColor.Red);
            return new TaxiService();
        }
    }

    static void SaveData(TaxiService service, bool manual = false)
    {
        LoadingProgressBar("Збереження");

        string json = JsonSerializer.Serialize(service, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(file, json);

        if (manual)
            PrintMessage("💾 Дані збережено вручну!", ConsoleColor.Green);
        else
            PrintMessage("✔️ Автозбереження виконано!", ConsoleColor.Green);
    }


    // ====================== АНІМАЦІЇ ======================

    static void LoadingProgressBar(string text)
    {
        Console.Write(text + ": [");

        int total = 20;
        for (int i = 0; i < total; i++)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("█");
            Console.ResetColor();
            Thread.Sleep(60);
        }

        Console.WriteLine("] ✔️");
    }

    static void CarAnimation()
    {
        string[] frames =
        {
            "🚕          ",
            "  🚕        ",
            "    🚕      ",
            "      🚕    ",
            "        🚕  ",
            "          🚕💨"
        };

        foreach (string frame in frames)
        {
            Console.Write("\r" + frame);
            Thread.Sleep(120);
        }
        Console.WriteLine();
    }
}