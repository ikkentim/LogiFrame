using System;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a drawable scroll bar.
    /// </summary>
    public class ScrollBarIncomplete : Container//Incomplete component
    {
        private readonly Square _square = new Square();
        private bool _horizontal = true;
        private float _maximumValue = 100;
        private float _value;

        /// <summary>
        /// Initializes a new instance of the LogiFrame.Components.ScrollBar class.
        /// </summary>
        public ScrollBarIncomplete()
        {
            Components.Add(_square);
            _square.Fill = true;
        }

        /// <summary>
        /// Gets or sets whether the scroll bar is progressing horizontally.
        /// </summary>
        public bool Horizontal
        {
            get { return _horizontal; }
            set
            {
                if (SwapProperty(ref _horizontal, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the maximum Value this LogiFrame.Components.ScrollBar can have.
        /// </summary>
        public float MaximumValue
        {
            get { return _maximumValue; }
            set
            {
                if (SwapProperty(ref _maximumValue, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the Value of this LogiFrame.Components.ScrollBar.
        /// </summary>
        public float Value
        {
            get { return _value; }
            set
            {
                if (SwapProperty(ref _value, value, false))
                    OnChanged(EventArgs.Empty);
            }
        }

        protected override Bytemap Render()
        {
            float progress = _maximumValue > 0 ? _value / _maximumValue : 0;

            if (_horizontal)
                _square.Size.Set((int) ((Size.Width)*progress), Size.Height);
            else
                _square.Size.Set(Size.Width, (int) ((Size.Height)*progress));

            return base.Render();
        }
    }
}
