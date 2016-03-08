using System.Speech.Synthesis;
using Microsoft.Win32;
using System;
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
using System.Windows.Shapes;

namespace tienda02
{
    /// <summary>
    /// Lógica de interacción para añadirElemento.xaml
    /// </summary>
    public partial class añadirElemento : Window
    {
        SpeechSynthesizer sinte = new SpeechSynthesizer(); //TTS

        private elementos _elementoAñadido = new elementos(); 
        private Decimal decPrecio;

        public elementos elementoAñadido
        {
          get { return _elementoAñadido; }
        }

        public añadirElemento()
        {
            //síntesis para el habla
            sinte.SelectVoice("Microsoft Zira Desktop");

            sinte.SpeakAsync("First scan the code");

            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            sinte.SpeakAsync("Changes dismissed");

            this.Close();
        }

        private void btnBuscarFoto_Click(object sender, RoutedEventArgs e)
        {
            sinte.SpeakAsync("Let's find a picture for this article");

            String szruta = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Selecciona la imagen";
            ofd.InitialDirectory = szruta;
            
            try
            {
                if (ofd.ShowDialog() == true)
                {
                    añadirElemento1.txbImagen.Text = ofd.FileName;
                    añadirElemento1.image.Source = new BitmapImage(new Uri(ofd.FileName));

                    this.txbNombre.Focus();
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if ((txbCodigo.Text != "") && (txbImagen.Text != "") && (txbNombre.Text != "") && (txbPrecio.Text != ""))
            {
                sinte.SpeakAsync("Well done; article added");

                _elementoAñadido.codigo = añadirElemento1.txbCodigo.Text;
                _elementoAñadido.nombre = añadirElemento1.txbNombre.Text;
                _elementoAñadido.imagen = añadirElemento1.txbImagen.Text;
                _elementoAñadido.precio = añadirElemento1.txbPrecio.Text;

                this.DialogResult = true;

            }
            else
            {
                sinte.SpeakAsync("Something is missing");
            }

        }

        private void txbPrecio_GotFocus(object sender, RoutedEventArgs e)
        {
            sinte.SpeakAsync("Give me a price for this article");

            tecladoNumerico miTeclado = new tecladoNumerico();
            if (miTeclado.ShowDialog() ==true)
            {
                this.txbPrecio.Text = miTeclado.decPrecio.ToString();
            }            
        }

        private void txbImagen_GotFocus(object sender, RoutedEventArgs e)
        {
            sinte.SpeakAsync("Give me an image");
        }

        private void txbNombre_GotFocus(object sender, RoutedEventArgs e)
        {
            sinte.SpeakAsync("Give me a name");
        }
        
    }
}
