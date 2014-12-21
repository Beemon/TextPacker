using System;
using System.Runtime.InteropServices;
namespace System
{
	public static class Win32
	{
		public const int WM_DRAWCLIPBOARD = 776;
		public const int WM_CHANGECBCHAIN = 781;
		[DllImport("user32.dll")]
		public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);
		[DllImport("user32.dll")]
		public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
	}
}
