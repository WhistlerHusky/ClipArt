using System.Windows.Forms;

namespace MouseHooking
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class HookMouse
    {
        private static event MouseEventHandler s_MouseMove;
        public static event MouseEventHandler MouseMove
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                s_MouseMove += value;
            }

            remove
            {
                s_MouseMove -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static event MouseEventHandler s_leftMouseDown;
        private static event MouseEventHandler s_MouseClick;
        private static event MouseEventHandler s_MouseClickExt;
        private static event MouseEventHandler s_MouseWheel;
        private static event MouseEventHandler s_MouseUp;
        private static event MouseEventHandler s_MouseDoubleClick;

        private static void EnsureSubscribedToGlobalMouseEvents()
        {


        }
    }

}
