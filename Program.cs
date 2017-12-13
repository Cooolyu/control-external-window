
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


namespace helloWorld
{
    class Program
    {

        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int VK_CONTROL = 0x11;
        public const int VK_F5 = 0x74;
        public const int KEYEVENTF_KEYUP = 0x2;
        public const int VK_MENU = 0x12;
        public const int WM_SETTEXT = 0xC;
        public const int WM_CLEAR = 0x303;
        public const int BN_CLICKED = 0;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_CLOSE = 0x10;
        public const int WM_COMMAND = 0x111;
        public const int WM_SYSKEYDOWN = 0x104;
        public const int GW_HWNDNEXT = 2;
        public const int WM_CLICK = 0x00F5;



        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        //参数1:指的是类名。参数2，指的是窗口的标题名。两者至少要知道1个
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);
        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr hWndParent, WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        private static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);

        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hwnd, uint wMsg, int wParam, string lParam);



        /**
        定义控件属性
        **/
        public struct WindowInfo
        {
            public IntPtr hWnd;
            public string SzWindowName;
            public string SzClassName;
        }


        public static List<WindowInfo> GetWindowByParentHwndAndClassName(IntPtr parentHwnd, string className)
        {
            List<WindowInfo> wndList = new List<WindowInfo>();
            EnumChildWindows(parentHwnd, delegate (IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);
                wnd.hWnd = hWnd;
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.SzWindowName = sb.ToString();
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.SzClassName = sb.ToString();
                wndList.Add(wnd);
                return true;
            }, 0);
            return wndList.Where(o => o.SzClassName == className).ToList();
        }


        static void Main(string[] args)
        {
            IntPtr win = FindWindow(null, "计算器");
            if (win != IntPtr.Zero)
            {
                IntPtr winForm = FindWindowEx(win, new IntPtr(), "CalcFrame", null);
                if (winForm != IntPtr.Zero)
                {
                    IntPtr winForm1 = FindWindowEx(winForm, new IntPtr(), "#32770", null);
                    IntPtr winForm2 = FindWindowEx(winForm, winForm1, "#32770", null);

                    List<WindowInfo> buttons = GetWindowByParentHwndAndClassName(winForm2, "Button");
                    // foreach(WindowInfo b in buttons){
                    //     Console.WriteLine(b.hWnd);
                    // }

                    IntPtr button1 = buttons[4].hWnd;
                    IntPtr button2 = buttons[10].hWnd;
                    IntPtr buttonAdd = buttons[22].hWnd;
                    IntPtr buttonEqual = buttons[27].hWnd;

                    //按钮点击 1+2
                    SendMessage(button1, WM_CLICK, 0, "0");
                    System.Threading.Thread.Sleep(3000);

                    SendMessage(buttonAdd, WM_CLICK, 0, "0");
                    System.Threading.Thread.Sleep(3000);
                    
                    SendMessage(button2, WM_CLICK, 0, "0");
                    System.Threading.Thread.Sleep(3000);

                    SendMessage(buttonEqual, WM_CLICK, 0, "0");

                }
                // IntPtr resultEdit = SendMessage(winEdit, 0xC, 0, "4567");
            }
            else
            {
                Console.WriteLine("not find");
            }
        }
    }
}
