using System.Windows;
using ClipArt.Helper;
using HungryDeveloper.Wpf.TaskbarNotification;

namespace ClipArt
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// 
        /// </summary>
        private TaskbarIcon notifyIcon;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            CHookingHelper.InitHooking();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose();

            base.OnExit(e);
        }
    }
}
