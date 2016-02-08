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
        private struct KeyboardHookStruct
        {
            /// <summary>
            /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
            /// </summary>
            public int VirtualKeyCode { get; set; }
            
            /// <summary>
            /// Specifies a hardware scan code for the key. 
            /// </summary>
            public int ScanCode { get; set; }

            /// <summary>
            /// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
            /// </summary>
            public int Flags { get; set; }

            /// <summary>
            /// Specifies the Time stamp for this message.
            /// </summary>
            public int Time { get; set; }

            /// <summary>
            /// Specifies extra information associated with the message. 
            /// </summary>
            public int ExtraInfo { get; set; }
        }
    }
}
