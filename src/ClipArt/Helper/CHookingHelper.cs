using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Management;
using ClipArt.UI;
using ClipArt.ViewModel;
using ClipboardHookinkg;
using KeyboardHooking;
using WinApiUtil;

namespace ClipArt.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public static class CHookingHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        private static List<ClipboardEntity> _clipboardEntities;
        
        /// <summary>
        /// 
        /// </summary>
        public static void InitHooking()
        {
            InitializeClipboardHooking();

            InitializeKeyboardHooking();
        }
        
        /// <summary>
        /// 
        /// </summary>
        private static void InitializeClipboardHooking()
        {
            SetClipboardHooking();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetClipboardHooking()
        {
            Log.Debug("ClipboardMonitor.SetClipboardHooking()");

            ClipboardMonitor.OnClipboardChange += ClipboardMonitor_OnClipboardChange;

            ClipboardMonitor.Start();

            _clipboardEntities = new List<ClipboardEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="data"></param>
        private static void ClipboardMonitor_OnClipboardChange(ClipboardFormat format, object data)
        {
            Log.Debug("ClipboardMonitor_OnClipboardChange()");
            Icon icon = null;
            int processId;

            IntPtr clipboardHandle = ClipboardMonitor.GetClipboardOwner();

            if (0 < (int)clipboardHandle)
            {
                processId = CWinApi.GetProcessIdByHandle(clipboardHandle);

                if (0 < processId)
                {
                    try
                    {
                        // 32비트 프로그램에서 64비트 프로그램 프로세스 접근이 안되기 때문에
                        // 이런식으로 Process의 실행 위치를 가져온다. 
                        const string Query = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
                        var searcher = new ManagementObjectSearcher(Query);

                        foreach (var item in searcher.Get())
                        {
                            object id = item["ProcessID"];
                            object path = item["ExecutablePath"];

                            if (path != null && id.ToString() == processId.ToString())
                            {
                                icon = Icon.ExtractAssociatedIcon(path.ToString());
                                break;
                            }
                        }
                    }
                    catch
                    {
                        icon = null;
                    }
                }
            }

            var ms = new MemoryStream();

            if (icon != null)
            {
                icon.ToBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            }

            _clipboardEntities.Add(
                new ClipboardEntity(
                    ms.ToArray(),
                    data,
                    format,
                    DateTime.Now.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        private static void InitializeKeyboardHooking()
        {
            HookKeyboard.SetKeyboardHook();
            HookKeyboard.KeyUp += HookKeyboard_KeyUp;
            HookKeyboard.KeyDown += HookKeyboard_KeyDown;
            HookKeyboard.KeyPress += HookKeyboard_KeyPress;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void HookKeyboard_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            var clipboardWindow = new ClipboardWindow
           {
               DataContext = new ClipboardWindowViewModel(_clipboardEntities)
           };

            clipboardWindow.Show();
        }

        private static void HookKeyboard_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
        }

        private static void HookKeyboard_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }
    }
}
