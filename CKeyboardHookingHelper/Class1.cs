using KeyboardHooking;

namespace CKeyboardHookingHelper
{
    public static class CKeyboardHookingHelper
    {
        public static void InitKeyboardHooking()
        {
            HookKeyboard.SetKeyboardHook();
            HookKeyboard.KeyUp += HookKeyboard_KeyUp;
            HookKeyboard.KeyPress += HookKeyboard_KeyPress;
            HookKeyboard.KeyDown += HookKeyboard_KeyDown;
        }

        static void HookKeyboard_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }

        static void HookKeyboard_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
        }

        static void HookKeyboard_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
        }
    }
}
