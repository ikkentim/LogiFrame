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

            double hradius = (double)Size.Width / 2;
            double vradius = (double)Size.Height / 2;

            for (double i = 0.0; i < 360.0; i += 0.1)
            {
                double angle = i * (Math.PI / 180);
                int x = (int)Math.Floor(hradius + (hradius - 1) * Math.Cos(angle));
                int y = (int)Math.Floor(vradius + (vradius - 1) * Math.Sin(angle));

                result.SetPixel(x, y, true);

            }

            return result;
        }
    }
}
