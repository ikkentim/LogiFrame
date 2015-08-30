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
using System.Drawing;

namespace LogiFrame.Drawing
{
    public class MonochromeBitmap
    {
        public MonochromeBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Data = new byte[width*height];
        }

        public MonochromeBitmap(MonochromeBitmap bitmap, int width, int height) : this(width, height)
        {
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));

            for (var x = Math.Min(Width, bitmap.Width) - 1; x >= 0; x--)
                for (var y = Math.Min(Height, bitmap.Height) - 1; y >= 0; y--)
                    this[x, y] = bitmap[x, y];
        }

        public MonochromeBitmap(Bitmap bitmap, int width, int height) : this(width, height)
        {
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));

            for (var x = Math.Min(Width, bitmap.Width) - 1; x >= 0; x--)
                for (var y = Math.Min(Height, bitmap.Height) - 1; y >= 0; y--)
                    this[x, y] = IsColorFilled(bitmap.GetPixel(x, y));
        }

        public MonochromeBitmap(Bitmap bitmap)
        {
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));

            Width = bitmap.Width;
            Height = bitmap.Height;
            Data = new byte[Width*Height];

            for (var x = Math.Min(Width, bitmap.Width) - 1; x >= 0; x--)
                for (var y = Math.Min(Height, bitmap.Height) - 1; y >= 0; y--)
                    this[x, y] = IsColorFilled(bitmap.GetPixel(x, y));
        }

        public int Width { get; }
        public int Height { get; }
        public byte[] Data { get; }

        public bool this[int x, int y]
        {
            get { return x >= 0 && x < Width && y >= 0 && y < Height && Data[x + y*Width] == 0xff; }
            set
            {
                if (x < 0 || x >= Width || y < 0 || y >= Height) return;
                Data[x + y*Width] = (byte) (value ? 0xff : 0x00);
            }
        }

        public bool this[Point location]
        {
            get { return this[location.X, location.Y]; }
            set { this[location.X, location.Y] = value; }
        }

        public void Merge(MonochromeBitmap bitmap, Point location, IMergeMethod mergeMethod)
        {
            if (bitmap == null) return;
            if (mergeMethod == null) throw new ArgumentNullException(nameof(mergeMethod));

            mergeMethod.Merge(bitmap, this, location);
        }

        public void MergeOverride(MonochromeBitmap bitmap, Point location)
        {
            Merge(bitmap, location, MergeMethods.Override);
        }

        public void Reset()
        {
            for (var i = 0; i < Data.Length; i++) Data[i] = 0x00;
        }

        private static bool IsColorFilled(Color color)
        {
            return color.A == 255 && color.R < 64 && color.G < 64 && color.B < 64;
        }
    }
}