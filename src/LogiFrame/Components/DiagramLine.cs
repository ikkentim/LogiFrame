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
using System.Collections.Specialized;
using System.Linq;

namespace LogiFrame.Components
{
    /// <summary>
    /// Represents a line in a <see cref="Diagram`2"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class DiagramLine<TKey, TValue> : Component
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="axisObject">The axis object.</param>
        /// <returns></returns>
        public delegate int XAxisConversionDelegate(TKey axisObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axisObject">The axis object.</param>
        /// <returns></returns>
        public delegate TKey XAxisLimitDelegate(TKey axisObject);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axisObject">The axis object.</param>
        /// <returns></returns>
        public delegate int YAxisConversionDelegate(TValue axisObject);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="axisObject">The axis object.</param>
        /// <returns></returns>
        public delegate TValue YAxisLimitDelegate(TValue axisObject);

        private XAxisLimitDelegate _maxXAxis;
        private YAxisLimitDelegate _maxYAxis;
        private XAxisLimitDelegate _minXAxis;
        private YAxisLimitDelegate _minYAxis;

        private DiagramDataCollection<TKey, TValue> _values = new DiagramDataCollection<TKey, TValue>();
        private XAxisConversionDelegate _xAxisConverter;
        private YAxisConversionDelegate _yAxisConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramLine{TKey, TValue}"/> class.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the x axis converter.
        /// </summary>
        public XAxisConversionDelegate XAxisConverter
        {
            get { return _xAxisConverter; }
            set
            {
                _xAxisConverter = value;
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the y axis converter.
        /// </summary>
        public YAxisConversionDelegate YAxisConverter
        {
            get { return _yAxisConverter; }
            set
            {
                _yAxisConverter = value;
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the minimum x axis.
        /// </summary>
        public XAxisLimitDelegate MinXAxis
        {
            get { return _minXAxis; }
            set
            {
                _minXAxis = value;
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the minimum y axis.
        /// </summary>
        public YAxisLimitDelegate MinYAxis
        {
            get { return _minYAxis; }
            set
            {
                _minYAxis = value;
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the maximum x axis.
        /// </summary>
        public XAxisLimitDelegate MaxXAxis
        {
            get { return _maxXAxis; }
            set
            {
                _maxXAxis = value;
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the maximum y axis.
        /// </summary>
        public YAxisLimitDelegate MaxYAxis
        {
            get { return _maxYAxis; }
            set
            {
                _maxYAxis = value;
                OnChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        public DiagramDataCollection<TKey, TValue> Values
        {
            get { return _values; }
            set
            {
                _values.CollectionChanged -= _values_CollectionChanged;
                SwapProperty(ref _values, value);
                _values.CollectionChanged += _values_CollectionChanged;
            }
        }

        /// <summary>
        /// Renders this instance to a <see cref="Snapshot" />.
        /// </summary>
        /// <returns>
        /// The rendered <see cref="Snapshot" />.
        /// </returns>
        protected override Snapshot Render()
        {
            var bymap = new Snapshot(Size);

            if (_values == null || XAxisConverter == null || YAxisConverter == null || MinXAxis == null ||
                MaxXAxis == null || MinYAxis == null || MaxYAxis == null || !_values.Any())
                return bymap;

            IOrderedEnumerable<KeyValuePair<TKey, TValue>> xOrderedValues = _values.OrderBy(p => XAxisConverter(p.Key));
            IOrderedEnumerable<KeyValuePair<TKey, TValue>> yOrderedValues = _values.OrderBy(p => YAxisConverter(p.Value));

            int minx = XAxisConverter(MinXAxis(xOrderedValues.FirstOrDefault().Key));
            int maxx = XAxisConverter(MaxXAxis(xOrderedValues.LastOrDefault().Key));
            int miny = YAxisConverter(MinYAxis(yOrderedValues.FirstOrDefault().Value));
            int maxy = YAxisConverter(MaxYAxis(yOrderedValues.LastOrDefault().Value));

            float xperpixel = ((float) maxx - minx)/Size.Width;
            float pixelpery = Size.Height/((float) maxy - miny);
            int prefypix = int.MinValue;

            for (int pixelx = 0; pixelx < Size.Width; pixelx++)
            {
                float currentxkey = minx + xperpixel*pixelx;

                KeyValuePair<TKey, TValue> previous =
                    xOrderedValues.LastOrDefault(p => XAxisConverter(p.Key) < currentxkey);
                KeyValuePair<TKey, TValue> current =
                    xOrderedValues.FirstOrDefault(p => (float) XAxisConverter(p.Key) == currentxkey);
                KeyValuePair<TKey, TValue> next = xOrderedValues.FirstOrDefault(p => XAxisConverter(p.Key) > currentxkey);
                bool prevf = false;
                bool nextf = false;
                int prevx = 0;
                int prevy = 0;
                int nextx = 0;
                int nexty = 0;

                if (
                    !EqualityComparer<KeyValuePair<TKey, TValue>>.Default.Equals(current,
                        default(KeyValuePair<TKey, TValue>)))
                {
                    int pixely = Size.Height - 1 - (int) Math.Floor((YAxisConverter(current.Value) - miny)*pixelpery);
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

                int pixy = Size.Height - 1 -
                           (int)
                               Math.Floor(((((nextx - prevx) == 0 ? 0 : ((float) nexty - prevy)/(nextx - prevx))*
                                            (currentxkey - prevx) + prevy) - miny)*pixelpery);

                if (pixy >= 0 && pixy < Size.Height)
                    bymap.SetPixel(pixelx, pixy, true);

                if (prefypix != int.MinValue)
                {
                    if (pixy - prefypix > 1)
                    {
                        for (int py = prefypix + 1; py < pixy; py++)
                            if (py >= 0 && py < Size.Height)
                                bymap.SetPixel(pixelx - 1, py, true);
                    }
                    else if (pixy - prefypix < -1)
                    {
                        for (int py = pixy + 1; py < prefypix; py++)
                            if (py >= 0 && py < Size.Height)
                                bymap.SetPixel(pixelx - 1, py, true);
                    }
                }

                prefypix = pixy;
            }
            return bymap;
        }

        private void _values_CollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e)
        {
            OnChanged(EventArgs.Empty);
        }
    }
}