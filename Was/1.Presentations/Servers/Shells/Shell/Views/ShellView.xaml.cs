using System.ComponentModel;
using System.Windows;

namespace Kengic.Was.Presentation.Server.Shell.Views
{
    public partial class Shell
    {
        public Shell()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = MessageBox.Show("Are you sure close windows!?", "Don't make mistake", MessageBoxButton.YesNo) != MessageBoxResult.Yes;
            base.OnClosing(e);
        }
    }
}