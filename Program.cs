/*
 *  Main file, entry point of the program
 */
using System.Runtime.InteropServices;

namespace RubiksCube
{
    class Program
    {
        static void Main(string[] args)
        {
            [DllImport("kernel32.dll")]
            static extern IntPtr GetConsoleWindow();

            [DllImport("user32.dll")]
            static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            const int SW_HIDE = 0;
            const int SW_SHOW = 5;

            var handle = GetConsoleWindow();

            // Hide
            ShowWindow(handle, SW_HIDE);

            // Show
            //ShowWindow(handle, SW_SHOW);

            Window window = new Window();
            window.Run();
        }
    }
}
