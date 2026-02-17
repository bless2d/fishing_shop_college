// TODO:
// 1. Реализовать отображение товаров по категориям
// 2. Реализовать процесс покупки товаров и наборов
// 3. Реализовать консультацию по подбору снастей

using System;
using System.Collections.Generic;

namespace FishingStore
{
    public class StoreMenu
    {
        private StoreManager manager;
        
        public StoreMenu()
        {
            manager = new StoreManager();
            InitializeStoreData();
        }
        
        private void InitializeStoreData()
        {
            // Инициализация тестовых данных - товары
            manager.AddProduct(new FishingProduct(1, "Спиннинг Shimano", "Shimano", 4500, 10, "Удочки"));
            manager.AddProduct(new FishingProduct(2, "Катушка Daiwa", "Daiwa", 3200, 15, "Катушки"));
            manager.AddProduct(new FishingProduct(3, "Леска 0.25мм", "Salmo", 350, 50, "Лески"));
            manager.AddProduct(new FishingProduct(4, "Воблер Rapala", "Rapala", 850, 30, "Приманки"));
            manager.AddProduct(new FishingProduct(5, "Рыболовный жилет", "Norfin", 2800, 8, "Одежда"));
            manager.AddProduct(new FishingProduct(6, "Подсачек", "Mikado", 1200, 12, "Аксессуары"));
            
            // Инициализация рыболовных наборов
            FishingSet beginnerSet = new FishingSet(1, "Набор новичка", "Спиннинг", "Начальный", 
                "Все необходимое для начала рыбалки на спиннинг");
            // TODO: Добавить товары в набор
            beginnerSet.AddItem(manager.GetAllProducts().Find(p => p.Id == 1), 1); // Спиннинг
            beginnerSet.AddItem(manager.GetAllProducts().Find(p => p.Id == 2), 1); // Катушка
            beginnerSet.AddItem(manager.GetAllProducts().Find(p => p.Id == 3), 1); // Леска
            beginnerSet.AddItem(manager.GetAllProducts().Find(p => p.Id == 4), 2); // Приманки
            manager.AddFishingSet(beginnerSet);
        }
        
        // TODO 1: Показать товары по категориям
        public void ShowProductsByCategory()
        {
            Console.WriteLine("=== КАТАЛОГ ТОВАРОВ ДЛЯ РЫБАЛКИ ===");
            
            // Получить все товары через manager.GetAllProducts()
            // Сгруппировать товары по типам (удочки, катушки, приманки и т.д.)
            // Для каждой категории вывести товары с детальной информацией
            var products = manager.GetAllProducts();
            var grouped = products.GroupBy(p => p.ProductType);
            foreach (var group in grouped)
            {
                Console.WriteLine($"\n--- {group.Key} ---");
                foreach (var product in group)
                {
                    Console.WriteLine(product.ToString());
                }
            }
        }
        
        // TODO 2: Показать рыболовные наборы
        public void ShowFishingSets()
        {
            Console.WriteLine("=== РЫБОЛОВНЫЕ НАБОРЫ ===");
            
            // Получить все наборы через manager.GetAllFishingSets()
            // Для каждого набора вызвать ShowSetComposition()
            var sets = manager.GetAllFishingSets();
            foreach (var set in sets)
            {
                set.ShowSetComposition();
                Console.WriteLine();
            }
        }
        
        // TODO 2: Оформить покупку
        public void ProcessPurchase()
        {
            Console.WriteLine("=== ОФОРМЛЕНИЕ ПОКУПКИ ===");
            
            // 1. Запросить телефон клиента
            Console.Write("Введите телефон клиента: ");
            string phone = Console.ReadLine();
            Customer customer = manager.FindCustomerByPhone(phone);
            if (customer == null)
            {
                Console.Write("Клиент не найден. Регистрация нового клиента.\nВведите ФИО: ");
                string fullName = Console.ReadLine();
                Console.Write("Опыт (новичок/средний/опытный): ");
                string experience = Console.ReadLine();
                Console.Write("Тип ловли: ");
                string fishingType = Console.ReadLine();
                customer = manager.RegisterCustomer(fullName, phone, experience, fishingType);
            }
            
            // 2. Спросить: покупать отдельные товары или набор
            Console.Write("Выберите тип покупки (1 - отдельные товары, 2 - набор): ");
            string choice = Console.ReadLine();
            
            // Создать покупку
            var purchase = customer.CreatePurchase(manager.GetNextPurchaseNumber());
            decimal totalPrice = 0;
            
            if (choice == "1")
            {
                // Отдельные товары
                ShowProductsByCategory();
                bool adding = true;
                while (adding)
                {
                    Console.Write("Введите ID товара (или 0 для завершения): ");
                    if (int.TryParse(Console.ReadLine(), out int id) && id != 0)
                    {
                        var product = manager.GetAllProducts().Find(p => p.Id == id);
                        if (product != null)
                        {
                            Console.Write("Количество: ");
                            if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
                            {
                                if (customer.AddToPurchase(purchase, product, qty))
                                {
                                    totalPrice += product.Price * qty;
                                    manager.RecordProductSale(product, qty);
                                }
                                else
                                {
                                    Console.WriteLine("Недостаточно на складе.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Товар не найден.");
                        }
                    }
                    else
                    {
                        adding = false;
                    }
                }
            }
            else if (choice == "2")
            {
                // Набор
                ShowFishingSets();
                Console.Write("Введите ID набора: ");
                if (int.TryParse(Console.ReadLine(), out int setId))
                {
                    var set = manager.GetAllFishingSets().Find(s => s.Id == setId);
                    if (set != null && set.IsSetAvailable())
                    {
                        // Добавить товары из набора в покупку
                        foreach (var item in set.GetItems())
                        {
                            customer.AddToPurchase(purchase, item.Key, item.Value);
                            totalPrice += item.Key.Price * item.Value;
                            manager.RecordProductSale(item.Key, item.Value);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Набор недоступен.");
                        return;
                    }
                }
            }
            
            // 6. Рассчитать стоимость со скидкой
            decimal discount;
            decimal finalPrice = customer.CalculatePurchaseTotal(purchase, out discount);
            
            // 7. Зафиксировать продажу
            manager.RecordSale(finalPrice);
            
            // 8. Начислить бонусы
            customer.AddBonusPoints(finalPrice);
            purchase.BonusEarned = finalPrice * 0.01m;
            purchase.TotalAmount = finalPrice;
            
            // 9. Завершить покупку
            customer.CompletePurchase(purchase);
            
            // Выдать чек
            Console.WriteLine("\n=== ЧЕК ПОКУПКИ ===");
            Console.WriteLine($"Номер покупки: {purchase.PurchaseNumber}");
            Console.WriteLine($"Клиент: {customer.FullName}");
            foreach (var item in purchase.Items)
            {
                Console.WriteLine($"{item.Product.Name} x{item.Quantity} - {item.Price * item.Quantity} руб.");
            }
            Console.WriteLine($"Скидка: {discount} руб.");
            Console.WriteLine($"Итого: {finalPrice} руб.");
            Console.WriteLine($"Начислено бонусов: {purchase.BonusEarned}");
        }
            
        }
        
        // TODO 3: Консультация по подбору снастей
        public void ProvideConsultation()
        {
            Console.WriteLine("=== КОНСУЛЬТАЦИЯ ПО ПОДБОРУ СНАСТЕЙ ===");
            
            // 1. Запросить опыт рыболова (новичок, любитель, профи)
            // 2. Запросить тип ловли (спиннинг, фидер, поплавок, зимняя)
            // 3. Запросить бюджет
            // 4. Подобрать товары из каталога по параметрам
            // 5. Показать рекомендованные товары
            // 6. Предложить сформировать набор
        }
        
        // TODO 3: Показать статистику магазина
        public void ShowStoreStats()
        {
            Console.WriteLine("=== СТАТИСТИКА МАГАЗИНА ===");
            
            // Вывести общую выручку через manager.GetTotalRevenue()
            // Вывести количество зарегистрированных клиентов
            // Вывести самые популярные категории товаров
            // Вывести товары с низким остатком на складе (< 5 шт.)
        }
        
        // Готовый метод - главное меню
        public void ShowMainMenu()
        {
            bool running = true;
            
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== РЫБОЛОВНЫЙ МАГАЗИН 'КЛЕВ' ===");
                Console.WriteLine("1. Каталог товаров");
                Console.WriteLine("2. Рыболовные наборы");
                Console.WriteLine("3. Оформить покупку");
                Console.WriteLine("4. Консультация по снастям");
                Console.WriteLine("5. Статистика магазина");
                Console.WriteLine("6. Поиск клиента");
                Console.WriteLine("7. Выход");
                Console.Write("Выберите: ");
                
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        ShowProductsByCategory();
                        break;
                    case "2":
                        ShowFishingSets();
                        break;
                    case "3":
                        ProcessPurchase();
                        break;
                    case "4":
                        ProvideConsultation();
                        break;
                    case "5":
                        ShowStoreStats();
                        break;
                    case "6":
                        SearchCustomer();
                        break;
                    case "7":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
                
                if (running)
                {
                    Console.WriteLine("\nНажмите Enter...");
                    Console.ReadLine();
                }
            }
        }
        
        // Метод поиска клиента
        private void SearchCustomer()
        {
            Console.Write("Введите телефон клиента: ");
            string phone = Console.ReadLine();
            
            Customer customer = manager.FindCustomerByPhone(phone);
            if (customer != null)
            {
                customer.ShowCustomerInfo();
            }
            else
            {
                Console.WriteLine("Клиент не найден");
            }
        }
    }
}