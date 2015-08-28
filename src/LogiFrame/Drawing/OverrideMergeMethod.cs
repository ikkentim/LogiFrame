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
using System.Drawing;

namespace LogiFrame.Drawing
{
    public class OverrideMergeMethod : IMergeMethod
    {
        public void Merge(MonochromeBitmap source, MonochromeBitmap destination, Point location)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            var maxX = Math.Min(location.X + source.Width, destination.Width);
            var maxY = Math.Min(location.Y + source.Height, destination.Height);

            for (var x = Math.Max(location.X, 0); x < maxX; x++)
                for (var y = Math.Max(location.Y, 0); y < maxY; y++)
                    destination[x, y] = source[x - location.X, y - location.Y];
        }
    }
}