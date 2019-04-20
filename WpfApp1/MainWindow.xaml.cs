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
        }

        private void OCRButton_Click(object sender, RoutedEventArgs e)
        {
            var Ocr = new IronOcr.AdvancedOcr()
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

            var Result = Ocr.Read(@".\Input\DOC01100.pdf");
            string text = Result.Text;
            UpdatesListBox.Items.Add(text);
        }

        //private string PerformOCR()
    }
}
