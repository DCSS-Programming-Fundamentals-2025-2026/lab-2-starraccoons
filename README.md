# CafeRush

Консольний застосунок для управління кав'ярнею. Дозволяє приймати замовлення, вести облік складу, формувати звіти та керувати меню напоїв.

## Команда

Рекун Катерина -- QA — тестовий проєкт `CafeRush.Tests` 
Турчик Зоряна -- Core — upgrade партнерського репозиторію 

## Структура проєкту


CafeRush/
├── Contracts/
│   └── IRecipe.cs
├── Domain/
│   ├── Drink.cs
│   ├── IngredientType.cs
│   ├── MenuItemBase.cs
│   ├── Order.cs
│   └── Stock.cs
├── Exceptions/
│   └── CafeExceptions.cs
├── Services/
│   ├── MenuService.cs
│   ├── OrderService.cs
│   ├── OrderServiceOperations.cs
│   └── ReportService.cs
├── Cafe.cs
├── ConsoleMenu.cs
└── Program.cs

CafeRush.Tests/
├── MenuServiceTests.cs
├── StockAndOrderTests.cs
└── IntegrationAndReportTests.cs


## Як запустити проєкт

1. Відкрити рішення у Visual Studio
2. Встановити `CafeRush` як стартовий проєкт
3. Натиснути `Ctrl + F5`

## Як запустити тести

### Visual Studio
1. Відкрити `Test Explorer` через меню `Test => Test Explorer`
2. Натиснути `Run All Tests` або `Ctrl + R, A`
3. Всі 30 тестів мають позначитись зеленим

### Командний рядок
bash
cd CafeRush.Tests
dotnet test

## Тестовий проєкт

`MenuServiceTests.cs`: додавання та видалення напоїв, зсув масиву = 10 тестів
`StockAndOrderTests.cs`: логіка складу, підрахунок суми замовлень = 10 тестів
`IntegrationAndReportTests.cs`: інтеграційні сценарії, стан сервісів, resize масиву = 10 тестів

Кожен тест написаний за структурою **AAA**:

Arrange — підготовка початкових даних
Act     — виконання однієї конкретної дії
Assert  — перевірка очікуваного результату

## Меню застосунку

Пункт| Дія 
|1   | Створити замовлення 
|2   | Показати склад 
|3   | Звіт за день 
|4   | Показати меню напоїв 
|5   | Створити власний напій 
|6   | Поповнити склад 
|7   | Перезапуск дня 
|8   | Додати напій у меню 
|9   | Видалити напій з меню 
|0   | Вихід 