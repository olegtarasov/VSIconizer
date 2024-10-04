//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NotLimited.Framework.Common.Helpers.Native
{
    public class MouseMoveEventArgs : EventArgs
    {
        public MouseMoveEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }

    public sealed class MouseHook : IDisposable
    {
        private static class NativeMethods
        {
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable UnusedMember.Local
            internal const int WH_MOUSE_LL = 14;


            internal enum MouseMessages
            {
                WM_LBUTTONDOWN = 0x0201,
                WM_LBUTTONUP = 0x0202,
                WM_MOUSEMOVE = 0x0200,
                WM_MOUSEWHEEL = 0x020A,
                WM_RBUTTONDOWN = 0x0204,
                WM_RBUTTONUP = 0x0205
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct MSLLHOOKSTRUCT
            {
                public PointApi pt;
                public uint mouseData;
                public uint flags;
                public uint time;
                public IntPtr dwExtraInfo;
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
                                                        IntPtr wParam, IntPtr lParam);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern IntPtr GetModuleHandle(string lpModuleName);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetCursorPos(out PointApi lpPoint);

// ReSharper restore UnusedMember.Local
// ReSharper restore MemberCanBePrivate.Local
// ReSharper restore FieldCanBeMadeReadOnly.Local
// ReSharper restore InconsistentNaming
        }

        private IntPtr _hookId = IntPtr.Zero;

        public event EventHandler<MouseMoveEventArgs> MouseMove;
        public event EventHandler LButtonUp;

        private void OnLButtonUp(EventArgs e)
        {
            if (LButtonUp != null) 
                LButtonUp(this, e);
        }

        private void OnMouseMove(MouseMoveEventArgs e)
        {
            if (MouseMove != null) 
                MouseMove(null, e);
        }


        public void SetHook()
        {
            _hookId = SetHook(HookCallback);
        }

        public void RemoveHook()
        {
            if (_hookId != IntPtr.Zero)
            {
                if (!NativeMethods.UnhookWindowsHookEx(_hookId))
                    throw new InvalidOperationException("Failed to remove the hook!");

                _hookId = IntPtr.Zero;
            }
        }

        public PointApi GetCursorPos()
        {
            PointApi result;
            NativeMethods.GetCursorPos(out result);
            return result;
        }

        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return NativeMethods.SetWindowsHookEx(NativeMethods.WH_MOUSE_LL, proc, NativeMethods.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var hookStruct = (NativeMethods.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeMethods.MSLLHOOKSTRUCT));
                switch ((NativeMethods.MouseMessages)wParam)
                {
                    case NativeMethods.MouseMessages.WM_MOUSEMOVE:
                        OnMouseMove(new MouseMoveEventArgs(hookStruct.pt.x, hookStruct.pt.y));
                        break;
                    case NativeMethods.MouseMessages.WM_LBUTTONUP:
                        OnLButtonUp(EventArgs.Empty);
                        break;
                }
            }

            return NativeMethods.CallNextHookEx(_hookId, nCode, wParam, lParam);
        }

        

        public void Dispose()
        {
            RemoveHook();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PointApi
    {
        public int x;
        public int y;
    }
}