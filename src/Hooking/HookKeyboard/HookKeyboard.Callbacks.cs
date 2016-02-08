using System;

namespace KeyboardHooking
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class HookKeyboard
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);
    }
}
