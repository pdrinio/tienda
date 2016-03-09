using System.Speech.Synthesis;
using System.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace tienda02
{
    /// <summary>
    /// Lógica de interacción para tecladoNumerico.xaml
    /// </summary>
    public partial class tecladoNumerico : Window
    {
        private Decimal _decPrecio; //el precio teclado (variable global, y la property

        public Decimal decPrecio
        {
            get { return _decPrecio; }
        }

        //audio
        SoundPlayer player = new SoundPlayer(Properties.Settings.Default.MensajeBeep); //para los beep
        
        MediaPlayer mPlayer = new MediaPlayer(); //para los mp3 utilizo MediaPlayer                                               

        SpeechSynthesizer sinte = new SpeechSynthesizer(); //síntesis para el habla
        

        public tecladoNumerico()
        {
            sinte.SelectVoice(Properties.Settings.Default.voz.ToString());

            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (Decimal.TryParse(lblCantidad.Content.ToString(), NumberStyles.Any, new CultureInfo("en-US"), out _decPrecio))
            {
                sinte.SpeakAsync("Article is OK");
                this.DialogResult = true;
            }
            
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            sinte.SpeakAsync("Cancel");
            this.Close();
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            String szValor = this.lblCantidad.Content.ToString();            
            if (szValor.Length > 0)
            {
                szValor =  szValor.Remove(szValor.Length - 1);
                this.lblCantidad.Content = szValor;
            }
            
        }

        private void cmb7_Click(object sender, RoutedEventArgs e)
        {
      
            Button botonPulsado = (Button)sender;
            
            if (compruebaValor(botonPulsado.Content.ToString()))
            {
                this.lblCantidad.Content += "7";
                suenaTecla();
            }          
        }

        private void cmb8_Click(object sender, RoutedEventArgs e)
        {
            suenaTecla();

            Button botonPulsado = (Button)sender;

            if (compruebaValor(botonPulsado.Content.ToString()))
            {
                this.lblCantidad.Content += "8";
            }
        }

        private void cmb9_Click(object sender, RoutedEventArgs e)
        {
            suenaTecla();

            Button botonPulsado = (Button)sender;

            if (compruebaValor(botonPulsado.Content.ToString()))
            {
                this.lblCantidad.Content += "9";
            }
        }

        private void cmb4_Click(object sender, RoutedEventArgs e)
        {
            suenaTecla();

            Button botonPulsado = (Button)sender;

            if (compruebaValor(botonPulsado.Content.ToString()))
            {
                this.lblCantidad.Content += "4";
            }
        }

        private void cmb5_Click(object sender, RoutedEventArgs e)
        {
            suenaTecla();

            Button botonPulsado = (Button)sender;

            if (compruebaValor(botonPulsado.Content.ToString()))
            {
                this.lblCantidad.Content += "5";
            }
        }

        private void cmb6_Click(object sender, RoutedEventArgs e)
        {
            suenaTecla();

            Button botonPulsado = (Button)sender;

            if (compruebaValor(botonPulsado.Content.ToString()))
            {
                this.lblCantidad.Content += "6";
            }
        }

        private void cmb1_Click(object sender, RoutedEventArgs e)
        {
            suenaTecla();

            Button botonPulsado = (Button)sender;

            if (compruebaValor(botonPulsado.Content.ToString()))
            {
                this.lblCantidad.Content += "1";
            }
        }

        private void cmb2_Click(object sender, RoutedEventArgs e)
        {
            suenaTecla();

            Button botonPulsado = (Button)sender;

            if (compruebaValor(botonPulsado.Content.ToString()))
            {
                this.lblCantidad.Content += "2";
            }
        }

        private void cmb3_Click(object sender, RoutedEventArgs e)
        {
            suenaTecla();

            Button botonPulsado = (Button)sender;

            if (compruebaValor(botonPulsado.Content.ToString()))
            {
                this.lblCantidad.Content += "3";
            }
        }

        private void cmb0_Click(object sender, RoutedEventArgs e)
        {
            suenaTecla();

            Button botonPulsado = (Button)sender;

            if (compruebaValor(botonPulsado.Content.ToString()))
            {
                this.lblCantidad.Content += "0";
            }
        }

        private void cmbPto_Click(object sender, RoutedEventArgs e)
        {
            Button botonPulsado = (Button)sender;

            if (compruebaValor(botonPulsado.Content.ToString()))
            {
                suenaTecla();
                this.lblCantidad.Content += ".";                
            }

        }

        private Boolean compruebaValor(string caracter)      
        { // se invoca cada vez que se pulsa una tecla; si no castea a decimal, lanza un sonido y no hace nada; devuelve bool
            try
            {
                string szValor = this.lblCantidad.Content.ToString();                    
                szValor += caracter;

                decimal decValor;
                if (Decimal.TryParse(szValor, NumberStyles.Any, new CultureInfo("en-US"), out decValor))
                {
                    if(decValor < Properties.Settings.Default.maxCompra)
                    {
                        return true; //es decimal, y menor que el valor máximo de Compra
                    }
                    else
                    {
                        sinte.SpeakAsync("This is very expensive!!!"); //tas pasao
                        return false;
                    }
                }
                else
                {
                    //el valor no parsea a decimal                    
                    mPlayer.Open(new Uri(Properties.Settings.Default.MensajeNo.ToString()));
                    mPlayer.Play();

                    return false;
                }
            }
            catch (Exception)
            {
                return false;               
            }            
        }

        private void suenaTecla()
        {
            //beep!
            player.Play();

        }
    }
}
