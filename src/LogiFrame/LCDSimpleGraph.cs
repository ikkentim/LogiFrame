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
using System.Collections.Generic;
using System.Linq;

namespace LogiFrame
{
    /// <summary>
    /// Represents a simple graph.
    /// </summary>
    public class LCDSimpleGraph : LCDControl
    {
        private readonly Queue<int> _values = new Queue<int>();
        private int _maximum = 100;
        private int _minimum;
        private BorderStyle _style;

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;
                if (_maximum < value) _maximum = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                if (_minimum > value) _minimum = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the style of the graph.
        /// </summary>
        public BorderStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets the maximum number of entries in this graph.
        /// </summary>
        public int MaxEntries
                    => Math.Max(0, Width - (Style == BorderStyle.Border ? 2 : (Style == BorderStyle.BorderWithPadding ? 4 : 0)))
                    ;

        /// <summary>
        /// Gets the values in the graph.
        /// </summary>
        public IEnumerable<int> Values => _values.ToArray();

        /// <summary>
        /// Pushes the specified value to the graph.
        /// </summary>
        public void PushValue(int value)
        {
            ThrowIfDisposed();

            _values.Enqueue(value);
            if (_values.Count > MaxEntries)
                _values.Dequeue();

            Invalidate();
        }

        #region Overrides of LCDControl

        /// <summary>
        /// Raises the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LogiFrame.LCDPaintEventArgs" /> instance containing the event data.</param>
        protected override void OnPaint(LCDPaintEventArgs e)
        {
            int graphX, graphY, graphWidth, graphHeight;
            switch (Style)
            {
                case BorderStyle.Border:
                    graphX = 1;
                    graphY = 1;
                    graphWidth = Width - 2;
                    graphHeight = Height - 2;
                    break;
                case BorderStyle.BorderWithPadding:
                    graphX = 2;
                    graphY = 2;
                    graphWidth = Width - 4;
                    graphHeight = Height - 4;
                    break;
                case BorderStyle.None:
                default:
                    graphX = 0;
                    graphY = 0;
                    graphWidth = Width;
                    graphHeight = Height;
                    break;
            }

            if (Style == BorderStyle.Border || Style == BorderStyle.BorderWithPadding)
            {
                for (var x = 1; x < Width - 1; x++)
                {
                    e.Bitmap[x, 0] = true;
                    e.Bitmap[x, Height - 1] = true;
                }
                for (var y = 0; y < Height; y++)
                {
                    e.Bitmap[0, y] = true;
                    e.Bitmap[Width - 1, y] = true;
                }
            }

            if (graphWidth <= 0 || graphHeight <= 0 || !_values.Any())
            {
                base.OnPaint(e);
                return;
            }

            var cx = graphX + MaxEntries - _values.Count;

            foreach (
                var height in
                    _values.Select(value => (float) (value - Minimum)/(Maximum - Minimum))
                        .Select(value => Math.Min(1f, Math.Max(0f, value)))
                        .Select(value => (int) (value*graphHeight)))
            {
                for (var y = 0; y < height; y++)
                    e.Bitmap[cx, graphY + graphHeight - y] = true;

                cx++;
            }

            base.OnPaint(e);
        }

        #endregion
    }
}