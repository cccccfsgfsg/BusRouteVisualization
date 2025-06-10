BusRouteVisualization
Описание проекта
BusRouteVisualization — это приложение на C#, разработанное с использованием Windows Forms, которое позволяет визуализировать маршруты общественного транспорта на карте. Приложение поддерживает создание новых маршрутов, добавление точек, отображение маршрутов на PictureBox и анимацию движения автобуса. Данные о маршрутах сохраняются в файл routes.json для последующего использования.

Требования

Среда разработки: Visual Studio 2019 или новее.
Фреймворк: .NET Framework 4.7.2 или новее (рекомендуется .NET 4.8).
Зависимости: Пакет Newtonsoft.Json (добавляется через NuGet).


Установка
1. Клонирование репозитория

Скачайте или склонируйте проект с помощью Git:git clone <URL_вашего_репозитория>


Или загрузите ZIP-архив и распакуйте его.

2. Установка зависимостей

Откройте проект в Visual Studio.
В меню выберите Tools > NuGet Package Manager > Manage NuGet Packages for Solution.
Найдите и установите пакет Newtonsoft.Json.

3. Сборка проекта

Нажмите Build > Build Solution или используйте сочетание клавиш Ctrl+Shift+B.
Убедитесь, что нет ошибок компиляции.

4. Запуск

Нажмите F5 для запуска в режиме отладки.
Или найдите сгенерированный .exe-файл в папке bin\Debug (или bin\Release).


Использование
Интерфейс

PictureBox (picMap): Отображает карту с маршрутами и точками.
ComboBox (cmbRoutes): Список доступных маршрутов.
TextBox (txtRouteName): Поле для ввода имени нового маршрута (с placeholder "Название маршрута").
Кнопки:
Добавить точку (btnAddPoint): Добавляет новую точку к текущему маршруту.
Создать маршрут (btnCreateRoute): Создаёт новый маршрут с указанным именем.
Загрузить маршрут (btnLoadRoute): Загружает выбранный маршрут для отображения.
Анимировать (btnAnimate): Запускает анимацию движения автобуса.



Пошаговая инструкция

Создание маршрута:

Введите имя маршрута в txtRouteName.
Нажмите Создать маршрут.
Новый маршрут появится в списке cmbRoutes.


Добавление точек:

Нажмите Добавить точку, введите название точки в диалоговом окне.
Кликните на picMap, чтобы установить координаты точки.


Загрузка маршрута:

Выберите маршрут в cmbRoutes.
Нажмите Загрузить маршрут, чтобы отобразить его на карте.


Анимация:

Убедитесь, что маршрут содержит минимум 2 точки.
Нажмите Анимировать, чтобы увидеть движение автобуса (зелёная точка).


Сохранение:

Все изменения автоматически сохраняются в файл routes.json при закрытии формы или создании нового маршрута.




Сохранение данных

Файл: routes.json.
Местоположение:
Во время разработки: bin\Debug\routes.json или bin\Release\routes.json (в зависимости от конфигурации сборки).
После установки: в той же директории, где находится .exe-файл, если не изменён путь.


Динамический путь (опционально):
Можно настроить сохранение в папке пользователя (например, %AppData%\BusRouteVisualization\routes.json) путём изменения кода:private static readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BusRouteVisualization", "routes.json");


Требуется добавить using System.IO; и создать директорию, если её нет.


Формат файла: Данные сериализуются с помощью Newtonsoft.Json в читаемом формате. Пример:[
  {
    "Id": 1,
    "Name": "Маршрут 1: Центр - Периферия",
    "Points": [
      { "Id": 1, "Name": "Центральная площадь", "X": 50.0, "Y": 50.0, "TimeFromStart": "00:00:00" },
      { "Id": 2, "Name": "Улица Ленина", "X": 150.0, "Y": 100.0, "TimeFromStart": "00:05:00" }
    ]
  }
]




Структура кода

Form1.cs: Основная логика приложения, включая визуализацию и анимацию.
Form1.Designer.cs: Генерируемый код для элементов формы.
Program.cs: Точка входа приложения.
Зависимости: Newtonsoft.Json для работы с JSON.


Возможные проблемы и решения

Файл не сохраняется:
Проверьте права доступа к папке bin\Debug. Используйте динамический путь в %AppData%.


Ошибки компиляции:
Убедитесь, что Newtonsoft.Json установлен.


Анимация не работает:
Убедитесь, что маршрут содержит минимум 2 точки.




Описание кода
Ниже приведён пример основного кода приложения BusRouteVisualization, включая структуру классов и их назначение. Этот код находится в файле Form1.cs.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BusRouteVisualization
{
    public partial class Form1 : Form
    {
        private List<BusRoute> routes = new List<BusRoute>(); // Список всех маршрутов
        private BusRoute selectedRoute; // Выбранный маршрут для отображения
        private BusRoute newRoute; // Новый создаваемый маршрут
        private Timer animationTimer; // Таймер для анимации движения автобуса
        private int currentPointIndex = 0; // Текущий индекс точки в анимации
        private float animationProgress = 0f; // Прогресс анимации между точками
        private PointF busPosition; // Позиция автобуса на карте
        private const string FilePath = "routes.json"; // Путь к файлу для сохранения данных

        public Form1()
        {
            InitializeComponent();
            picMap.Width = 541;
            picMap.Height = 368;
            picMap.MouseClick += PicMap_MouseClick;
            LoadRoutesFromFile(); // Загружаем маршруты при старте
            InitializeAnimationTimer();
        }

        private void InitializeAnimationTimer()
        {
            animationTimer = new Timer
            {
                Interval = 50
            };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void LoadRoutesFromFile()
        {
            if (File.Exists(FilePath))
            {
                try
                {
                    string jsonString = File.ReadAllText(FilePath);
                    routes = JsonConvert.DeserializeObject<List<BusRoute>>(jsonString);
                    if (routes != null)
                    {
                        cmbRoutes.DataSource = null;
                        cmbRoutes.DataSource = routes;
                        cmbRoutes.DisplayMember = "Name";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки маршрутов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveRoutesToFile()
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(routes, Formatting.Indented);
                File.WriteAllText(FilePath, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения маршрутов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTestData()
        {
            if (routes.Count == 0) // Добавляем тестовые данные только если файл пуст
            {
                var route1 = new BusRoute { Id = 1, Name = "Маршрут 1: Центр - Периферия" };
                route1.Points.Add(new RoutePoint { Id = 1, Name = "Центральная площадь", X = 50, Y = 50, TimeFromStart = TimeSpan.FromMinutes(0) });
                route1.Points.Add(new RoutePoint { Id = 2, Name = "Улица Ленина", X = 150, Y = 100, TimeFromStart = TimeSpan.FromMinutes(5) });
                route1.Points.Add(new RoutePoint { Id = 3, Name = "Торговый центр", X = 300, Y = 150, TimeFromStart = TimeSpan.FromMinutes(10) });
                route1.Points.Add(new RoutePoint { Id = 4, Name = "Жилой район", X = 450, Y = 300, TimeFromStart = TimeSpan.FromMinutes(20) });

                var route2 = new BusRoute { Id = 2, Name = "Маршрут 2: Вокзал - Аэропорт" };
                route2.Points.Add(new RoutePoint { Id = 5, Name = "Вокзал", X = 30, Y = 30, TimeFromStart = TimeSpan.FromMinutes(0) });
                route2.Points.Add(new RoutePoint { Id = 6, Name = "Шоссе", X = 200, Y = 50, TimeFromStart = TimeSpan.FromMinutes(10) });
                route2.Points.Add(new RoutePoint { Id = 7, Name = "Аэропорт", X = 500, Y = 20, TimeFromStart = TimeSpan.FromMinutes(25) });

                routes.Add(route1);
                routes.Add(route2);

                cmbRoutes.DataSource = null;
                cmbRoutes.DataSource = routes;
                cmbRoutes.DisplayMember = "Name";
                SaveRoutesToFile(); // Сохраняем тестовые данные в файл
            }
        }

        private void BtnAddPoint_Click(object sender, EventArgs e)
        {
            if (newRoute == null)
            {
                MessageBox.Show("Сначала создайте маршрут.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var pointName = Prompt.ShowDialog("Введите название точки:", "Новая точка");
            if (!string.IsNullOrEmpty(pointName))
            {
                newRoute.Points.Add(new RoutePoint
                {
                    Id = newRoute.Points.Count + 1,
                    Name = pointName,
                    X = 0,
                    Y = 0,
                    TimeFromStart = TimeSpan.FromMinutes(newRoute.Points.Count * 5)
                });
                MessageBox.Show("Кликните на карте, чтобы установить точку.", "Указание координат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void PicMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (newRoute != null && newRoute.Points.Count > 0 && newRoute.Points[newRoute.Points.Count - 1].X == 0)
            {
                var lastPoint = newRoute.Points[newRoute.Points.Count - 1];
                lastPoint.X = e.X;
                lastPoint.Y = e.Y;
                DrawRoute(newRoute);
                MessageBox.Show($"Точка {lastPoint.Name} добавлена на ({e.X}, {e.Y}).", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCreateRoute_Click(object sender, EventArgs e)
        {
            var routeName = txtRouteName.Text;
            if (string.IsNullOrEmpty(routeName) || routeName == "Название маршрута")
            {
                MessageBox.Show("Введите название маршрута.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            newRoute = new BusRoute { Id = routes.Count + 1, Name = routeName };
            routes.Add(newRoute);
            cmbRoutes.DataSource = null;
            cmbRoutes.DataSource = routes;
            cmbRoutes.DisplayMember = "Name";
            cmbRoutes.SelectedItem = newRoute;
            selectedRoute = newRoute;
            SaveRoutesToFile(); // Сохраняем после создания нового маршрута
            MessageBox.Show($"Маршрут {routeName} создан. Добавляйте точки.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLoadRoute_Click(object sender, EventArgs e)
        {
            if (cmbRoutes.SelectedItem is BusRoute route)
            {
                selectedRoute = route;
                newRoute = null;
                DrawRoute(selectedRoute);
                MessageBox.Show($"Загружен маршрут: {selectedRoute.Name}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Выберите маршрут.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnimate_Click(object sender, EventArgs e)
        {
            if (selectedRoute == null || selectedRoute.Points.Count < 2)
            {
                MessageBox.Show("Выберите маршрут с минимум двумя точками.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            currentPointIndex = 0;
            animationProgress = 0f;
            busPosition = new PointF(selectedRoute.Points[0].X, selectedRoute.Points[0].Y);
            animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (currentPointIndex >= selectedRoute.Points.Count - 1)
            {
                animationTimer.Stop();
                return;
            }

            var startPoint = selectedRoute.Points[currentPointIndex];
            var endPoint = selectedRoute.Points[currentPointIndex + 1];
            animationProgress += 0.05f;

            if (animationProgress >= 1f)
            {
                animationProgress = 0f;
                currentPointIndex++;
                if (currentPointIndex >= selectedRoute.Points.Count - 1)
                {
                    animationTimer.Stop();
                    return;
                }
                startPoint = selectedRoute.Points[currentPointIndex];
                endPoint = selectedRoute.Points[currentPointIndex + 1];
            }

            busPosition.X = startPoint.X + (endPoint.X - startPoint.X) * animationProgress;
            busPosition.Y = startPoint.Y + (endPoint.Y - startPoint.Y) * animationProgress;

            DrawRoute(selectedRoute);
        }

        private void DrawRoute(BusRoute route)
        {
            if (route == null || route.Points.Count == 0) return;

            Bitmap bmp = new Bitmap(picMap.Width, picMap.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (Pen routePen = new Pen(Color.Blue, 3))
                {
                    for (int i = 0; i < route.Points.Count - 1; i++)
                    {
                        g.DrawLine(routePen, route.Points[i].X, route.Points[i].Y,
                                   route.Points[i + 1].X, route.Points[i + 1].Y);
                    }
                }

                foreach (var point in route.Points)
                {
                    g.FillEllipse(Brushes.Red, point.X - 5, point.Y - 5, 10, 10);
                    g.DrawString(point.Name, this.Font, Brushes.Black, point.X + 10, point.Y - 10);
                }

                if (animationTimer.Enabled)
                {
                    g.FillEllipse(Brushes.Green, busPosition.X - 8, busPosition.Y - 8, 16, 16);
                }
            }

            picMap.Image = bmp;
            picMap.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            picMap.Image = new Bitmap(picMap.Width, picMap.Height);
            using (Graphics g = Graphics.FromImage(picMap.Image))
            {
                g.Clear(Color.White);
            }
        }

        private void txtRouteName_Enter(object sender, EventArgs e)
        {
            if (txtRouteName.Text == "Название маршрута")
            {
                txtRouteName.Text = "";
                txtRouteName.ForeColor = Color.Black;
            }
        }

        private void txtRouteName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRouteName.Text))
            {
                txtRouteName.Text = "Название маршрута";
                txtRouteName.ForeColor = Color.Gray;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveRoutesToFile(); // Сохраняем маршруты при закрытии формы
        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 240 };
            Button confirmation = new Button() { Text = "OK", Left = 160, Top = 80, Width = 100 };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.ShowDialog();
            return textBox.Text;
        }
    }

    public class BusRoute
    {
        public int Id { get; set; } // Уникальный идентификатор маршрута
        public string Name { get; set; } // Название маршрута
        public List<RoutePoint> Points { get; set; } = new List<RoutePoint>(); // Список точек маршрута
    }

    public class RoutePoint
    {
        public int Id { get; set; } // Уникальный идентификатор точки
        public string Name { get; set; } // Название точки
        public float X { get; set; } // Координата X на карте
        public float Y { get; set; } // Координата Y на карте
        public TimeSpan TimeFromStart { get; set; } // Время от начала маршрута до точки
    }
}

Изменения и улучшения

Красные строки:

Использован тег <span style="color: red"> для выделения ключевых слов и названий (например, "Newtonsoft.Json", названия кнопок). На GitHub это может не отображаться как цветной текст, но в некоторых рендерерах Markdown будет работать.
Замените на **жирный текст** или ~~зачёркнутый текст~~, если нужен альтернативный акцент (GitHub поддерживает это лучше).


Отступы и структура:

Добавлены заголовки разного уровня (#, ##, ###) для чёткой иерархии.
Использованы горизонтальные линии (---) для разделения секций.
Списки оформлены с отступами и нумерованы для пошаговых инструкций.


Читаемость:

Кодовый пример (DrawRoute, BusRoute, RoutePoint) выровнен с отступами для удобства чтения.
Комментарии в коде оставлены для пояснения назначения переменных и методов.


Совместимость:

Учтено, что GitHub рендерит Markdown с ограничениями. Если нужен более яркий дизайн, можно добавить эмодзи (например, ✅ для успехов) или таблицы.



Инструкции

Скопируйте содержимое тега <xaiArtifact/> в файл README.md в корневой папке вашего проекта.
Откройте файл в редакторе (например, Visual Studio Code) и проверьте рендеринг.
Если цвет не отображается на GitHub, замените <span style="color: red">текст</span> на **текст** или используйте другие Markdown-стили (например, > цитата).
Закоммитьте и отправьте изменения на GitHub:git add README.md
git commit -m "Update README.md with improved formatting - 07:41 AM PDT, 06/10/2025"
git push origin main



Если нужно добавить ещё элементы (например, скриншоты, таблицы или эмодзи), дайте знать, и я дополню!
BusRouteVisualization
Описание проекта
BusRouteVisualization — это приложение на C# с использованием Windows Forms, которое позволяет визуализировать маршруты общественного транспорта на карте. Приложение поддерживает создание новых маршрутов, добавление точек, отображение маршрутов на PictureBox и анимацию движения автобуса по маршруту. Данные о маршрутах сохраняются в файл routes.json для последующего использования.
Требования

Среда разработки: Visual Studio 2019 или новее.
Фреймворк: .NET Framework 4.7.2 или новее (рекомендуется .NET 4.8).
Зависимости: Пакет Newtonsoft.Json (добавляется через NuGet).

Установка

Клонирование репозитория:

Скачайте или склонируйте проект с помощью Git:git clone <URL_вашего_репозитория>


Или загрузите ZIP-архив и распакуйте его.


Установка зависимостей:

Откройте проект в Visual Studio.
В меню выберите Tools > NuGet Package Manager > Manage NuGet Packages for Solution.
Найдите и установите пакет Newtonsoft.Json.


Сборка проекта:

Нажмите Build > Build Solution или используйте сочетание клавиш Ctrl+Shift+B.
Убедитесь, что нет ошибок компиляции.


Запуск:

Нажмите F5 для запуска в режиме отладки или найдите сгенерированный .exe-файл в папке bin\Debug (или bin\Release).



Использование
Интерфейс

PictureBox (picMap): Отображает карту с маршрутами и точками.
ComboBox (cmbRoutes): Список доступных маршрутов.
TextBox (txtRouteName): Поле для ввода имени нового маршрута (с placeholder "Название маршрута").
Кнопки:
Добавить точку (btnAddPoint): Добавляет новую точку к текущему маршруту.
Создать маршрут (btnCreateRoute): Создаёт новый маршрут с указанным именем.
Загрузить маршрут (btnLoadRoute): Загружает выбранный маршрут для отображения.
Анимировать (btnAnimate): Запускает анимацию движения автобуса по маршруту.



Пошаговая инструкция

Создание маршрута:

Введите имя маршрута в txtRouteName.
Нажмите Создать маршрут.
Новый маршрут появится в списке cmbRoutes.


Добавление точек:

Нажмите Добавить точку, введите название точки в диалоговом окне.
Кликните на picMap, чтобы установить координаты точки.


Загрузка маршрута:

Выберите маршрут в cmbRoutes.
Нажмите Загрузить маршрут, чтобы отобразить его на карте.


Анимация:

Убедитесь, что маршрут содержит минимум 2 точки.
Нажмите Анимировать, чтобы увидеть движение автобуса (зелёная точка).


Сохранение:

Все изменения автоматически сохраняются в файл routes.json при закрытии формы или создании нового маршрута.



Сохранение данных

Файл: routes.json.
Местоположение:
Во время разработки: bin\Debug\routes.json или bin\Release\routes.json (в зависимости от конфигурации сборки).
После установки: в той же директории, где находится .exe-файл, если не изменён путь.


Динамический путь (опционально):
Можно настроить сохранение в папке пользователя (например, %AppData%\BusRouteVisualization\routes.json) путём изменения кода:private static readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BusRouteVisualization", "routes.json");


Это требует добавления using System.IO; и создания директории, если её нет.


Формат файла: Данные сериализуются с помощью Newtonsoft.Json в читаемом формате. Пример:[
  {
    "Id": 1,
    "Name": "Маршрут 1: Центр - Периферия",
    "Points": [
      { "Id": 1, "Name": "Центральная площадь", "X": 50.0, "Y": 50.0, "TimeFromStart": "00:00:00" },
      { "Id": 2, "Name": "Улица Ленина", "X": 150.0, "Y": 100.0, "TimeFromStart": "00:05:00" }
    ]
  }
]



Структура кода

Form1.cs: Основная логика приложения, включая визуализацию и анимацию.
Form1.Designer.cs: Генерируемый код для элементов формы.
Program.cs: Точка входа приложения.
Зависимости: Newtonsoft.Json для работы с JSON.

Возможные проблемы и решения

Файл не сохраняется:
Проверьте права доступа к папке bin\Debug. Используйте динамический путь в %AppData%.


Ошибки компиляции:
Убедитесь, что Newtonsoft.Json установлен.


Анимация не работает:
Убедитесь, что маршрут содержит минимум 2 точки.



Описание кода
Ниже приведён пример основного кода приложения BusRouteVisualization, включая структуру классов и их назначение. Этот код находится в файле Form1.cs.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace BusRouteVisualization
{
    public partial class Form1 : Form
    {
        private List<BusRoute> routes = new List<BusRoute>(); // Список всех маршрутов
        private BusRoute selectedRoute; // Выбранный маршрут для отображения
        private BusRoute newRoute; // Новый создаваемый маршрут
        private Timer animationTimer; // Таймер для анимации движения автобуса
        private int currentPointIndex = 0; // Текущий индекс точки в анимации
        private float animationProgress = 0f; // Прогресс анимации между точками
        private PointF busPosition; // Позиция автобуса на карте
        private const string FilePath = "routes.json"; // Путь к файлу для сохранения данных

        public Form1()
        {
            InitializeComponent();
            picMap.Width = 541;
            picMap.Height = 368;
            picMap.MouseClick += PicMap_MouseClick;
            LoadRoutesFromFile(); // Загружаем маршруты при старте
            InitializeAnimationTimer();
        }

        private void InitializeAnimationTimer()
        {
            animationTimer = new Timer
            {
                Interval = 50
            };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void LoadRoutesFromFile()
        {
            if (File.Exists(FilePath))
            {
                try
                {
                    string jsonString = File.ReadAllText(FilePath);
                    routes = JsonConvert.DeserializeObject<List<BusRoute>>(jsonString);
                    if (routes != null)
                    {
                        cmbRoutes.DataSource = null;
                        cmbRoutes.DataSource = routes;
                        cmbRoutes.DisplayMember = "Name";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки маршрутов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveRoutesToFile()
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(routes, Formatting.Indented);
                File.WriteAllText(FilePath, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения маршрутов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTestData()
        {
            if (routes.Count == 0) // Добавляем тестовые данные только если файл пуст
            {
                var route1 = new BusRoute { Id = 1, Name = "Маршрут 1: Центр - Периферия" };
                route1.Points.Add(new RoutePoint { Id = 1, Name = "Центральная площадь", X = 50, Y = 50, TimeFromStart = TimeSpan.FromMinutes(0) });
                route1.Points.Add(new RoutePoint { Id = 2, Name = "Улица Ленина", X = 150, Y = 100, TimeFromStart = TimeSpan.FromMinutes(5) });
                route1.Points.Add(new RoutePoint { Id = 3, Name = "Торговый центр", X = 300, Y = 150, TimeFromStart = TimeSpan.FromMinutes(10) });
                route1.Points.Add(new RoutePoint { Id = 4, Name = "Жилой район", X = 450, Y = 300, TimeFromStart = TimeSpan.FromMinutes(20) });

                var route2 = new BusRoute { Id = 2, Name = "Маршрут 2: Вокзал - Аэропорт" };
                route2.Points.Add(new RoutePoint { Id = 5, Name = "Вокзал", X = 30, Y = 30, TimeFromStart = TimeSpan.FromMinutes(0) });
                route2.Points.Add(new RoutePoint { Id = 6, Name = "Шоссе", X = 200, Y = 50, TimeFromStart = TimeSpan.FromMinutes(10) });
                route2.Points.Add(new RoutePoint { Id = 7, Name = "Аэропорт", X = 500, Y = 20, TimeFromStart = TimeSpan.FromMinutes(25) });

                routes.Add(route1);
                routes.Add(route2);

                cmbRoutes.DataSource = null;
                cmbRoutes.DataSource = routes;
                cmbRoutes.DisplayMember = "Name";
                SaveRoutesToFile(); // Сохраняем тестовые данные в файл
            }
        }

        private void BtnAddPoint_Click(object sender, EventArgs e)
        {
            if (newRoute == null)
            {
                MessageBox.Show("Сначала создайте маршрут.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var pointName = Prompt.ShowDialog("Введите название точки:", "Новая точка");
            if (!string.IsNullOrEmpty(pointName))
            {
                newRoute.Points.Add(new RoutePoint
                {
                    Id = newRoute.Points.Count + 1,
                    Name = pointName,
                    X = 0,
                    Y = 0,
                    TimeFromStart = TimeSpan.FromMinutes(newRoute.Points.Count * 5)
                });
                MessageBox.Show("Кликните на карте, чтобы установить точку.", "Указание координат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void PicMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (newRoute != null && newRoute.Points.Count > 0 && newRoute.Points[newRoute.Points.Count - 1].X == 0)
            {
                var lastPoint = newRoute.Points[newRoute.Points.Count - 1];
                lastPoint.X = e.X;
                lastPoint.Y = e.Y;
                DrawRoute(newRoute);
                MessageBox.Show($"Точка {lastPoint.Name} добавлена на ({e.X}, {e.Y}).", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCreateRoute_Click(object sender, EventArgs e)
        {
            var routeName = txtRouteName.Text;
            if (string.IsNullOrEmpty(routeName) || routeName == "Название маршрута")
            {
                MessageBox.Show("Введите название маршрута.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            newRoute = new BusRoute { Id = routes.Count + 1, Name = routeName };
            routes.Add(newRoute);
            cmbRoutes.DataSource = null;
            cmbRoutes.DataSource = routes;
            cmbRoutes.DisplayMember = "Name";
            cmbRoutes.SelectedItem = newRoute;
            selectedRoute = newRoute;
            SaveRoutesToFile(); // Сохраняем после создания нового маршрута
            MessageBox.Show($"Маршрут {routeName} создан. Добавляйте точки.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLoadRoute_Click(object sender, EventArgs e)
        {
            if (cmbRoutes.SelectedItem is BusRoute route)
            {
                selectedRoute = route;
                newRoute = null;
                DrawRoute(selectedRoute);
                MessageBox.Show($"Загружен маршрут: {selectedRoute.Name}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Выберите маршрут.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnimate_Click(object sender, EventArgs e)
        {
            if (selectedRoute == null || selectedRoute.Points.Count < 2)
            {
                MessageBox.Show("Выберите маршрут с минимум двумя точками.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            currentPointIndex = 0;
            animationProgress = 0f;
            busPosition = new PointF(selectedRoute.Points[0].X, selectedRoute.Points[0].Y);
            animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (currentPointIndex >= selectedRoute.Points.Count - 1)
            {
                animationTimer.Stop();
                return;
            }

            var startPoint = selectedRoute.Points[currentPointIndex];
            var endPoint = selectedRoute.Points[currentPointIndex + 1];
            animationProgress += 0.05f;

            if (animationProgress >= 1f)
            {
                animationProgress = 0f;
                currentPointIndex++;
                if (currentPointIndex >= selectedRoute.Points.Count - 1)
                {
                    animationTimer.Stop();
                    return;
                }
                startPoint = selectedRoute.Points[currentPointIndex];
                endPoint = selectedRoute.Points[currentPointIndex + 1];
            }

            busPosition.X = startPoint.X + (endPoint.X - startPoint.X) * animationProgress;
            busPosition.Y = startPoint.Y + (endPoint.Y - startPoint.Y) * animationProgress;

            DrawRoute(selectedRoute);
        }

        private void DrawRoute(BusRoute route)
        {
            if (route == null || route.Points.Count == 0) return;

            Bitmap bmp = new Bitmap(picMap.Width, picMap.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (Pen routePen = new Pen(Color.Blue, 3))
                {
                    for (int i = 0; i < route.Points.Count - 1; i++)
                    {
                        g.DrawLine(routePen, route.Points[i].X, route.Points[i].Y,
                                   route.Points[i + 1].X, route.Points[i + 1].Y);
                    }
                }

                foreach (var point in route.Points)
                {
                    g.FillEllipse(Brushes.Red, point.X - 5, point.Y - 5, 10, 10);
                    g.DrawString(point.Name, this.Font, Brushes.Black, point.X + 10, point.Y - 10);
                }

                if (animationTimer.Enabled)
                {
                    g.FillEllipse(Brushes.Green, busPosition.X - 8, busPosition.Y - 8, 16, 16);
                }
            }

            picMap.Image = bmp;
            picMap.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            picMap.Image = new Bitmap(picMap.Width, picMap.Height);
            using (Graphics g = Graphics.FromImage(picMap.Image))
            {
                g.Clear(Color.White);
            }
        }

        private void txtRouteName_Enter(object sender, EventArgs e)
        {
            if (txtRouteName.Text == "Название маршрута")
            {
                txtRouteName.Text = "";
                txtRouteName.ForeColor = Color.Black;
            }
        }

        private void txtRouteName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRouteName.Text))
            {
                txtRouteName.Text = "Название маршрута";
                txtRouteName.ForeColor = Color.Gray;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveRoutesToFile(); // Сохраняем маршруты при закрытии формы
        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 240 };
            Button confirmation = new Button() { Text = "OK", Left = 160, Top = 80, Width = 100 };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.ShowDialog();
            return textBox.Text;
        }
    }

    public class BusRoute
    {
        public int Id { get; set; } // Уникальный идентификатор маршрута
        public string Name { get; set; } // Название маршрута
        public List<RoutePoint> Points { get; set; } = new List<RoutePoint>(); // Список точек маршрута
    }

    public class RoutePoint
    {
        public int Id { get; set; } // Уникальный идентификатор точки
        public string Name { get; set; } // Название точки
        public float X { get; set; } // Координата X на карте
        public float Y { get; set; } // Координата Y на карте
        public TimeSpan TimeFromStart { get; set; } // Время от начала маршрута до точки
    }
}

Описание кода

Класс Form1:

Основной класс формы, содержащий логику работы приложения.
Переменные:
routes: Список всех маршрутов.
selectedRoute: Текущий выбранный маршрут для отображения.
newRoute: Новый маршрут, который создаётся пользователем.
animationTimer: Таймер для анимации движения автобуса.
currentPointIndex и animationProgress: Управляют анимацией между точками.
busPosition: Текущая позиция автобуса на карте.
FilePath: Путь к файлу routes.json.


Методы:
InitializeComponent(): Инициализация компонентов формы (генерируется дизайнером).
LoadRoutesFromFile(): Загружает маршруты из файла routes.json.
SaveRoutesToFile(): Сохраняет маршруты в файл routes.json.
LoadTestData(): Добавляет тестовые данные, если файл пуст.
BtnAddPoint_Click(): Обрабатывает добавление новой точки.
PicMap_MouseClick(): Устанавливает координаты точки на карте.
BtnCreateRoute_Click(): Создаёт новый маршрут.
btnLoadRoute_Click(): Загружает выбранный маршрут.
btnAnimate_Click(): Запускает анимацию.
AnimationTimer_Tick(): Обновляет позицию автобуса во время анимации.
DrawRoute(): Рисует маршрут и точки на PictureBox.
Form1_FormClosing(): Сохраняет данные при закрытии формы.




Класс Prompt:

Статический класс для отображения диалогового окна ввода текста (например, для названия точки).


Класс BusRoute:

Представляет маршрут общественного транспорта.
Свойства:
Id: Уникальный идентификатор маршрута.
Name: Название маршрута.
Points: Список объектов RoutePoint, представляющих точки маршрута.




Класс RoutePoint:

Представляет точку на маршруте.
Свойства:
Id: Уникальный идентификатор точки.
Name: Название точки.
X и Y: Координаты точки на карте.
TimeFromStart: Время, прошедшее от начала маршрута до этой точки.



