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

namespace LogiFrame
{
    /// <summary>
    ///     Represents an ellipse.
    /// </summary>
    public class LCDEllipse : LCDControl
    {
        #region Overrides of LCDControl

        /// <summary>
        ///     Raises the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LogiFrame.LCDPaintEventArgs" /> instance containing the event data.</param>
        protected override void OnPaint(LCDPaintEventArgs e)
        {
            var widthRadius = (Width - 1)/2;
            var heightRadius = (Height - 1)/2;
            var centerX = Width/2;
            var centerY = Height/2;
            var widthRadiusSq = widthRadius*widthRadius;
            var heightRadiusSq = heightRadius*heightRadius;

            for (int x = 0, y = heightRadius, sigma = 2*heightRadiusSq + widthRadiusSq*(1 - 2*heightRadius);
                heightRadiusSq*x <= widthRadiusSq*y;
                x++)
            {
                e.Bitmap[centerX + x, centerY + y] = true;
                e.Bitmap[centerX - x, centerY + y] = true;
                e.Bitmap[centerX + x, centerY - y] = true;
                e.Bitmap[centerX - x, centerY - y] = true;
                if (sigma >= 0)
                {
                    sigma += 4*widthRadiusSq*(1 - y);
                    y--;
                }
                sigma += heightRadiusSq*((4*x) + 6);
            }

            for (int x = widthRadius, y = 0, sigma = 2*widthRadiusSq + heightRadiusSq*(1 - 2*widthRadius);
                widthRadiusSq*y <= heightRadiusSq*x;
                y++)
            {
                e.Bitmap[centerX + x, centerY + y] = true;
                e.Bitmap[centerX - x, centerY + y] = true;
                e.Bitmap[centerX + x, centerY - y] = true;
                e.Bitmap[centerX - x, centerY - y] = true;
                if (sigma >= 0)
                {
                    sigma += 4*heightRadiusSq*(1 - x);
                    x--;
                }
                sigma += widthRadiusSq*((4*y) + 6);
            }

            base.OnPaint(e);
        }

        #endregion
    }
}