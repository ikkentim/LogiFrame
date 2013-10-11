
namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a drawable square.
    /// </summary>
    public class Square : Component
    {
        private bool _fill;

        /// <summary>
        /// Whether the LogiFrame.Components.Square should be filled.
        /// </summary>
        public bool Fill
        {
            get
            {
                return _fill;
            }
            set
            {
                bool change = _fill != value;

                _fill = value;

                if (change)
                    HasChanged = true;
            }
        }

        protected override Bytemap Render()
        {
            Bytemap result = new Bytemap(Size);

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
                    result.SetPixel(x, 0, true);
                    result.SetPixel(x, Size.Height - 1, true);
                }
                for (int y = 0; y < Size.Height; y++)
                {
                    result.SetPixel(0, y, true);
                    result.SetPixel(Size.Width - 1, y, true);
                }
            }
            return result;
        }
    }
}
