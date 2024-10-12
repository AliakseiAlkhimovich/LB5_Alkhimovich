using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace HorseRaceControlLibrary
{
    public class HorseRaceControl : Control
    {
        private DispatcherTimer timer;
        private Random random;
        private double[] horsePositions;
        private double[] horseSpeeds;
        private const int HorseCount = 5;

        static HorseRaceControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorseRaceControl), new FrameworkPropertyMetadata(typeof(HorseRaceControl)));
        }

        public HorseRaceControl()
        {
            random = new Random();
            horsePositions = new double[HorseCount];
            horseSpeeds = new double[HorseCount];

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;

            this.MouseLeftButtonDown += HorseRaceControl_MouseLeftButtonDown;
            this.MouseRightButtonDown += HorseRaceControl_MouseRightButtonDown;

            ResetRace();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < HorseCount; i++)
            {
                horsePositions[i] += horseSpeeds[i];
                if (horsePositions[i] >= 1.0)
                {
                    horsePositions[i] = 0.0;
                }
            }
            InvalidateVisual();
        }

        private void HorseRaceControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int horseIndex = GetHorseIndexFromMousePosition(e.GetPosition(this));
            if (horseIndex != -1)
            {
                MessageBox.Show($"Лошадь {horseIndex + 1} Скорость: {horseSpeeds[horseIndex]:F2}");
            }
        }


        private void HorseRaceControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            // Создаем массив для хранения индексов лошадей и их позиций
            var horsePositionsWithIndex = horsePositions
                .Select((position, index) => new { Position = position, Index = index })
                .OrderByDescending(h => h.Position)
                .ToArray();

            // Формируем сообщение с результатами
            StringBuilder results = new StringBuilder("Результаты гонки:\n");
            for (int i = 0; i < horsePositionsWithIndex.Length; i++)
            {
                results.AppendLine($"Лошадь {horsePositionsWithIndex[i].Index + 1} занимает {i + 1}-е место с позицией: {horsePositionsWithIndex[i].Position:F2}");
            }

            // Выводим сообщение с результатами
            MessageBox.Show(results.ToString());
        }



        private int GetHorseIndexFromMousePosition(Point position)
        {
            double horseWidth = ActualWidth / HorseCount;
            int index = (int)(position.X / horseWidth);
            return index >= 0 && index < HorseCount ? index : -1;
        }

        public void StartRace()
        {
            timer.Start();
        }

        public void PauseRace()
        {
            timer.Stop();
        }

        public void ResetRace()
        {
            for (int i = 0; i < HorseCount; i++)
            {
                horsePositions[i] = 0.0;
                horseSpeeds[i] = random.NextDouble() * 0.1;
            }
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            double horseWidth = ActualWidth / HorseCount;
            for (int i = 0; i < HorseCount; i++)
            {
                Rect horseRect = new Rect(i * horseWidth, horsePositions[i] * ActualHeight, horseWidth, 20);
                drawingContext.DrawRectangle(Brushes.Brown, null, horseRect);
            }
        }
    }
}
