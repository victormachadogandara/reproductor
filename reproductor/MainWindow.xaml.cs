﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace reproductor
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Lector de archivos
        AudioFileReader reader;
        //Comunicacion con la tarjeta de audio
        //Exclusivo para salida
        WaveOut output;

        public MainWindow()
        {
            InitializeComponent();
            ListarDisposSalida();
            btnRerpoducir.IsEnabled = false;
            btnDetener.IsEnabled = false;
            btnPausa.IsEnabled = false;
        }

        void ListarDisposSalida()
        {
            cbDispositivoSalida.Items.Clear();
            for(int i = 0; i < WaveOut.DeviceCount; i++)
            {
                WaveOutCapabilities capacidades = WaveOut.GetCapabilities(i);
                cbDispositivoSalida.Items.Add(capacidades.ProductName);
            }
            cbDispositivoSalida.SelectedIndex = 0;
        }

        private void BtnExaminar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                txtRutaArchivo.Text = openFileDialog.FileName;
                btnRerpoducir.IsEnabled = true;
            }
        }

        private void CbDispositivoSalida_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnRerpoducir_Click(object sender, RoutedEventArgs e)
        {

            if (output != null && output.PlaybackState == PlaybackState.Paused)
            {
                //retomo reproduccion
                output.Play();
                btnRerpoducir.IsEnabled = false;
                btnPausa.IsEnabled = true;
                btnDetener.IsEnabled = true;
            }
            else
            {
                if (txtRutaArchivo.Text != null && txtRutaArchivo.Text != string.Empty)
                {
                    reader = new AudioFileReader(txtRutaArchivo.Text);

                    output = new WaveOut();
                    output.DeviceNumber = cbDispositivoSalida.SelectedIndex;

                    output.PlaybackStopped += Output_PlaybackStopped;

                    output.Init(reader);
                    output.Play();

                    btnRerpoducir.IsEnabled = false;
                    btnPausa.IsEnabled = true;
                    btnDetener.IsEnabled = true;

                    lblTiempoTotal.Text = reader.TotalTime.ToString().Substring(0, 8);
                }
            }
        }

        private void Output_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            reader.Dispose();
            output.Dispose();
        }

        private void O(object sender, StoppedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnDetener_Click(object sender, RoutedEventArgs e)
        {
            if (output != null)
            {
                output.Stop();
                btnRerpoducir.IsEnabled = true;
                btnPausa.IsEnabled = false;
                btnDetener.IsEnabled = false;
            }
        }

        private void BtnPausa_Click(object sender, RoutedEventArgs e)
        {
            if(output != null)
            {
                output.Pause();
                btnRerpoducir.IsEnabled = true;
                btnPausa.IsEnabled = false;
                btnDetener.IsEnabled = true;
            }
        }
    }
        
}
