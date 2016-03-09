using System.Media;
using System.Speech.Synthesis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    /// Lógica de interacción para gestionArticulos.xaml
    /// </summary>
    public partial class gestionArticulos : Window
    {
        //miembro de la clase de la ventana, como colección observable de elementos: la lista de objetos del grid;
        public ObservableCollection<elementos> lstElementosSeleccionados = new ObservableCollection<elementos>();

        //id anterior, pero en este caso los objetos de "bbdd" (lo que traemos del json)
        public List<elementos> lstElementos = new List<elementos>();

        //objeto de síntesis para el habla
        SpeechSynthesizer sinte = new SpeechSynthesizer();

        //objeto para los beeps de cada vez que toca algro en el grid
        SoundPlayer player = new SoundPlayer(Properties.Settings.Default.MensajeBeep);
        

        public gestionArticulos()
        {
            InitializeComponent();

            //síntesis para el habla
            sinte.SelectVoice(Properties.Settings.Default.voz.ToString());

            //vinculamos el grid a la lista global de elementos
            dataGridElementos.ItemsSource = lstElementosSeleccionados;
            

            try //nos traemos del json todos los elementos, a ambas listas (la observable del grid, y la de "BBDD")
            {
                string json = File.ReadAllText(@"c:\tienda02\tienda02.json");
                List<elementos> lstElementosBBDD = JsonConvert.DeserializeObject<List<elementos>>(json); //leemos del json, y traemos al grid
                lstElementos = lstElementosBBDD; //hacemos que la lista de BBDD sea la del grid, inicialmente                
                foreach (elementos elementito in lstElementos)
                {
                    lstElementosSeleccionados.Add(elementito);
                }

            }

            catch (Exception e)
            {
                MessageBox.Show("Excepcion" + e.ToString());
                throw;
            }
        }


        private void dataGridElementos_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //un beep
            player.Play();

            //mostramos información del elemento seleccionado, cuando cambia la selección del grid
            elementos elementoAñadido = (elementos)gestionArticulos1.dataGridElementos.SelectedItem;
            gestionArticulos1.imageSeleccionado.Source = new BitmapImage(new Uri(elementoAñadido.imagen));
        }

        private void btnEliminar_Copy_Click(object sender, RoutedEventArgs e)
        {
            try //GUARDAR Y SALIR: escribe el json al fichero, y sale
            {
                //tts
                sinte.SpeakAsync("All information is saved");

                string json = JsonConvert.SerializeObject(lstElementos.ToArray());                
                System.IO.File.WriteAllText(@"c:\tienda02\tienda02.json", json);
            }
            catch (Exception)
            {
                MessageBox.Show("Imposible guardar");
                throw;
            }
            this.Close();
        }

        private void btnAñadir_Click(object sender, RoutedEventArgs e)
        {
            //TTS
            sinte.Speak("Let's add a new article"); //no lo hago Async para que no se mezcle con el siguiente

            //llamamos al formulario de captura de información del nuevo elemento
            añadirElemento wpfAñadir = new añadirElemento();

            if (wpfAñadir.ShowDialog() == true) //cuando pulsa aceptar devuelvo true
            {
                elementos elementoAñadido = wpfAñadir.elementoAñadido; //y me traigo una property pública del elemento
                lstElementos.Add(elementoAñadido);
                lstElementosSeleccionados.Add(elementoAñadido);
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gestionArticulos1.dataGridElementos.SelectedIndex == -1)
            {
                sinte.SpeakAsync("I need you to select an article to delete it, please");

            }
            else
            {
                //TTS, mensaje de borrado
                sinte.SpeakAsync("Article flaged to be disposed. Tap on OK to save changes");

                //des-suscribimos de los eventos 
                gestionArticulos1.dataGridElementos.SelectionChanged -= dataGridElementos_SelectionChanged_1;

                //eliminamos de las listas, mandamos el foco a cualquier otra cosa
                elementos elementoSeleccionadoGrid = (elementos)gestionArticulos1.dataGridElementos.SelectedItem;
                lstElementos.Remove(elementoSeleccionadoGrid);
                lstElementosSeleccionados.Remove(elementoSeleccionadoGrid);
                gestionArticulos1.imageSeleccionado.Focus();

                //y volvemos a suscribir a eventos
                gestionArticulos1.dataGridElementos.SelectionChanged += dataGridElementos_SelectionChanged_1;
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            sinte.SpeakAsync("Changes dismissed");
            this.Close();
        }
    }
}