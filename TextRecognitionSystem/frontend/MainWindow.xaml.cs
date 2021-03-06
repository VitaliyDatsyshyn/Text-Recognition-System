﻿using frontend.Enums;
using frontend.Helpers;
using frontend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace frontend
{
    public partial class MainWindow : Window
    {
        private TextRecognitionSettings _settings;
        private Modes _mode = Modes.Full;
        private List<string> _processingFiles;
        private List<OcrResults> _ocrResults;

        public MainWindow()
        {
            _settings = new TextRecognitionSettings();
            _processingFiles = new List<string>();
            _ocrResults = new List<OcrResults>();
            InitializeComponent();
        }

        private void CloseWindow(object sender, RoutedEventArgs e) => Close();

        #region Language page
        private void SelectEnglish(object sender, RoutedEventArgs e) => _settings.SetLanguage(Languages.English);

        private void SelectUkrainian(object sender, RoutedEventArgs e) => _settings.SetLanguage(Languages.Ukrainian);

        private void LanguageNext(object sender, RoutedEventArgs e)
        {
            LanguagePage.Visibility = Visibility.Hidden;
            SetupPage.Visibility = Visibility.Visible;
        }
        #endregion

        #region Setup page
        private void SelectFullMode(object sender, RoutedEventArgs e) => _mode = Modes.Full;

        private void SelectCustomMode(object sender, RoutedEventArgs e) => _mode = Modes.Custom;

        private void SetupBack(object sender, RoutedEventArgs e)
        {
            SetupPage.Visibility = Visibility.Hidden;
            LanguagePage.Visibility = Visibility.Visible;
        }

        private void SetupNext(object sender, RoutedEventArgs e)
        {
            SetupPage.Visibility = Visibility.Hidden;
            if (_mode == Modes.Custom)
                BinarizationPage.Visibility = Visibility.Visible;
            else
                DocumentsSelectionPage.Visibility = Visibility.Visible;
        }
        #endregion

        #region Binarizarion page
        private void TurnOnBinarization(object sender, RoutedEventArgs e) => _settings.IsBinarizationEnable = true;

        private void TurnOffBinarization(object sender, RoutedEventArgs e) => _settings.IsBinarizationEnable = false;

        private void BinarizationBack(object sender, RoutedEventArgs e)
        {
            BinarizationPage.Visibility = Visibility.Hidden;
            SetupPage.Visibility = Visibility.Visible;
        }

        private void BinarizationNext(object sender, RoutedEventArgs e)
        {
            BinarizationPage.Visibility = Visibility.Hidden;
            NoiseRemovalPage.Visibility = Visibility.Visible;
        }
        #endregion

        #region Noise Removal page
        private void TurnOnNoiseRemoval(object sender, RoutedEventArgs e) => _settings.IsNoiseRemovalEnable = true;

        private void TurnOffNoiseRemoval(object sender, RoutedEventArgs e) => _settings.IsNoiseRemovalEnable = false;

        private void NoiseRemovalBack(object sender, RoutedEventArgs e)
        {
            NoiseRemovalPage.Visibility = Visibility.Hidden;
            BinarizationPage.Visibility = Visibility.Visible;
        }

        private void NoiseRemovalNext(object sender, RoutedEventArgs e)
        {
            NoiseRemovalPage.Visibility = Visibility.Hidden;
            ContrastAdjustmentPage.Visibility = Visibility.Visible;
        }
        #endregion

        #region Contrast Adjustment page
        private void TurnOnContrastAdjustment(object sender, RoutedEventArgs e) => _settings.IsContrastAdjusmentEnable = true;

        private void TurnOffContrastAdjustment(object sender, RoutedEventArgs e) => _settings.IsContrastAdjusmentEnable = false;

        private void ContrastAdjustmentBack(object sender, RoutedEventArgs e)
        {
            ContrastAdjustmentPage.Visibility = Visibility.Hidden;
            NoiseRemovalPage.Visibility = Visibility.Visible;
        }

        private void ContrastAdjustmentNext(object sender, RoutedEventArgs e)
        {
            ContrastAdjustmentPage.Visibility = Visibility.Hidden;
            RotationPage.Visibility = Visibility.Visible;
        }
        #endregion

        #region Rotation page
        private void TurnOnRotation(object sender, RoutedEventArgs e) => _settings.IsRotationEnable = true;

        private void TurnOffRotation(object sender, RoutedEventArgs e) => _settings.IsRotationEnable = false;

        private void RotationBack(object sender, RoutedEventArgs e)
        {
            RotationPage.Visibility = Visibility.Hidden;
            ContrastAdjustmentPage.Visibility = Visibility.Visible;
        }

        private void RotationNext(object sender, RoutedEventArgs e)
        {
            RotationPage.Visibility = Visibility.Hidden;
            TypeRecognitionPage.Visibility = Visibility.Visible;
        }
        #endregion

        #region Type Recognition page
        private void TurnOnTypeRecognition(object sender, RoutedEventArgs e) => _settings.IsTypeRecognitionEnable = true;

        private void TurnOffTypeRecognition(object sender, RoutedEventArgs e) => _settings.IsTypeRecognitionEnable = false;

        private void TypeRecognitionBack(object sender, RoutedEventArgs e)
        {
            TypeRecognitionPage.Visibility = Visibility.Hidden;
            RotationPage.Visibility = Visibility.Visible;
        }

        private void TypeRecognitionNext(object sender, RoutedEventArgs e)
        {
            TypeRecognitionPage.Visibility = Visibility.Hidden;
            WordsCorrectionPage.Visibility = Visibility.Visible;
        }
        #endregion

        #region Words Correction page
        private void TurnOnWordsCorrection(object sender, RoutedEventArgs e) => _settings.IsWordsCorrectionEnable = true;

        private void TurnOffWordsCorrection(object sender, RoutedEventArgs e) => _settings.IsWordsCorrectionEnable = false;

        private void WordsCorrectionBack(object sender, RoutedEventArgs e)
        {
            WordsCorrectionPage.Visibility = Visibility.Hidden;
            TypeRecognitionPage.Visibility = Visibility.Visible;
        }

        private void WordsCorrectionNext(object sender, RoutedEventArgs e)
        {
            WordsCorrectionPage.Visibility = Visibility.Hidden;
            DocumentsSelectionPage.Visibility = Visibility.Visible;
        }
        #endregion

        #region Documents Selection page
        private void BrowseToDocuments(object sender, RoutedEventArgs e)
        {
            var filter = @"Documents | *.jpeg; *.jpg; *.png; *.tif; *.tiff; *.pdf";
            var files = FileHelper.PickFiles(filter);
            _processingFiles.AddRange(files);
            PathToDocuments.Text = String.Join("; ", files);
        }
        private void DocumentsSelectionBack(object sender, RoutedEventArgs e)
        {
            DocumentsSelectionPage.Visibility = Visibility.Hidden;
            if (_mode == Modes.Custom)
                WordsCorrectionPage.Visibility = Visibility.Visible;
            else
                SetupPage.Visibility = Visibility.Visible;
        }

        private void DocumentsSelectionNext(object sender, RoutedEventArgs e)
        {
            DocumentsSelectionPage.Visibility = Visibility.Hidden;
            KeyWordsPage.Visibility = Visibility.Visible;
        }
        #endregion

        #region Key Words page
        private void AddKeyWord(object sender, RoutedEventArgs e)
        {
            _settings.KeyWords.Add(NewKeyWord.Text);
            AddWordToListBox(NewKeyWord.Text);
            NewKeyWord.Text = String.Empty;
        }

        private void AddWordToListBox(string word)
        {
            var listBoxItem = new ListBoxItem
            {
                Content = word
            };
            KeyWords.Items.Add(listBoxItem);
        }

        private void LoadKeyWords(object sender, RoutedEventArgs e)
        {
            var file = FileHelper.PickFile("Txt files |*.txt");
            var keyWords = FileHelper.GetWordsFromFile(file);
            foreach (var keyWord in keyWords)
            {
                if (!String.IsNullOrWhiteSpace(keyWord) && !String.IsNullOrEmpty(keyWord))
                {
                    _settings.KeyWords.Add(keyWord);
                    AddWordToListBox(keyWord);
                }
            }
        }

        private void SaveKeyWords(object sender, RoutedEventArgs e)
        {
            var file = FileHelper.PickFile("Txt files |*.txt");
            if (File.Exists(file))
            {
                File.AppendAllLines(file, _settings.KeyWords);
                MessageBox.Show("Saved!");
            }
        }

        private void KeyWordsBack(object sender, RoutedEventArgs e)
        {
            KeyWordsPage.Visibility = Visibility.Hidden;
            DocumentsSelectionPage.Visibility = Visibility.Visible;
        }

        private void Run(object sender, RoutedEventArgs e)
        {
            KeyWordsPage.Visibility = Visibility.Hidden;
            ResultsPage.Visibility = Visibility.Visible;
            OcrResults.ItemsSource = _ocrResults;
            try
            {
                Task.Run(() =>
                {
                    foreach (var file in _processingFiles)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            _ocrResults.Add(new OcrResults() { FileName = new FileInfo(file).Name, OcredText = "Processing..."});
                            OcrResults.Items.Refresh();
                        });
                        _settings.FileName = file;
                        ServerHelper.UploadFileToServer(file);
                        var results = ServerHelper.GetOcrResults(_settings);
                        this.Dispatcher.Invoke(() =>
                        {
                            _ocrResults.RemoveAt(_ocrResults.Count - 1);
                            _ocrResults.AddRange(results);
                            OcrResults.Items.Refresh();
                        });
                    }
                });
            }
            catch(Exception ex)
            {
                MessageBox.Show("ERROR\n" + ex.Message);
            }
        }
        #endregion

        #region Results page
        private void SaveAsTxt(object sender, RoutedEventArgs e)
        {
            var resultedFile = FileHelper.SaveResultsAsTxt(_ocrResults);
            MessageBox.Show("Saved to " + resultedFile);
        }
        private void SaveAsCsv(object sender, RoutedEventArgs e)
        {
            var resultedFile = FileHelper.SaveResultsAsCsv(_ocrResults);
            MessageBox.Show("Saved to " + resultedFile);
        }

        private void ResultsBack(object sender, RoutedEventArgs e)
        {
            ResultsPage.Visibility = Visibility.Hidden;
            KeyWordsPage.Visibility = Visibility.Visible;
        }
        #endregion
    }
}
