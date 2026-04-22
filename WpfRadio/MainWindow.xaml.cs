using System;
using System.Windows;
using NAudio.Wave;

namespace WpfRadio
{
    public partial class MainWindow : Window
    {
        private MediaFoundationReader reader;
        private WaveOutEvent waveOut;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            string url = UrlBox.Text.Trim();
            if (string.IsNullOrEmpty(url)) return;

            // Останавливаем, если что-то играло
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }

            try
            {
                reader = new MediaFoundationReader(url);
                waveOut = new WaveOutEvent();
                waveOut.Init(reader);
                waveOut.Play();

                PlayBtn.IsEnabled = false;
                StopBtn.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (reader != null)
            {
                reader.Dispose();
                reader = null;
            }

            PlayBtn.IsEnabled = true;
            StopBtn.IsEnabled = false;
        }
    }
}