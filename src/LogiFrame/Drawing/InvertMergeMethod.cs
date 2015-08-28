using System;
using System.Drawing;

namespace LogiFrame.Drawing
{
    public class InvertMergeMethod : IMergeMethod
    {
        public void Merge(MonochromeBitmap source, MonochromeBitmap destination, Point location)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            var maxX = Math.Min(location.X + source.Width, destination.Width);
            var maxY = Math.Min(location.Y + source.Height, destination.Height);

            for (var x = Math.Max(location.X, 0); x < maxX; x++)
                for (var y = Math.Max(location.Y, 0); y < maxY; y++)
                    if (source[x - location.X, y - location.Y])
                        destination[x, y] = !destination[x, y];
        }
    }
}