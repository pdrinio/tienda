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
        private elementos _elementoAñadido = new elementos();

        public elementos elementoAñadido
        {
          get { return _elementoAñadido; }
        }

        public añadirElemento()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnBuscarFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Selecciona la imagen";
            ofd.InitialDirectory = "c:\\tienda02";
            try
            {
                if (ofd.ShowDialog() == true)
                {
                    añadirElemento1.txbImagen.Text = ofd.FileName;
                    añadirElemento1.image.Source = new BitmapImage(new Uri(ofd.FileName));
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            
            _elementoAñadido.codigo = añadirElemento1.txbCodigo.Text;
            _elementoAñadido.nombre = añadirElemento1.txbNombre.Text;
            _elementoAñadido.imagen = añadirElemento1.txbImagen.Text;
            _elementoAñadido.precio = añadirElemento1.txbPrecio.Text;

            this.DialogResult = true;
        }
    }
}
