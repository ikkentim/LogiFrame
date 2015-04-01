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
using System.Drawing;
using System.Linq;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a drawable diagram
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class Diagram<TKey, TValue> : Container
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lowestValue">The lowest value.</param>
        /// <param name="highestValue">The highest value.</param>
        /// <returns></returns>
        public delegate string XAxisLabelDelegate(TKey lowestValue, TKey highestValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lowestValue">The lowest value.</param>
        /// <param name="highestValue">The highest value.</param>
        /// <returns></returns>
        public delegate string YAxisLabelDelegate(TValue lowestValue, TValue highestValue);

        private readonly DiagramLine<TKey, TValue> _diagramLine;

        private readonly Label _hLabel;
        private readonly Line _hLine;
        private readonly Rotator _rotator;
        private readonly Label _vLabel;
        private readonly Line _vLine;

        private XAxisLabelDelegate _xAxisLabel = (lowestValue, highestValue) => lowestValue + "-" + highestValue;

        private YAxisLabelDelegate _yAxisLabel =
            (lowestValue, highestValue) => Activator.CreateInstance(lowestValue.GetType()) + "-" + highestValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Diagram{TKey, TValue}"/> class.
        /// </summary>
        public Diagram()
        {
            Components.Add(_hLine = new Line());
            Components.Add(_vLine = new Line());
            Components.Add(_diagramLine = new DiagramLine<TKey, TValue>
            {
                Location = new Location(1, 0),
            });

            Components.Add(_rotator = new Rotator
            {
                Location = new Location(1, 0),
                Rotation = Rotation.Rotate90Degrees,
                IsTransparent = true,
                IsTopEffectEnabled = true
            });

            _rotator.Components.Add(_vLabel = new Label
            {
                IsAutoSize = true,
                VerticalAlignment = Alignment.Bottom,
                HorizontalAlignment = Alignment.Center,
                Font = new Font("Arial", 7f),
            });
            Components.Add(_hLabel = new Label
            {
                IsAutoSize = true,
                IsTransparent = true,
                IsTopEffectEnabled = true,
                VerticalAlignment = Alignment.Bottom,
                HorizontalAlignment = Alignment.Center,
                Font = new Font("Arial", 7f),
            });
        }

        /// <summary>
        /// Gets the line.
        /// </summary>
        public DiagramLine<TKey, TValue> Line
        {
            get { return _diagramLine; }
        }

        /// <summary>
        /// Gets or sets the x axis label.
        /// </summary>
        public XAxisLabelDelegate XAxisLabel
        {
            get { return _xAxisLabel; }
            set
            {
                _xAxisLabel = value;
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the y axis label.
        /// </summary>
        public YAxisLabelDelegate YAxisLabel
        {
            get { return _yAxisLabel; }
            set
            {
                _yAxisLabel = value;
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Renders all graphics of this <see cref="Container" />.
        /// </summary>
        /// <returns>
        /// The rendered <see cref="Snapshot" />.
        /// </returns>
        protected override Snapshot Render()
        {
            if (_diagramLine.Values.Any())
            {
                IOrderedEnumerable<KeyValuePair<TKey, TValue>> xOrderedValues =
                    _diagramLine.Values.OrderBy(p => _diagramLine.XAxisConverter(p.Key));
                IOrderedEnumerable<KeyValuePair<TKey, TValue>> yOrderedValues =
                    _diagramLine.Values.OrderBy(p => _diagramLine.YAxisConverter(p.Value));

                TKey minx = _diagramLine.MinXAxis(xOrderedValues.FirstOrDefault().Key);
                TKey maxx = _diagramLine.MaxXAxis(xOrderedValues.LastOrDefault().Key);
                TValue miny = _diagramLine.MinYAxis(yOrderedValues.FirstOrDefault().Value);
                TValue maxy = _diagramLine.MaxYAxis(yOrderedValues.LastOrDefault().Value);


                if (XAxisLabel != null &&
                    _diagramLine.MinXAxis != null &&
                    _diagramLine.MaxXAxis != null &&
                    minx != null && maxx != null)
                    _hLabel.Text = XAxisLabel(minx, maxx);
                else
                    _hLabel.Text = String.Empty;

                if (YAxisLabel != null &&
                    _diagramLine.MinYAxis != null &&
                    _diagramLine.MaxYAxis != null &&
                    miny != null && maxy != null)
                    _vLabel.Text = YAxisLabel(miny, maxy);
                else
                    _vLabel.Text = String.Empty;
            }
            else
            {
                _hLabel.Text = String.Empty;
                _vLabel.Text = String.Empty;
            }

            _rotator.Size = new Size(Size.Height - 1, 10);

            _vLabel.Location = new Location(_rotator.Size.Width/2, _rotator.Size.Height + 1);
            _hLabel.Location = new Location(Size.Width/2, Size.Height);

            _hLine.Start = new Location(0, Size.Height - 1);
            _hLine.End = new Location(Size.Width - 1, Size.Height - 1);

            _vLine.Start = new Location(0, 0);
            _vLine.End = new Location(0, Size.Height - 1);

            _diagramLine.Size = new Size(Size.Width - 1, Size.Height - 1);

            return base.Render();
        }
    }
}