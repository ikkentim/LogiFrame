using System;
using LogiFrame;
using System.Diagnostics;

namespace LogiFrame.Components
{
    /// <summary>
    /// The sole purpose of this class is testing the rendering system.
    /// </summary>
    public class Test : Component
    {
        protected override Bytemap Render()
        {
            Bytemap result = new Bytemap(Size);

            for (int x = 0; x < Size.Width; x++)
                for (int y = 0; y < Size.Height; y++)
                    result.SetPixel(x, y, true);

            return result;
        }
    }
}
