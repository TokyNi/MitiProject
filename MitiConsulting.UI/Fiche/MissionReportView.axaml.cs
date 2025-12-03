using Avalonia.Controls; 
using Avalonia.Markup.Xaml;

namespace MitiConsulting.UI.Views 
{
    public partial class MissionReportView : UserControl
    {
        public MissionReportView()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}