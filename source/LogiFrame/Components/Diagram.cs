// LogiFrame rendering library.
// Copyright (C) 2014 Tim Potze
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>. 

using System;
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
                var xOrderedValues = _diagramLine.Values.OrderBy(p => _diagramLine.XAxisConverter(p.Key));
                var yOrderedValues = _diagramLine.Values.OrderBy(p => _diagramLine.YAxisConverter(p.Value));

                var minx = _diagramLine.MinXAxis(xOrderedValues.FirstOrDefault().Key);
                var maxx = _diagramLine.MaxXAxis(xOrderedValues.LastOrDefault().Key);
                var miny = _diagramLine.MinYAxis(yOrderedValues.FirstOrDefault().Value);
                var maxy = _diagramLine.MaxYAxis(yOrderedValues.LastOrDefault().Value);


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