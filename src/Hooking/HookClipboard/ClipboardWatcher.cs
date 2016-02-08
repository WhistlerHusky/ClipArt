using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using WinApiUtil;

namespace ClipboardHookinkg
{
    /// <summary>
    /// 
    /// </summary>
    public class ClipboardWatcher : Form
    {
        /// <summary>
        /// 
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private const int WM_DRAWCLIPBOARD = 0x308;

        /// <summary>
        /// 
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private const int WM_CHANGECBCHAIN = 0x030D;

        /// <summary>
        /// 
        /// </summary>
        private static readonly string[] Formats = Enum.GetNames(typeof(ClipboardFormat));

        /// <summary>
        /// 
        /// </summary>
        private static ClipboardWatcher m_Instance;

        /// <summary>
        /// 
        /// </summary>
        private static IntPtr nextClipboardViewer;

        /// <summary>
        /// 
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private int WM_GETICON = 0x7F;

        /// <summary>
        /// 
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private int GCL_HICONSM = -34;

        /// <summary>
        /// 
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private int GCL_HICON = -14;

        /// <summary>
        /// 
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private int ICON_SMALL = 0;

        /// <summary>
        /// 
        /// </summary
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private int ICON_BIG = 1;

        /// <summary>
        /// 
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed. Suppression is OK here.")]
        private int ICON_SMALL2 = 2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="data"></param>
        public delegate void OnClipboardChangeEventHandler(ClipboardFormat format, object data);

        /// <summary>
        /// 
        /// </summary>
        public static event OnClipboardChangeEventHandler OnClipboardChange;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWndNewViewer"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWndRemove"></param>
        /// <param name="hWndNewNext"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        /// <summary>
        /// 
        /// </summary>
        public static void Start()
        {
            // we can only have one instance if this class
            if (m_Instance != null)
            {
                return;
            }

            Thread t = new Thread(new ParameterizedThreadStart(x =>
            {
                Application.Run(new ClipboardWatcher());
            }));

            t.SetApartmentState(ApartmentState.STA); // give the [STAThread] attribute

            t.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Stop()
        {
            m_Instance.Invoke(new MethodInvoker(() =>
            {
                ChangeClipboardChain(m_Instance.Handle, nextClipboardViewer);
            }));

            m_Instance.Invoke(new MethodInvoker(m_Instance.Close));

            m_Instance.Dispose();

            m_Instance = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected override void SetVisibleCore(bool value)
        {
            CreateHandle();

            m_Instance = this;

            nextClipboardViewer = SetClipboardViewer(m_Instance.Handle);

            base.SetVisibleCore(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    ClipChanged();
                    CWinApi.SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    Trace.WriteLine(m.ToString() + nextClipboardViewer.ToString());
                    break;

                case WM_CHANGECBCHAIN:
                    if (m.WParam == nextClipboardViewer)
                    {
                        nextClipboardViewer = m.LParam;
                    }
                    else
                    {
                        CWinApi.SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    }
                    
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void ClipChanged()
        {
            IDataObject iData = Clipboard.GetDataObject();

            ClipboardFormat? format = null;

            foreach (var f in Formats)
            {
                if (iData.GetDataPresent(f))
                {
                    format = (ClipboardFormat)Enum.Parse(typeof(ClipboardFormat), f);
                    break;
                }
            }

            object data = iData.GetData(format.ToString());

            if (data == null || format == null)
            {
                return;
            }

            if (OnClipboardChange != null)
            {
                OnClipboardChange((ClipboardFormat)format, data);
            }
        }
    }
}
