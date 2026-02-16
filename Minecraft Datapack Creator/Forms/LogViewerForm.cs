using System.IO;

namespace MinecraftDatapackCreator.Forms;
internal sealed partial class LogViewerForm : Form
{
    public LogViewerForm(ILogger logger)
    {
        InitializeComponent();


        try
        {
            using StreamReader sr = new StreamReader(File.Open(logger.Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            string? item = sr.ReadLine();
            while (item is not null)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                int lastIndexOf = 0;
                int a, b;

                a = item.IndexOf('[', lastIndexOf) + 1;
                b = item.IndexOf(']', a);
                ReadOnlySpan<char> date = item.AsSpan(a, b - a);
                lastIndexOf = b + 1;

                a = item.IndexOf('[', lastIndexOf) + 1;
                b = item.IndexOf(']', a);
                ReadOnlySpan<char> type = item.AsSpan(a, b - a);
                lastIndexOf = b + 1;

                ReadOnlySpan<char> message = item.AsSpan(lastIndexOf).Trim();
                dataGridView1.Rows.Add(date.ToString(), type.ToString(), message.ToString());
                item = sr.ReadLine();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, Program.ProductTitle);
            throw;
        }
    }
}
