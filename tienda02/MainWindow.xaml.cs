using System.Speech.Synthesis;
using System.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Cache;
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


namespace tienda02
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        //para los audio en wav
        SoundPlayer playerBeep = new SoundPlayer(@"c:\tienda02\sonidos\wav\beep.wav");
        
        //para los audios en MP3
        MediaPlayer playerCash = new MediaPlayer();

        //para el habla
        SpeechSynthesizer sinte = new SpeechSynthesizer(); 

        //miembro de la clase de la ventana, como colección observable de elementos: la lista de objetos del grid;
        public ObservableCollection<elementos> lstElementosSeleccionados = new ObservableCollection<elementos>();

        //id anterior, pero en este caso los objetos de "bbdd" (lo que traemos del json)
        public List<elementos> lstElementos = new List<elementos>();

        //el precio del último artículo añadido a la cesta, y la cuenta a pagar
        public decimal decUltimo, decTotal;

        public MainWindow()
        {
            sinte.SelectVoice("Microsoft Zira Desktop"); //elegimos inglés

            InitializeComponent();

            //vinculamos el grid a la lista global de elementos
            dataGridElementos.ItemsSource = lstElementosSeleccionados;

            try
            {
                string json = File.ReadAllText(@"c:\tienda02\tienda02.json");
                //var lstElementosBBDD = JsonConvert.DeserializeObject<List<elementos>>(json); ANTES
                List<elementos> lstElementosBBDD = JsonConvert.DeserializeObject<List<elementos>>(json); //leemos del json, y traemos al grid
                lstElementos = lstElementosBBDD; //hacemos que la lista de BBDD sea la del grid, inicialmente                
            }

            catch (Exception e)
            {
                MessageBox.Show("Excepcion" + e.ToString());
                throw;
            }

        }

        private void dataGridElementos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //un beep            
            playerBeep.Play();

            //limpiamos los controles del último seleccionado
            principal.labelElementoSeleccionadoCodigo.Content = "";
            principal.labelElementoSeleccionadoNombre.Content = "";
            principal.imageElementoSeleccionado.Source = null;

            //traemos la imagen del objeto seleccionado
            elementos elementoAñadido = (elementos)principal.dataGridElementos.SelectedItem;            
            principal.imageElementoAñadido.Source = new BitmapImage(new Uri(elementoAñadido.imagen));

            //y mandamos el foco al campo de entrada, por si quieren pistolear uno nuevo
            principal.textBoxNuevoElemento.Focus();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            //decimos adios
            sinte.SpeakAsync("Come back soon!");

            //el usuario cancela
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            //clin-clin
            playerCash.Open(new Uri(@"c:\tienda02\sonidos\mp3\cash.mp3"));
            playerCash.Play();

            //el usuario acepta la compra
            //TODO: implementar factura/ticket
            this.Close(); 
        }

        private void textBoxNuevoElemento_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                try
                { //limpiamos el último añadido
                    principal.imageElementoAñadido.Source = null;

                    //recuperamos el código del elemento que ha leído la pistola
                    String szLeido = principal.textBoxNuevoElemento.Text.ToString();

                    //encontramos el elemento
                    var elementoSeleccionado = (from a in lstElementos
                                                    //  where a.codigo == principal.textBoxNuevoElemento.Text.ToString()
                                                where a.codigo == szLeido
                                                select a).First();

                    //lo traemos a la interfaz                
                    principal.labelElementoSeleccionadoCodigo.Content = elementoSeleccionado.codigo;
                    principal.labelElementoSeleccionadoNombre.Content = elementoSeleccionado.nombre;
                    principal.labelElementoSeleccionadoPrecio.Content = elementoSeleccionado.precio + " €";

                    principal.imageElementoSeleccionado.Source = null;
                    principal.imageElementoSeleccionado.Source = new BitmapImage(new Uri(elementoSeleccionado.imagen.ToString()));

                    // lo añadimos a la lista
                    lstElementosSeleccionados.Add(elementoSeleccionado);

                    //actualizamos el total de la compra, y lo presentamos
                    if (Decimal.TryParse(elementoSeleccionado.precio, NumberStyles.Any, new CultureInfo("en-US"), out decUltimo))
                    {
                        decTotal += decUltimo;
                        principal.labelTotalImporte.Content = decTotal.ToString() + " €";
                    }
                   

                    //y limpiamos el campo de entrada del código, para la siguiente
                    principal.textBoxNuevoElemento.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Imposible añadir el elemento seleccionado. " + ex.ToString());
                    throw;
                }
            }
        }

       
    }

}
