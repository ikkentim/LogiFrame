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

namespace LogiFrame.Components
{
    /// <summary>
    ///     Represents a drawable square.
    /// </summary>
    public class Square : Component
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

            if (Fill)
            {
                for (int x = 0; x < Size.Width; x++)
                    for (int y = 0; y < Size.Height; y++)
                        result.SetPixel(x, y, true);
            }
            else
            {
                for (int x = 0; x < Size.Width; x++)
                {
                    result.SetPixel(x, 0, true); //Top
                    result.SetPixel(x, Size.Height - 1, true); //Bottom
                }
                for (int y = 0; y < Size.Height; y++)
                {
                    result.SetPixel(0, y, true); //Left
                    result.SetPixel(Size.Width - 1, y, true); //Right
                }
            }
            return result;
        }
    }
}