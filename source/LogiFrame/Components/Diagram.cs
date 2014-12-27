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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LogiFrame.Components
{
    public class Diagram<TKey, TValue> : Container
    {
        public delegate string XAxisLabelDelegate(TKey lowestValue, TKey highestValue);

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
                Transparent = true,
                TopEffect = true
            });

            _rotator.Components.Add(_vLabel = new Label
            {
                AutoSize = true,
                VerticalAlignment = Alignment.Bottom,
                HorizontalAlignment = Alignment.Center,
                Font = new Font("Arial", 7f),
            });
            Components.Add(_hLabel = new Label
            {
                AutoSize = true,
                Transparent = true,
                TopEffect = true,
                VerticalAlignment = Alignment.Bottom,
                HorizontalAlignment = Alignment.Center,
                Font = new Font("Arial", 7f),
            });
        }

        public DiagramLine<TKey, TValue> Line
        {
            get { return _diagramLine; }
        }

        public XAxisLabelDelegate XAxisLabel
        {
            get { return _xAxisLabel; }
            set
            {
                _xAxisLabel = value;
                OnChanged(EventArgs.Empty);
            }
        }

        public YAxisLabelDelegate YAxisLabel
        {
            get { return _yAxisLabel; }
            set
            {
                _yAxisLabel = value;
                OnChanged(EventArgs.Empty);
            }
        }

        protected override Bytemap Render()
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

            _rotator.Size.Set(Size.Height - 1, 10);

            _vLabel.Location.Set(_rotator.Size.Width/2, _rotator.Size.Height + 1);
            _hLabel.Location.Set(Size.Width/2, Size.Height);

            _hLine.Start.Set(0, Size.Height - 1);
            _hLine.End.Set(Size.Width - 1, Size.Height - 1);

            _vLine.Start.Set(0, 0);
            _vLine.End.Set(0, Size.Height - 1);

            _diagramLine.Size.Set(Size.Width - 1, Size.Height - 1);

            return base.Render();
        }
    }
}