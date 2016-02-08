using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinApiUtil
{
    /// <summary>
    /// 
    /// </summary>
    public static class CWinApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="threadid"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr handle, out int threadid);
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static Process GetProcessByHandle(IntPtr handle)
        {
            int processId;

            GetWindowThreadProcessId(handle, out processId);

            return Process.GetProcessById(processId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static int GetProcessIdByHandle(IntPtr hWnd)
        {
            int processId;

            GetWindowThreadProcessId(hWnd, out processId);

            return processId;
        }
    }
}
