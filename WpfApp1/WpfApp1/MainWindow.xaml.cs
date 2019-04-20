﻿using System;
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
            //var Result = Ocr.Read(@".\Input\DOC01100.pdf");
            //string text = Result.Text;
            var pdfFiles = Directory.GetFiles("./Input", "*.pdf");
            string filePath = pdfFiles[0];
            string text = PerformOCR(filePath);
            WriteText(WriteOutput(filePath, text));
        }

        private void WriteText(string text)
        {
            UpdatesListBox.Items.Add(DateTime.Now.ToString("HH:mm:ss") + ": " + text);
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
            var result = Ocr.Read(filePath);
            return result.Text;
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
