// LogiFrame
// Copyright 2015 Tim Potze
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Runtime.InteropServices;

namespace LogiFrame.Internal
{
    internal static class LgLcd
    {
        public delegate int OnConfigureDelegate(int connection, IntPtr pContext);

        public delegate int OnSoftButtonsDelegate(int device, int dwButtons, IntPtr pContext);

        public const int InvalidConnection = -1;
        public const int InvalidDevice = -1;
        public const uint ButtonButton0 = 0x00000001;
        public const uint ButtonButton1 = 0x00000002;
        public const uint ButtonButton2 = 0x00000004;
        public const uint ButtonButton3 = 0x00000008;
        public const uint BitmapFormat160X43X1 = 0x00000001;
        public const uint BitmapWidth = 160;
        public const uint BitmapHeight = 43;
        public const uint PriorityIdleNoShow = 0;
        public const uint PriorityBackground = 64;
        public const uint PriorityNormal = 128;
        public const uint PriorityAlert = 255;

        [DllImport("LgLcd.dll", EntryPoint = "lgLcdInit")]
        public static extern int Init();

        [DllImport("LgLcd.dll", EntryPoint = "lgLcdDeInit")]
        public static extern int DeInit();

        [DllImport("LgLcd.dll", EntryPoint = "lgLcdConnect")]
        public static extern int Connect(ref ConnectContext ctx);

        [DllImport("LgLcd.dll", EntryPoint = "lgLcdDisconnect")]
        public static extern int Disconnect(int connection);

        [DllImport("LgLcd.dll", EntryPoint = "lgLcdEnumerate")]
        public static extern int Enumerate(int connection, int index, out DeviceDescription description);

        [DllImport("LgLcd.dll", EntryPoint = "lgLcdOpen")]
        public static extern int Open(ref OpenContext ctx);

        [DllImport("LgLcd.dll", EntryPoint = "lgLcdClose")]
        public static extern int Close(int device);

        [DllImport("LgLcd.dll", EntryPoint = "lgLcdReadSoftButtons")]
        public static extern int ReadSoftButtons(int device, out int buttons);

        [DllImport("LgLcd.dll", EntryPoint = "lgLcdUpdateBitmap")]
        public static extern int UpdateBitmap(int device, ref Bitmap160X43X1 bitmap, uint priority);

        [StructLayout(LayoutKind.Sequential)]
        public struct Bitmap160X43X1
        {
            public BitmapHeader hdr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6880)] public byte[] pixels;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BitmapHeader
        {
            public uint Format;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ConfigureContext
        {
            public OnConfigureDelegate ConfigCallback;
            public IntPtr ConfigContext;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ConnectContext
        {
            public string AppFriendlyName;
            public bool IsPersistent;
            public bool IsAutostartable;
            public ConfigureContext OnConfigure;
            public int Connection;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DeviceDescription
        {
            public int Width;
            public int Height;
            public int BitsPerPixel;
            public int SoftButtonsCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OpenContext
        {
            public int Connection;
            public int Index;
            public SoftButtonsChangedContext OnSoftButtonsChanged;
            public int Device;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SoftButtonsChangedContext
        {
            public OnSoftButtonsDelegate Callback;
            public IntPtr Context;
        }
    }
}