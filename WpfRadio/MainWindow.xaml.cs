using System;
using System.Windows;
using NAudio.Wave;

namespace WpfRadio
{
    public partial class MainWindow : Window
    {
        private MediaFoundationReader reader;
        private WaveOutEvent waveOut;
        //reader читает поток из интернета, waveOut выводит звук на колонки
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Play_Click(object sender, RoutedEventArgs e)   //прописываем логику для кнопки плей
        {
            string url = UrlBox.Text.Trim();
            if (string.IsNullOrEmpty(url)) return; //если ничего не ввели — выходим из метода

            if (waveOut != null)  //если радио уже играло
            {
                waveOut.Stop(); //прекращаем вывод звука
                waveOut.Dispose();  //освобождаем аудиоустройство
                waveOut = null; //забываем о нём
            }
            if (reader != null)
            {
                reader.Dispose(); //закрываем сетевой поток    
                reader = null;
            }

            try //используем метод try-catch для запуска радио
            {
                reader = new MediaFoundationReader(url);    //создаём читатель аудиопотока по URL
                waveOut = new WaveOutEvent();   //создаём выходное аудиоустройство
                waveOut.Init(reader);   //говорим waveOut брать звук из нашего reader
                waveOut.Play(); //запускаем воспроизведение

                PlayBtn.IsEnabled = false;  //отключаем возможность повторного нажатия кнопки play, включаем кнопку stop
                StopBtn.IsEnabled = true;
            }
            catch (Exception ex)    //выводим ошибку, если токовая есть
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e) //прописываем логику для кнопки стоп
        {
            if (waveOut != null) //также останавливаем и чистим поток
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

            PlayBtn.IsEnabled = true;   //возвращаем кнопки в исходное положение 
            StopBtn.IsEnabled = false;
        }
    }
}