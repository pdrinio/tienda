using Newtonsoft.Json;
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
using System.IO;

namespace tienda02
{
    /// <summary>
    /// Lógica de interacción para añadirElemento.xaml
    /// </summary>
    public partial class añadirElemento : Window
    {
        SpeechSynthesizer sinte = new SpeechSynthesizer(); //TTS

        private Decimal decPrecio;

        public List<elementos> lstElementos = new List<elementos>(); //relación de elementos ya dados de alta (para comprobar que no doy de alta un repe)

        private elementos _elementoAñadido = new elementos(); //objeto del elemento que voy a añadir cuando salga de aquí
        
        public elementos elementoAñadido //parte pública del elemento que doy de alta; es lo que devuelvo al salir
        {
          get { return _elementoAñadido; }
        }

        public añadirElemento()
        {
            //síntesis para el habla
            sinte.SelectVoice(Properties.Settings.Default.voz.ToString());            

            cargaBBDD();//cargar la bbdd

            sinte.SpeakAsync("First scan the code");//damos instrucciones de lo primero que tiene que hacer

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
                sinte.SpeakAsync("I have a problema with this picture");
               
            }


        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            //01. comprobamos que ha introducido toda la información 
            if ((txbCodigo.Text != "") && (txbImagen.Text != "") && (txbNombre.Text != "") && (txbPrecio.Text != ""))
            {
                //02. ahora comprobamos que el código no está repe, y si OK, cerramos el diálogo con TRUE
                var yaExiste = lstElementos.Any(x => x.codigo == txbCodigo.Text.ToString());

                if (yaExiste)
                {//el artículo ya existe
                    sinte.SpeakAsync("Article already exists; scan another");
                }
                else //el artículo no existía previamente
                {
                    sinte.SpeakAsync("Well done; article added");

                    _elementoAñadido.codigo = añadirElemento1.txbCodigo.Text;
                    _elementoAñadido.nombre = añadirElemento1.txbNombre.Text;
                    _elementoAñadido.imagen = añadirElemento1.txbImagen.Text;
                    _elementoAñadido.precio = añadirElemento1.txbPrecio.Text;

                    this.DialogResult = true;
                }

            }
            else sinte.SpeakAsync("Something is missing");
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
        
        private void cargaBBDD()
        {
            try //nos traemos del json todos los elementos, a la lista de los elementos
            {
                string json = File.ReadAllText(@"c:\tienda02\tienda02.json");
                List<elementos> _lstElementosBBDD = JsonConvert.DeserializeObject<List<elementos>>(json); //leemos del json, y traemos al grid        
                lstElementos = _lstElementosBBDD; //hacemos que la lista de BBDD sea la del grid, inicialmente                   
            }

            catch (Exception e)
            {
                sinte.SpeakAsync("Imposible to access the database");
                this.Close();
            }
        }
    }
}
