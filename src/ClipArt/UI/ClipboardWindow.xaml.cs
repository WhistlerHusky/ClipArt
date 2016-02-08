using System.Windows;
using System.Windows.Controls;
using ClipboardHookinkg;
using Control = System.Windows.Forms.Control;

namespace ClipArt.UI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    public delegate void ClipboardChanged(object obj);
    
    /// <summary>
    /// Interaction logic for ClipboardWindow.xaml
    /// </summary>
    public partial class ClipboardWindow
    {
        public event ClipboardChanged ClipClipboardChanged;

        protected virtual void OnClipClipboardChanged(object obj)
        {
            var handler = this.ClipClipboardChanged;
            if (handler != null)
            {
                handler(obj);
            }
        }

        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ClipboardWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.Manual;

            Deactivated += this.ClipboardWindow_Deactivated;

            Loaded += this.ClipboardWindow_Loaded;
        }

        public void ClipboardWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = Control.MousePosition.X;
            this.Top = Control.MousePosition.Y; 
            this.Visibility = Visibility.Visible;
            this.Topmost = true;
            this.Focus();
            this.Activate();
            this.ShowActivated = true;
        }

        private void ClipboardWindow_Deactivated(object sender, System.EventArgs e)
        {
            Log.Debug("ClipboardWindow_Deactivated");
            Hide();
        }

        public void HookKeyboard_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Log.Debug("HookKeyboard_KeyDown");

            this.Visibility = Visibility.Visible;
        }

        private void DataRow_Selected(object sender, RoutedEventArgs e)
        {
            ClipboardMonitor.Stop();

            var test = (sender as DataGridRow).Item as ClipboardEntity;
            Clipboard.SetText(test.Data.ToString());

            ClipboardMonitor.Start();
            Close();
        }
    }
}
