using System.IO;

namespace MinecraftDatapackCreator.Forms;
internal partial class LogViewerForm : Form
{
    public LogViewerForm(ILogger logger)
    {
        InitializeComponent();


        try
        {
            foreach (string item in File.ReadAllLines(logger.Filename))
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                int lastIndexOf = 0;

                int a = item.IndexOf('[', lastIndexOf) + 1;
                int b = item.IndexOf(']', a);
                ReadOnlySpan<char> instanceId = item.AsSpan(a, b - a);
                lastIndexOf = b + 1;

                a = item.IndexOf('[', lastIndexOf) + 1;
                b = item.IndexOf(']', a);
                ReadOnlySpan<char> date = item.AsSpan(a, b - a);
                lastIndexOf = b + 1;

                a = item.IndexOf('[', lastIndexOf) + 1;
                b = item.IndexOf(']', a);
                ReadOnlySpan<char> type = item.AsSpan(a, b - a);
                lastIndexOf = b + 1;

                ReadOnlySpan<char> message = item.AsSpan(lastIndexOf).TrimStart().TrimEnd();
                dataGridView1.Rows.Add(instanceId.ToString(), date.ToString(), type.ToString(), message.ToString());
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, Program.ProductTitle);
            throw;
        }
    }
}
