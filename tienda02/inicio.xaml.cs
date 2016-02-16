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
    /// Lógica de interacción para inicio.xaml
    /// </summary>
    public partial class inicio : Window
    {
        public inicio()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCompra_Click(object sender, RoutedEventArgs e)
        {
            MainWindow wpfCompra = new MainWindow();
            wpfCompra.Show();
        }

        private void btnArticulos_Click(object sender, RoutedEventArgs e)
        {
            gestionArticulos wpfGestionArticulos = new gestionArticulos();
            wpfGestionArticulos.Show();
        }
    }
}
