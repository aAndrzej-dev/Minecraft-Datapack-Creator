namespace MinecraftDatapackCreator;

internal interface ITabPage
{
    public void Save();
    public string Filename { get; }
    public Color TabBackColor { get; }
    public Color TabForeColor { get; }
    public bool IsNotSaved { get; }
    public event EventHandler? SavedStateChanged;

}