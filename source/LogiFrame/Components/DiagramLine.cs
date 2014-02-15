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
using System.Collections.Generic;
using System.Linq;

namespace LogiFrame.Components
{
    public class DiagramLine<TKey, TValue> : Component
    {
        public delegate int XAxisConversionDelegate(TKey axisObject);

        public delegate TKey XAxisLimitDelegate(TKey axisObject);

        public delegate int YAxisConversionDelegate(TValue axisObject);

        public delegate TValue YAxisLimitDelegate(TValue axisObject);

        private XAxisLimitDelegate _maxXAxis;
        private YAxisLimitDelegate _maxYAxis;
        private XAxisLimitDelegate _minXAxis;
        private YAxisLimitDelegate _minYAxis;

        private DiagramDataCollection<TKey, TValue> _values = new DiagramDataCollection<TKey, TValue>();
        private XAxisConversionDelegate _xAxisConverter;
        private YAxisConversionDelegate _yAxisConverter;

        public DiagramLine()
        {
            _xAxisConverter = axisObject => (int) (object) axisObject;
            _yAxisConverter = axisObject => (int) (object) axisObject;
            _minXAxis = axisObject => axisObject;
            _minYAxis = axisObject => axisObject;
            _maxXAxis = axisObject => axisObject;
            _maxYAxis = axisObject => axisObject;

            _values.CollectionChanged += _values_CollectionChanged;
        }

        public XAxisConversionDelegate XAxisConverter
        {
            get { return _xAxisConverter; }
            set
            {
                _xAxisConverter = value;
                OnChanged(EventArgs.Empty);
            }
        }

        public YAxisConversionDelegate YAxisConverter
        {
            get { return _yAxisConverter; }
            set
            {
                _yAxisConverter = value;
                OnChanged(EventArgs.Empty);
            }
        }

        public XAxisLimitDelegate MinXAxis
        {
            get { return _minXAxis; }
            set
            {
                _minXAxis = value;
                OnChanged(EventArgs.Empty);
            }
        }

        public YAxisLimitDelegate MinYAxis
        {
            get { return _minYAxis; }
            set
            {
                _minYAxis = value;
                OnChanged(EventArgs.Empty);
            }
        }

        public XAxisLimitDelegate MaxXAxis
        {
            get { return _maxXAxis; }
            set
            {
                _maxXAxis = value;
                OnChanged(EventArgs.Empty);
            }
        }

        public YAxisLimitDelegate MaxYAxis
        {
            get { return _maxYAxis; }
            set
            {
                _maxYAxis = value;
                OnChanged(EventArgs.Empty);
            }
        }

        public DiagramDataCollection<TKey, TValue> Values
        {
            get { return _values; }
            set
            {
                if (!SwapProperty(ref _values, value, true)) return;

                _values.CollectionChanged -= _values_CollectionChanged;
                _values = value;
                _values.CollectionChanged += _values_CollectionChanged;
            }
        }

        protected override Bytemap Render()
        {
            Bytemap bymap = new Bytemap(Size);

            if (_values == null || XAxisConverter == null || YAxisConverter == null || MinXAxis == null ||
                MaxXAxis == null || MinYAxis == null || MaxYAxis == null || !_values.Any())
                return bymap;

            var xOrderedValues = _values.OrderBy(p => XAxisConverter(p.Key));
            var yOrderedValues = _values.OrderBy(p => YAxisConverter(p.Value));

            var minx = XAxisConverter(MinXAxis(xOrderedValues.FirstOrDefault().Key));
            var maxx = XAxisConverter(MaxXAxis(xOrderedValues.LastOrDefault().Key));
            var miny = YAxisConverter(MinYAxis(yOrderedValues.FirstOrDefault().Value));
            var maxy = YAxisConverter(MaxYAxis(yOrderedValues.LastOrDefault().Value));

            var xperpixel = ((float) maxx - minx)/Size.Width;
            var pixelpery = Size.Height/((float) maxy - miny);
            var prefypix = int.MinValue;

            for (var pixelx = 0; pixelx < Size.Width; pixelx++)
            {
                var currentxkey = minx + xperpixel*pixelx;

                var previous = xOrderedValues.LastOrDefault(p => XAxisConverter(p.Key) < currentxkey);
                var current = xOrderedValues.FirstOrDefault(p => (float) XAxisConverter(p.Key) == currentxkey);
                var next = xOrderedValues.FirstOrDefault(p => XAxisConverter(p.Key) > currentxkey);
                var prevf = false;
                var nextf = false;
                var prevx = 0;
                var prevy = 0;
                var nextx = 0;
                var nexty = 0;

                if (
                    !EqualityComparer<KeyValuePair<TKey, TValue>>.Default.Equals(current,
                        default(KeyValuePair<TKey, TValue>)))
                {
                    var pixely = Size.Height - 1 - (int) Math.Floor((YAxisConverter(current.Value) - miny)*pixelpery);
                    if (pixely >= 0 && pixely < Size.Height) bymap.SetPixel(pixelx, pixely, true);
                    prefypix = pixely;
                    continue;
                }
                if (
                    !EqualityComparer<KeyValuePair<TKey, TValue>>.Default.Equals(previous,
                        default(KeyValuePair<TKey, TValue>)))
                {
                    prevx = XAxisConverter(previous.Key);
                    prevy = YAxisConverter(previous.Value);
                    prevf = true;
                }
                if (
                    !EqualityComparer<KeyValuePair<TKey, TValue>>.Default.Equals(next,
                        default(KeyValuePair<TKey, TValue>)))
                {
                    nextx = XAxisConverter(next.Key);
                    nexty = YAxisConverter(next.Value);
                    nextf = true;
                }
                if (!prevf || !nextf) continue;

                var pixy = Size.Height - 1 -
                           (int)
                               Math.Floor(((((nextx - prevx) == 0 ? 0 : ((float) nexty - prevy)/(nextx - prevx))*
                                            (currentxkey - prevx) + prevy) - miny)*pixelpery);

                if (pixy >= 0 && pixy < Size.Height)
                    bymap.SetPixel(pixelx, pixy, true);

                if (prefypix != int.MinValue)
                {
                    if (pixy - prefypix > 1)
                    {
                        for (var py = prefypix + 1; py < pixy; py++)
                            if (py >= 0 && py < Size.Height)
                                bymap.SetPixel(pixelx - 1, py, true);
                    }
                    else if (pixy - prefypix < -1)
                    {
                        for (var py = pixy + 1; py < prefypix; py++)
                            if (py >= 0 && py < Size.Height)
                                bymap.SetPixel(pixelx - 1, py, true);
                    }
                }

                prefypix = pixy;
            }
            return bymap;
        }

        private void _values_CollectionChanged(object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }
    }
}