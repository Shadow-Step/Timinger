using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Timinger
{
    public static class KeyRegister
    {
        public static Dictionary<string, int> KeyDict { get; set; } = new Dictionary<string, int>()
        {
            {"F1", 0x0070},
            {"F2", 0x0071},
            {"F3", 0x0072},
            {"F4", 0x0073},
            {"F5", 0x0074},
            {"F6", 0x0075},
            {"F7", 0x0076},
            {"F8", 0x0077},
            {"F9", 0x0078},
            {"F10", 0x0079},
            {"F11", 0x007A},
        };
        public static Dictionary<int, int> VirtualKeyCodes { get; set; } = new Dictionary<int, int>()
        {
            {0x0070,0x00700000},
            {0x0071,0x00710000},
            {0x0072,0x00720000},
            {0x0073,0x00730000},
            {0x0074,0x00740000},
            {0x0075,0x00750000},
            {0x0076,0x00760000},
            {0x0077,0x00770000},
            {0x0078,0x00780000},
            {0x0079,0x00790000},
            {0x007A,0x007A0000},
            {0x007B,0x007B0000}
        };
        public static UInt32 MOUSE_BUTTONDOWN = 0x0002;
        public static UInt32 MOUSE_BUTTONUP = 0x0004;

        private static bool Registered { get; set; } = false;
        public static int HotKey { get; set; } = 0;
        public static int KeyId { get; set; } = 0;
        public static int KeyValue { get; set; }
        public static IntPtr hWnd { get; set; }

        [DllImport("user32.dll")]
        public static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, int dwData, IntPtr dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(int hwndParent, int hwndEnfant, int lpClasse, string title);
        [DllImport("user32.dll")]
        private static extern int RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(IntPtr hWnd, int id);

        public static void RegisterKey(string Key)
        {
            if (!KeyDict.ContainsKey(Key))
                throw new Exception();
            if (Registered)
                UnregisterKey(hWnd);
            int x = 0;
            KeyValue = KeyDict[Key];
            x = RegisterHotKey(hWnd, KeyId, 0, KeyValue);
            Registered = true;
        }
        public static void UnregisterKey(IntPtr hWnd)
        {
            UnregisterHotKey(hWnd, KeyId);
            Registered = false;
        }
        public static void Click()
        {
            KeyRegister.mouse_event(KeyRegister.MOUSE_BUTTONDOWN, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(50);
            KeyRegister.mouse_event(KeyRegister.MOUSE_BUTTONUP, 0, 0, 0, IntPtr.Zero);
        }
        public static void Click(int milliseconds)
        {
            Thread.Sleep(milliseconds);
            KeyRegister.mouse_event(KeyRegister.MOUSE_BUTTONDOWN, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(50);
            KeyRegister.mouse_event(KeyRegister.MOUSE_BUTTONUP, 0, 0, 0, IntPtr.Zero);
        }
    }
}
