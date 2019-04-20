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
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadCorrections("corrections.txt");
        }

        private Dictionary<string, string> corrections = new Dictionary<string, string>();
        private void LoadCorrections(string correctionsFile)
        {
            string file = correctionsFile;
            using (StreamReader reader = new StreamReader(file, Encoding.ASCII))
            {
                int lineCount = File.ReadAllLines(file).Length;
                try
                {
                    for (int i = 0; i < lineCount; ++i)
                    {
                        string wholeLine = reader.ReadLine();
                        string[] lineInfo = wholeLine.Split('-');
                        string wrong = lineInfo[0].Trim();
                        string right = lineInfo[1].Trim();
                        corrections.Add(wrong, right);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("ERROR: While parsing translation corrections the following error occured: " + e.Message);
                }
            }
        }



        private void OCRButton_Click(object sender, RoutedEventArgs e)
        {
            //var Result = Ocr.Read(@".\Input\DOC01100.pdf");
            //string text = Result.Text;
            UpdatesListBox.Items.Clear();
            WriteText("File processing started");

            var pdfFiles = Directory.GetFiles("./Input", "*.pdf");
            foreach (var file in pdfFiles)
            {
                string filePath = file;
                string roughText = PerformOCR(filePath);
                string cleanText = PerformCorrections(roughText);

                WriteText(WriteOutput(filePath, cleanText));
            }
        }



        private void WriteText(string text)
        {
            UpdatesListBox.Items.Add(DateTime.Now.ToString("HH:mm:ss") + ": " + text);
            UpdatesListBox.Items.Refresh();
        }



       IronOcr.AdvancedOcr Ocr = new IronOcr.AdvancedOcr()
        {
            CleanBackgroundNoise = false,
            EnhanceContrast = false,
            EnhanceResolution = false,
            Language = IronOcr.Languages.English.OcrLanguagePack,
            Strategy = IronOcr.AdvancedOcr.OcrStrategy.Fast,
            ColorSpace = IronOcr.AdvancedOcr.OcrColorSpace.GrayScale,
            DetectWhiteTextOnDarkBackgrounds = true,
            InputImageType = IronOcr.AdvancedOcr.InputTypes.AutoDetect,
            RotateAndStraighten = true,
            ReadBarCodes = false,
            ColorDepth = 4
        };

        private string PerformOCR(string filePath)
        {
            WriteText("Processing file '" + System.IO.Path.GetFileNameWithoutExtension(filePath) + "'...");
            var result = Ocr.Read(filePath);
            WriteText("File '" + System.IO.Path.GetFileNameWithoutExtension(filePath) + "' converted to text");
            return result.Text;
        }



        private string PerformCorrections(string roughText)
        {
            string cleanedText = roughText;
            foreach (var item in corrections)
            {
                cleanedText = cleanedText.Replace(item.Key, item.Value);
            }
            return cleanedText;
        }



        private string WriteOutput(string filePath, string text)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

            using (StreamWriter writer = new StreamWriter(System.IO.Path.Combine("./Output", fileName) + ".txt"))
            {
                writer.Write(text);
                writer.Flush();
            }
            return ("File '" + fileName + "' successfully exported");
        }
    }
}
