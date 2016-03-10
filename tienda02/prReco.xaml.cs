using System.Globalization;
using System.Speech.Recognition;
using System.Threading;
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
    /// Lógica de interacción para prReco.xaml
    /// </summary>
    public partial class prReco : Window
    {
        static ManualResetEvent _completed = null;
        public float precision = 0;

        public prReco()
        {
            InitializeComponent();

            CultureInfo[] culturas = (CultureInfo.GetCultures(CultureTypes.AllCultures));          

            SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("es-ES"));
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("SI")));
            _recognizer.LoadGrammar(new DictationGrammar());
            _recognizer.SpeechRecognized += _recognizer_SpeechRecognized;

            try
            {
                _recognizer.SetInputToDefaultAudioDevice(); // set input to default audio device
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception)
            {

                MessageBox.Show("No tengo micrófono activo");
            }
 
        }

         void _recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "SI")
            {
                _completed.Set();
                return;
            }
            precision = e.Result.Confidence;
            actualizaTexto(e.Result.Text.ToString(), precision);
            //Console.WriteLine("You said: " + e.Result.Text);
        }

        public void actualizaTexto(String szLeido, float precision)
        {          
           this.txbLeido.AppendText(szLeido);
            this.txbLeido.AppendText(" (" + precision.ToString() + ")");
           this.txbLeido.AppendText("\r\n");
        }
    }

}
