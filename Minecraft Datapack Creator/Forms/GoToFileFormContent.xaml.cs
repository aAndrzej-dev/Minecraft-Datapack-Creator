using MinecraftDatapackCreator.FileStructure;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace MinecraftDatapackCreator.Forms;

/// <summary>
/// Interaction logic for GoToFormContent.xaml
/// </summary>
internal sealed partial class GoToFileFormContent : System.Windows.Controls.UserControl
{
    private DatapackFileStructure Structure { get; }
    public DatapackFileInfo? SelectedFile { get; private set; }
    internal GoToFileFormContent(DatapackFileStructure structure)
    {
        InitializeComponent();
        Structure = structure;
        DataContext = this;
        txtGoTo.Focus();
    }

    public ObservableCollection<DatapackFileInfo> Files { get; } = new ObservableCollection<DatapackFileInfo>();

    private IEnumerable<DatapackFileInfo> FindFiles(string value)
    {
        return Structure.GetFiles().Where(x => x.FullName.Contains(value, StringComparison.OrdinalIgnoreCase) || x.NamespacedId?.Contains(value, StringComparison.OrdinalIgnoreCase) is true).OrderBy(x =>
        {
            if (string.Equals(value, x.NamespacedId, StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(value, x.NamespacedId, StringComparison.Ordinal))
                {
                    return 0;
                }
                return 1;
            }
            else if (string.Equals(value, x.Name, StringComparison.OrdinalIgnoreCase))
            {
                return 2;
            }
            else if (string.Equals(value, Path.GetFileNameWithoutExtension(x.Name), StringComparison.OrdinalIgnoreCase))
            {
                return 3;
            }
            if (x.Name.Contains(value, StringComparison.OrdinalIgnoreCase))
            {
                return 4;
            }
            if (x.NamespacedId?.Contains(value, StringComparison.OrdinalIgnoreCase) is true)
            {
                return 5;
            }
            return 6;



            //x.NamespacedId == value ? 0 : (x.Name == value ? 1 : 2)
        });
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {

        string text = txtGoTo.Text;

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
                SelectedFile = Files[lbResults.SelectedIndex];
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void LbResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (lbResults.Items.Count > 0 && lbResults.SelectedIndex >= 0)
        {
            SelectedFile = Files[lbResults.SelectedIndex];
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
