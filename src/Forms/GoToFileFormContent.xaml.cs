using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace MinecraftDatapackCreator.Forms
{
    /// <summary>
    /// Interaction logic for GoToFormContent.xaml
    /// </summary>
    internal partial class GoToFileFormContent : System.Windows.Controls.UserControl
    {
        private DatapackFileStructure Structure { get; }
        public string? SelectedFile { get; private set; }
        internal GoToFileFormContent(DatapackFileStructure structure)
        {
            InitializeComponent();
            Structure = structure;
            DataContext = this;
            txtGoTo.Focus();
        }

        public ObservableCollection<DatapackFileInfo> Files { get; } = new();

        private IEnumerable<DatapackFileInfo> FindFiles(string value)
        {
            return Structure.GetFiles().Where(x => x.FullName.Contains(value, StringComparison.OrdinalIgnoreCase) || x.NamespacedId?.Contains(value, StringComparison.OrdinalIgnoreCase) is true).OrderBy(x => x.NamespacedId == value ? 0 : (x.Name == value ? 1 : 2));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            string text = txtGoTo.Text.ToLowerInvariant();

                if (string.IsNullOrEmpty(text))
                {
                    
                        Files.Clear();
                    
                    return;
                }
                IEnumerable<DatapackFileInfo> files = FindFiles(text);

                
                    Files.Clear();
                    foreach (DatapackFileInfo item in files)
                    {
                        Files.Add(item);
                    }
                    if (Files.Count > 0)
                    {
                        lbResults.SelectedIndex = 0;
                    }
               
           

        }
        public event EventHandler? RequestClose;
        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            if (e.Key == Key.Enter)
            {
                if (lbResults.Items.Count > 0 && lbResults.SelectedIndex >= 0)
                {
                    SelectedFile = Files[lbResults.SelectedIndex].FullName;
                    RequestClose?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void LbResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lbResults.Items.Count > 0 && lbResults.SelectedIndex >= 0)
            {
                SelectedFile = Files[lbResults.SelectedIndex].FullName;
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
        }
        internal void Show()
        {
            txtGoTo.Text = string.Empty;
            SelectedFile = null;
            txtGoTo.Focus();
        }

    }
}
