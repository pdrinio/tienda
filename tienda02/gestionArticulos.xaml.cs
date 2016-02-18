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


        public gestionArticulos()
        {
            InitializeComponent();

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
            
            //mostramos información del elemento seleccionado, cuando cambia la selección del grid
            elementos elementoAñadido = (elementos)gestionArticulos1.dataGridElementos.SelectedItem;
            gestionArticulos1.imageSeleccionado.Source = new BitmapImage(new Uri(elementoAñadido.imagen));
        }

        private void btnEliminar_Copy_Click(object sender, RoutedEventArgs e)
        {
            try //GUARDAR Y SALIR: escribe el json al fichero, y sale
            {
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
        {       //llamamos al formulario de captura de información del nuevo elemento
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
                MessageBox.Show("No hay elementos seleccionados");

            }
            else
            {
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
            this.Close();
        }
    }
}