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

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a drawable circle.
    /// </summary>
    public class Circle : Component
    {
        private bool _fill;

        /// <summary>
        ///     Gets or sets whether the <see cref="Square" /> should be filled.
        /// </summary>
        public bool Fill
        {
            get { return _fill; }
            set { SwapProperty(ref _fill, value); }
        }

        /// <summary>
        ///     Renders all graphics of this <see cref="Component" />.
        /// </summary>
        /// <returns>
        ///     The rendered <see cref="Bytemap" />.
        /// </returns>
        protected override Bytemap Render()
        {
            // TODO: improve algorithm

            var result = new Bytemap(Size);

            double hradius = (double) Size.Width/2;
            double vradius = (double) Size.Height/2;

            for (double i = 0.0; i < 360.0; i += 0.1)
            {
                double angle = i*(Math.PI/180);
                var x = (int) Math.Floor(hradius + (hradius - 1)*Math.Cos(angle));
                var y = (int) Math.Floor(vradius + (vradius - 1)*Math.Sin(angle));

                result.SetPixel(x, y, true);
            }

            return result;
        }
    }
}