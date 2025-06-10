using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BusRouteVisualization
{
    public partial class Маршрут : Form
    {
        private List<BusRoute> routes = new List<BusRoute>();
        private BusRoute selectedRoute;
        private BusRoute newRoute;
        private Timer animationTimer;
        private int currentPointIndex = 0;
        private float animationProgress = 0f;
        private PointF busPosition;

        public Маршрут()
        {
            InitializeComponent();
            picMap.Width = 541;
            picMap.Height = 368;
            picMap.MouseClick += PicMap_MouseClick;
            LoadTestData();
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

        private void LoadTestData()
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
}