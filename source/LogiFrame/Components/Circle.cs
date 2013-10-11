﻿using System;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a drawable circle.
    /// </summary>
    public class Circle : Component
    {
        private bool _fill;

        /// <summary>
        /// Whether the LogiFrame.Components.Square should be filled.
        /// </summary>
        public bool Fill
        {
            get { return _fill; }
            set
            {
                if (_fill == value)
                    return;

                _fill = value;
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