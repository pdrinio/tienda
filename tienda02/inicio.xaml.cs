using System.Speech.Synthesis;
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
        SpeechSynthesizer sinte = new SpeechSynthesizer(); //para el habla        
        
        public inicio()
        {
            InitializeComponent();
            
            //poner la voz, y dar la bienvenida
            sinte.SelectVoice(Properties.Settings.Default.voz.ToString());
            sinte.SpeakAsync("Welcome to my shop!");
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            sinte.Speak("See you later, alligator!");
            this.Close();
        }

        private void btnCompra_Click(object sender, RoutedEventArgs e)
        {
            sinte.SpeakAsync("We have a new customer!!");

            MainWindow wpfCompra = new MainWindow();
            wpfCompra.Show();
        }

        private void btnArticulos_Click(object sender, RoutedEventArgs e)
        {
          
            sinte.SpeakAsync("Let's manage articles!");

            gestionArticulos wpfGestionArticulos = new gestionArticulos();
            wpfGestionArticulos.Show();
        }
    }
}
