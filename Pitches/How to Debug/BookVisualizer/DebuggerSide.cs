using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(StringDialogVisualizer.DebuggerSide),
typeof(VisualizerObjectSource),
Target = typeof(System.String),
Description = "String Dialog Visualizer")]
namespace StringDialogVisualizer
{
    public class DebuggerSide : DialogDebuggerVisualizer
    {
        override protected void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            MessageBox.Show(objectProvider.GetObject().ToString());
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(DebuggerSide));
            visualizerHost.ShowVisualizer();
        }
    }
}
