// LogiFrame
// Copyright (C) 2014 Tim Potze
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>

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
        ///     Gets or sets whether the LogiFrame.Components.Square should be filled.
        /// </summary>
        public bool Fill
        {
            get { return _fill; }
            set { SwapProperty(ref _fill, value); }
        }

        protected override Bytemap Render()
        {
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