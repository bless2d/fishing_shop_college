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
            // 2. Найти или зарегистрировать клиента
            // 3. Спросить: покупать отдельные товары или набор
            // 4. Если отдельные товары:
            //    - Показать каталог через ShowProductsByCategory()
            //    - В цикле добавлять товары в покупку
            // 5. Если набор:
            //    - Показать наборы через ShowFishingSets()
            //    - Выбрать набор
            //    - Проверить доступность набора
            //    - Добавить все товары из набора в покупку
            // 6. Рассчитать стоимость со скидкой
            // 7. Зафиксировать продажу через manager.RecordSale()
            // 8. Начислить бонусы клиенту
            // 9. Выдать чек с деталями покупки
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