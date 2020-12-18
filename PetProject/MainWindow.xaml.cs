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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;

namespace PetProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SpeechRecognitionEngine _start = new SpeechRecognitionEngine();
        SpeechRecognitionEngine _recognition = new SpeechRecognitionEngine();
        SpeechSynthesizer _synthesizer = new SpeechSynthesizer();
        int timeOut = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _recognition.SetInputToDefaultAudioDevice();
            Grammar commands = new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"Commands.txt"))));
            _recognition.LoadGrammarAsync(commands);
            _recognition.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Command_SpeechRecignized);

            _start.SetInputToDefaultAudioDevice();
            Grammar startCommands = new Grammar(new GrammarBuilder(new Choices(new string[] {"start", "stop" })));
            _start.LoadGrammarAsync(startCommands);
            startCommands.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Start_SpeechRecignized);
            _start.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void Command_SpeechRecignized(object sender, SpeechRecognizedEventArgs e)
        {
            string speach = e.Result.Text;
            switch (speach)
            {
                case "hello":
                    _synthesizer.SpeakAsync("hi");
                    break;
                case "what date is it":
                    _synthesizer.SpeakAsync(DateTime.Now.Date.ToString());
                    break;
                case "what day is it":
                    _synthesizer.SpeakAsync(DateTime.Now.DayOfWeek.ToString());
                    break;
                case "what time is  it":
                    _synthesizer.SpeakAsync(DateTime.Now.ToString("h mm ss tt"));
                    break;
                default:
                    break;
            }
        }

        private void Start_SpeechRecignized(object sender, SpeechRecognizedEventArgs e)
        {
            string speach = e.Result.Text;

            switch (speach)
            {
                case "test":
                    _synthesizer.SpeakAsync("test");
                    break;
                case "start":
                    _recognition.RecognizeAsync(RecognizeMode.Multiple);
                    break;
                case "stop":
                    _recognition.RecognizeAsyncCancel();
                    break;
                default:
                    break;
            }
        }
    }
}
