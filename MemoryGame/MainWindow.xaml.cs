using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;

namespace MemoryGame
{
    public partial class MainWindow : Window
    {
        private List<Button> gombok;
        private List<int> kartyaSzamok;
        private Button elsoKattintott, masodikKattintott;
        private DispatcherTimer idozito;
        private int elteltIdo;
        private int megtalaltParok;
        private int oszlopok = 2;
        private int sorok = 2;

        public MainWindow()
        {
            InitializeComponent();
            UjJatek();
        }

        private void UjJatek()
        {
            megtalaltParok = 0;
            if (idozito != null)
            {
                idozito.Stop();
            }

            kartyaSzamok = new List<int>();
            for (int i = 1; i <= (oszlopok * sorok) / 2; i++)
            {
                kartyaSzamok.Add(i);
                kartyaSzamok.Add(i);
            }

            KartyakKeverese();
            KartyakLetrehozasa();
            IdozitoInditasa();
        }

        private void KartyakKeverese()
        {
            Random rand = new Random();
            kartyaSzamok = kartyaSzamok.OrderBy(x => rand.Next()).ToList();
        }

        private void KartyakLetrehozasa()
        {
            gombok = new List<Button>();
            CardGrid.Children.Clear();

            UniformGrid grid = CardGrid;
            grid.Rows = sorok;
            grid.Columns = oszlopok;

            for (int i = 0; i < kartyaSzamok.Count; i++)
            {
                Button button = new Button
                {
                    Content = "?",
                    Tag = kartyaSzamok[i],
                    FontSize = 24
                };
                button.Click += CardButton_Click;
                gombok.Add(button);
                CardGrid.Children.Add(button);
            }
        }

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (elsoKattintott == null)
            {
                elsoKattintott = clickedButton;
                elsoKattintott.Content = elsoKattintott.Tag;
            }
            else if (masodikKattintott == null && clickedButton != elsoKattintott)
            {
                masodikKattintott = clickedButton;
                masodikKattintott.Content = masodikKattintott.Tag;

                CheckForMatch();
            }
        }

        private async void CheckForMatch()
        {
            if (elsoKattintott.Tag.ToString() == masodikKattintott.Tag.ToString())
            {
                megtalaltParok++;
                elsoKattintott.IsEnabled = false;
                masodikKattintott.IsEnabled = false;

                if (megtalaltParok == kartyaSzamok.Count / 2)
                {
                    idozito.Stop();
                    MessageBox.Show($"Gratulálok! A játék véget ért. Idő: {elteltIdo} másodperc.");
                    UjJatek();
                }
            }
            else
            {
                await Task.Delay(1000);
                elsoKattintott.Content = "?";
                masodikKattintott.Content = "?";
            }

            elsoKattintott = null;
            masodikKattintott = null;
        }

        private void IdozitoInditasa()
        {
            elteltIdo = 0;
            TimerTextBlock.Text = "Idő: 0 másodperc";
            idozito = new DispatcherTimer();
            idozito.Interval = TimeSpan.FromSeconds(1);
            idozito.Tick += Timer_Tick;
            idozito.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            elteltIdo++;
            TimerTextBlock.Text = $"Idő: {elteltIdo} másodperc";
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            UjJatek();
        }

        private void TwoByTwoButton_Click(object sender, RoutedEventArgs e)
        {
            oszlopok = 2;
            sorok = 2;
            UjJatek();
        }

        private void FourByFourButton_Click(object sender, RoutedEventArgs e)
        {
            oszlopok = 4;
            sorok = 4;
            UjJatek();
        }
    }
}
