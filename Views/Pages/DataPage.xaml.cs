using Microsoft.Win32;
using Notes.ViewModels.Pages;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Wpf.Ui.Controls;
using Xceed.Wpf.Toolkit;


namespace Notes.Views.Pages
{
    public partial class DataPage : INavigableView<DataViewModel>
    {
        public DataViewModel ViewModel { get; }

        public DataPage(DataViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
            docBox.PreviewKeyDown += docBox_PreviewKeyDown;
            fontSizeSlider.ValueChanged += FontSizeSlider_ValueChanged;

            foreach (System.Windows.Media.FontFamily fontFamily in System.Windows.Media.Fonts.SystemFontFamilies)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = fontFamily.Source;
                fontComboBox.Items.Add(item);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)   // Save Notes 
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "RichText Files (*.rtf)|*.rtf";
            if (sfd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(docBox.Document.ContentStart, docBox.Document.ContentEnd);
                using (FileStream fs = File.Create(sfd.FileName))
                {
                    if (Path.GetExtension(sfd.FileName).ToLower() == ".rtf")
                    {
                        doc.Save(fs, DataFormats.Rtf);
                        System.Windows.MessageBoxButton button = System.Windows.MessageBoxButton.OK;
                        System.Windows.MessageBoxImage messageBoxImage = System.Windows.MessageBoxImage.Information;
                        System.Windows.MessageBox.Show("You note saved", "Message", button, messageBoxImage);
                    }
                    else
                    {
                        System.Windows.MessageBoxButton button = System.Windows.MessageBoxButton.OK;
                        System.Windows.MessageBoxImage messageBoxImage = System.Windows.MessageBoxImage.Warning;
                        System.Windows.MessageBox.Show("Sorry but you note don't saved", "Message", button, messageBoxImage);
                    }
                }
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)   // Load notes 
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RichText Files (*.rtf)|*.rtf|All files (*.*)|*.*";

            if (ofd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(docBox.Document.ContentStart, docBox.Document.ContentEnd);
                using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                {
                    if (Path.GetExtension(ofd.FileName).ToLower() == ".rtf")
                    {
                        doc.Load(fs, DataFormats.Rtf);
                        System.Windows.MessageBoxButton button = System.Windows.MessageBoxButton.OK;
                        System.Windows.MessageBoxImage messageBoxImage = System.Windows.MessageBoxImage.Information;
                        System.Windows.MessageBox.Show("You note load", "Message", button, messageBoxImage);
                    }
                    else
                    {
                        System.Windows.MessageBoxButton button = System.Windows.MessageBoxButton.OK;
                        System.Windows.MessageBoxImage messageBoxImage = System.Windows.MessageBoxImage.Warning;
                        System.Windows.MessageBox.Show("You note not load", "Message", button, messageBoxImage);
                    }
                }

            }
        }

        private void Clean_Click(object sender, RoutedEventArgs e) // Clean all NOTES 
        {
            docBox.Document.Blocks.Clear();
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image | *.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;

                if (!string.IsNullOrEmpty(imagePath))
                {
                    BitmapImage originalImage = new BitmapImage(new Uri(imagePath));

                    double maxWidth = 800;
                    double maxHeight = 600;

                    double widthRatio = maxWidth / originalImage.Width;
                    double heightRation = maxHeight / originalImage.Height;
                    double scale = Math.Min(widthRatio, heightRation);

                    TransformedBitmap resizedImage = new TransformedBitmap(originalImage, new ScaleTransform(scale, scale));

                    var imageInLine = new InlineUIContainer(new System.Windows.Controls.Image { Source = resizedImage }, docBox.CaretPosition);
                    docBox.CaretPosition = imageInLine.ElementEnd;
                }
            }
        }

        private void docBox_PreviewKeyDown(object sender, KeyEventArgs e) // Ctrl + C 
        {
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                string clipboardText = Clipboard.GetText();
                if (Uri.IsWellFormedUriString(clipboardText, UriKind.Absolute))
                {
                    System.Windows.Documents.Hyperlink hyperlink = new System.Windows.Documents.Hyperlink(new Run(clipboardText));
                    hyperlink.NavigateUri = new Uri(clipboardText);
                    hyperlink.RequestNavigate += Hyperlink_RequestNavigate;

                    TextPointer textPointer = docBox.CaretPosition.GetInsertionPosition(LogicalDirection.Forward);
                    if (textPointer != null)
                    {
                        Paragraph paragraph = new Paragraph(hyperlink);
                        var paragraphInlinesCopy = new List<Inline>(paragraph.Inlines);
                        foreach (var inline in paragraphInlinesCopy)
                        {
                            textPointer.Paragraph.Inlines.Add(inline);
                        }
                    }

                    e.Handled = true;
                }
            }
        }
        string browserPath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Windows.Documents.Hyperlink hyperlink = (System.Windows.Documents.Hyperlink)sender;
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = browserPath,
                    Arguments = e.Uri.AbsoluteUri,
                });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Not work URL" + ex.Message);
            }
        }
        private void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color? selectedColor = colorPicker.SelectedColor;
            if (selectedColor.HasValue)
            {
                SolidColorBrush brush = new SolidColorBrush(selectedColor.Value);
                TextSelection selectedText = docBox.Selection;
                if (!selectedText.IsEmpty)
                {
                    selectedText.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
                }
            }
        }

        private void FontSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (docBox != null)
            {
                docBox.FontSize = e.NewValue;
            }
            else
            {
                System.Windows.MessageBox.Show("Enter Text");
            }
        }

        private void fontComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)fontComboBox.SelectedItem;
            string selectedFontFamily = selectedItem.Content.ToString();
            System.Windows.Media.FontFamily font = new System.Windows.Media.FontFamily(selectedFontFamily);
            TextSelection selectedText = docBox.Selection;

            if (!selectedText.IsEmpty)
            {
                selectedText.ApplyPropertyValue(TextElement.FontFamilyProperty, font);
            }
        }

    }
}
