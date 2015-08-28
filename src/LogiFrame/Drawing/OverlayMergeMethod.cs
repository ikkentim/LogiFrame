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
    public class OverlayMergeMethod : IMergeMethod
    {
        public void Merge(MonochromeBitmap source, MonochromeBitmap destination, Point location)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            var maxX = Math.Min(location.X + source.Width, destination.Width);
            var maxY = Math.Min(location.Y + source.Height, destination.Height);

            for (var x = Math.Max(location.X, 0); x < maxX; x++)
                for (var y = Math.Max(location.Y, 0); y < maxY; y++)
                {
                    if (source[x - location.X, y - location.Y])
                    {
                        destination[x, y] = true;
                        for (var ox = -1; ox <= 1; ox++)
                            for (var oy = -1; oy <= 1; oy++)
                                if (!(ox == 0 && oy == 0))
                                {
                                    var dx = x + ox;
                                    var dy = y + oy;
                                    if (dx < 0 || dy < 0 || dx >= destination.Width || dy >= destination.Height)
                                        continue;

                                    var sx = dx - location.X;
                                    var sy = dy - location.Y;

                                    if (source[sx, sy] || !destination[dx, dy])
                                        continue;

                                    destination[dx, dy] = false;
                                }
                    }
                }
        }
    }
}