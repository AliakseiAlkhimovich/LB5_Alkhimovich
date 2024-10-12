using HorseRaceControlLibrary;
using System.Windows;
namespace HorseRaceApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            horseRaceControl.StartRace();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            horseRaceControl.PauseRace();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            horseRaceControl.ResetRace();
        }
    }
}

