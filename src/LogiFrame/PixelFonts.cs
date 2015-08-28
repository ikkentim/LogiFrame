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
using System.Drawing.Text;
using System.Runtime.InteropServices;
using LogiFrame.Properties;

namespace LogiFrame
{
    /// <summary>
    ///     Contains pixel fonts.
    /// </summary>
    public static class PixelFonts
    {
        private static readonly PrivateFontCollection FontCollection = new PrivateFontCollection();

        /// <summary>
        ///     Initializes the <see cref="PixelFonts" /> class.
        /// </summary>
        static PixelFonts()
        {
            FontCollection.AddFontFromResource(Resources.F6px2bus);
            FontCollection.AddFontFromResource(Resources.F04B03);
            FontCollection.AddFontFromResource(Resources.F04B08);

            TitleFamily = FontCollection.Families[1];
            SmallFamily = FontCollection.Families[0];
            CapitalsFamily = FontCollection.Families[2];
        }

        /// <summary>
        ///     Gets the title pixel font family. Adviced font emSize 5.7f or bigger.
        /// </summary>
        public static FontFamily TitleFamily { get; }

        /// <summary>
        ///     Gets the small pixel font family. Adviced font emSize: 6f.
        /// </summary>
        public static FontFamily SmallFamily { get; }

        /// <summary>
        ///     Gets the all-caps pixel font family. Adviced font emSize 4.2f or bigger.
        /// </summary>
        public static FontFamily CapitalsFamily { get; }

        /// <summary>
        ///     Gets the title font.
        /// </summary>
        public static Font Title => new Font(TitleFamily, 5.7f);

        /// <summary>
        ///     Gets the small font.
        /// </summary>
        public static Font Small => new Font(SmallFamily, 6f);

        /// <summary>
        ///     Gets the capitals font.
        /// </summary>
        public static Font Capitals => new Font(CapitalsFamily, 4.2f);

        private static void AddFontFromResource(this PrivateFontCollection privateFontCollection, byte[] font)
        {
            if (privateFontCollection == null) throw new ArgumentNullException(nameof(privateFontCollection));
            if (font == null) throw new ArgumentNullException(nameof(font));

            var fontData = Marshal.AllocCoTaskMem(font.Length);
            Marshal.Copy(font, 0, fontData, font.Length);
            privateFontCollection.AddMemoryFont(fontData, font.Length);
            Marshal.FreeCoTaskMem(fontData);
        }
    }
}