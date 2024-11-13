using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

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

        public MainWindow()
        {
            InitializeComponent();
            UjJatek();
        }

        private void UjJatek()
        {
            kartyaSzamok = new List<int>
            {
                1, 1,
                2, 2,
                3, 3,
                4, 4,
                5, 5,
                6, 6,
                7, 7,
                8, 8
            };

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
    }
}
