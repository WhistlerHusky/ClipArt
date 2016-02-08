using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyboardHooking
{
    /// <summary>
    /// </summary>
    public static partial class HookKeyboard
    {
        /// <summary>
        /// </summary>
        private static int s_KeyboardHookHandle;

        /// <summary>
        /// </summary>
        private static bool s_isDownBothShiftAndCtrlKeys;

        /// <summary>
        /// </summary>
        private static HookProc s_KeyboardDelegate;

        /// <summary>
        /// </summary>
        public static event KeyPressEventHandler KeyPress;

        /// <summary>
        /// </summary>
        public static event KeyEventHandler KeyUp;

        /// <summary>
        /// </summary>
        public static event KeyEventHandler KeyDown;

        /// <summary>
        /// </summary>
        public static void SetKeyboardHook()
        {
            EnsureSubscribedToGlobalKeyboardEvents();
        }

        /// <summary>
        /// </summary>
        private static void EnsureSubscribedToGlobalKeyboardEvents()
        {
            if (0 == s_KeyboardHookHandle)
            {
                s_KeyboardDelegate = KeyboardHookProc;

                s_KeyboardHookHandle = SetWindowsHookEx(WhKeyboardLl, s_KeyboardDelegate, IntPtr.Zero, 0);

                if (s_KeyboardHookHandle == 0)
                {
                    int errorCode = Marshal.GetLastWin32Error();

                    throw new Win32Exception(errorCode);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private static int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            var handled = false;

            s_isDownBothShiftAndCtrlKeys = false;

            if (0 <= nCode)
            {
                // read structure KeyboardHookStruct at lParam
                var myKeyboardHookStruct =
                    (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

                // raise KeyPress
                if (KeyPress != null && wParam == WmKeydown)
                {
                    bool isDownShift = (GetKeyState(VkShift) & 0x8000) == 0x8000 ? true : false;

                    // bool isDownCapslock = (GetKeyState(VK_CAPITAL) != 0 ? true : false);
                    bool isDownControl = (GetKeyState(VkControl) & 0x8000) == 0x8000 ? true : false;

                    if (isDownControl && isDownShift)
                    {
                        s_isDownBothShiftAndCtrlKeys = true;
                    }
                }

                if (KeyDown != null && (wParam == WmKeydown || wParam == WmSyskeydown))
                {
                    var keyData = (Keys)myKeyboardHookStruct.VirtualKeyCode;

                    if (s_isDownBothShiftAndCtrlKeys)
                    {
                        if (Keys.V == keyData)
                        {
                            KeyDown.Invoke(null, null);
                        }
                    }
                }

                // raise KeyUp
                if (KeyUp != null && (wParam == WmKeyup || wParam == WmSyskeyup))
                {
                    s_isDownBothShiftAndCtrlKeys = false;
                    var keyData = (Keys)myKeyboardHookStruct.VirtualKeyCode;
                    var e = new KeyEventArgs(keyData);

                    // s_KeyUp.Invoke(null, e);
                    handled = e.Handled;
                }
            }

            // if event handled in application do not handoff to other listeners
            if (handled)
            {
                return -1;
            }

            // forward to other application
            return CallNextHookEx(s_KeyboardHookHandle, nCode, wParam, lParam);
        }

        /// <summary>
        /// </summary>
        public static void ReleaseKeyboardHook()
        {
            TryUnsubscribeFromGlobalKeyboardEvents();
        }

        /// <summary>
        /// </summary>
        private static void TryUnsubscribeFromGlobalKeyboardEvents()
        {
            // if no subsribers are registered unsubsribe from hook
            if (KeyDown == null && KeyUp == null && KeyPress == null)
            {
                ForceUnsunscribeFromGlobalKeyboardEvents();
            }
        }

        /// <summary>
        /// </summary>
        private static void ForceUnsunscribeFromGlobalKeyboardEvents()
        {
            if (0 == s_KeyboardHookHandle)
            {
                return;
            }

            // uninstall hook
            var result = UnhookWindowsHookEx(s_KeyboardHookHandle);

            // reset invalid handle
            s_KeyboardHookHandle = 0;

            // Free up for GC
            s_KeyboardDelegate = null;

            // if failed and exception must be thrown
            if (result != 0)
            {
                return;
            }

            // Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
            var errorCode = Marshal.GetLastWin32Error();

            // Initializes and throws a new instance of the Win32Exception class with the specified error. 
            throw new Win32Exception(errorCode);
        }
    }
}
