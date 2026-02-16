using System.Collections.ObjectModel;
using System.IO;

namespace MinecraftDatapackCreator.Forms;
/// <summary>
/// Interaction logic for WorldSelectFormContent.xaml
/// </summary>
internal sealed partial class WorldSelectFormContent : System.Windows.Controls.UserControl
{
    public ObservableCollection<MinecraftWorld> Worlds { get; }
    public string? SelectedWorld { get; private set; }
    public event EventHandler? RequestClose;

    public WorldSelectFormContent(Controller controller)
    {
        DataContext = this;
        Worlds = new ObservableCollection<MinecraftWorld>();
        InitializeComponent();
        DirectoryInfo di = new DirectoryInfo(Path.Join(controller.Settings.MinecraftDir, "saves"));
        if (!di.Exists)
            return;

        foreach (DirectoryInfo item in di.GetDirectories())
        {
            Worlds.Add(new MinecraftWorld(item.FullName, item.Name));

        }
    }

    internal void Show() => lbWorlds.Focus();

    internal readonly struct MinecraftWorld
    {
        public MinecraftWorld(string fullPath, string name)
        {
            FullPath = fullPath;
            Name = name;
            Icon = Path.Join(fullPath, "icon.png");
        }

        public string FullPath { get; }
        public string Name { get; }
        public string Icon { get; }
    }

    private void lbWorlds_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (lbWorlds.Items.Count > 0 && lbWorlds.SelectedIndex >= 0)
        {
            SelectedWorld = Worlds[lbWorlds.SelectedIndex].FullPath;
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
