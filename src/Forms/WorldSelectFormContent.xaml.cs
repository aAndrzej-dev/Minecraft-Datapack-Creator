using System.Collections.ObjectModel;
using System.IO;

namespace MinecraftDatapackCreator.Forms;
/// <summary>
/// Interaction logic for WorldSelectFormContent.xaml
/// </summary>
internal partial class WorldSelectFormContent : System.Windows.Controls.UserControl
{
    public ObservableCollection<MinecraftWorld> Worlds { get; }
    public WorldSelectFormContent(Settings settings)
    {
        DataContext = this;
        Worlds = new ObservableCollection<MinecraftWorld>();
        InitializeComponent();
        DirectoryInfo di = new DirectoryInfo(Path.Combine(settings.MinecraftDir, "saves"));
        if (!di.Exists)
            return;

        foreach (DirectoryInfo item in di.GetDirectories())
        {
            Worlds.Add(new MinecraftWorld(item.FullName, item.Name));

        }
    }

    internal void Show()
    {
        lbWorlds.Focus();
    }

    internal readonly struct MinecraftWorld
    {
        public MinecraftWorld(string fullPath, string name)
        {
            FullPath = fullPath;
            Name = name;
            string img = Path.Combine(fullPath, "icon.png");
            Icon = img;
        }

        public string FullPath { get; }
        public string Name { get; }

        public string Icon { get; }
    }
}
