using System;
using LogiFrame;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a drawable circle.
    /// </summary>
    public class Circle : Component
    {
        private bool fill;

        /// <summary>
        /// Whether the LogiFrame.Components.Square should be filled.
        /// </summary>
        public bool Fill
        {
            get
            {
                return fill;
            }
            set
            {
                bool change = fill != value;

                fill = value;

                if (change)
                    HasChanged = true;
            }
        }

        protected override Bytemap Render()
        {
            Bytemap result = new Bytemap(Size);

            double radius = Math.Min(Size.Width, Size.Height)/2-1;

            for (int j = 1; j <= 25; j++)
            {
                radius = (j + 1) * 5;
                for (double i = 0.0; i < 360.0; i += 0.1)
                {
                    double angle = i * System.Math.PI / 180;
                    int x = (int)(Size.Width/2 + radius * System.Math.Cos(angle));
                    int y = (int)(Size.Height/2 + radius * System.Math.Sin(angle));

                    result.SetPixel(x, y, true);
                    //System.Threading.Thread.Sleep(1); // If you want to draw circle very slowly.
                }
            }

            return result;
        }
    }
}
