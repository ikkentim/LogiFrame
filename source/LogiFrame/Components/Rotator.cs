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
    ///     Represents a LogiFrame.Components.Container which can be rotated.
    /// </summary>
    public class Rotator : Container
    {
        private Rotation _rotation;

        /// <summary>
        ///     Gets or sets the LogiFrame.Components.Rotation of this LogiFrame.Components.Rotator.
        /// </summary>
        public Rotation Rotation
        {
            get { return _rotation; }
            set { SwapProperty(ref _rotation, value); }
        }

        protected override Bytemap Render()
        {
            //Sum rotation
            int rotation = 0;
            if (Rotation.HasFlag(Rotation.Rotate90Degrees)) rotation += 90;
            if (Rotation.HasFlag(Rotation.Rotate180Degrees)) rotation += 180;
            if (Rotation.HasFlag(Rotation.Rotate270Degrees)) rotation += 270;
            rotation = rotation%360;

            //Render original result
            Bytemap result = base.Render();

            //Calculate goal canvas
            var endresult = new Bytemap(rotation == 90 || rotation == 270 ? Size.Height : Size.Width,
                rotation == 90 || rotation == 270 ? Size.Width : Size.Height);

            //Rotation algorithm
            switch (rotation)
            {
                case 0:
                    endresult = result;
                    break;
                case 90:
                    for (int x = 0; x < Size.Width; x++)
                        for (int y = 0; y < Size.Height; y++)
                            endresult.Data[x*Size.Height + (Size.Height - 1 - y)] = result.Data[x + Size.Width*y];
                    break;
                case 180:
                    for (int x = 0; x < Size.Width; x++)
                        for (int y = 0; y < Size.Height; y++)
                            endresult.Data[(Size.Width - 1 - x) + Size.Width*(Size.Height - 1 - y)] =
                                result.Data[x + Size.Width*y];
                    break;
                case 270:
                    for (int x = 0; x < Size.Width; x++)
                        for (int y = 0; y < Size.Height; y++)
                            endresult.Data[(Size.Width - 1 - x)*Size.Height + (y)] = result.Data[x + Size.Width*y];
                    break;
            }

            //Horizontal flip
            if (Rotation.HasFlag(Rotation.FlipHorizontal))
            {
                Bytemap hflipsrc = endresult;
                endresult = new Bytemap(endresult.Size);
                for (int x = 0; x < endresult.Size.Width; x++)
                    for (int y = 0; y < endresult.Size.Height; y++)
                        endresult.Data[endresult.Size.Width - 1 - x + y*endresult.Size.Width] =
                            hflipsrc.Data[x + y*endresult.Size.Width];
            }

            //Vertical flip
            if (Rotation.HasFlag(Rotation.FlipVertical))
            {
                Bytemap vflipsrc = endresult;
                endresult = new Bytemap(endresult.Size);
                for (int x = 0; x < endresult.Size.Width; x++)
                    for (int y = 0; y < endresult.Size.Height; y++)
                        endresult.Data[x + (endresult.Size.Height - 1 - y)*endresult.Size.Width] =
                            vflipsrc.Data[x + y*endresult.Size.Width];
            }
            return endresult;
        }
    }
}