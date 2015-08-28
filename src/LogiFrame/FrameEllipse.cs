namespace LogiFrame
{
    public class FrameEllipse : FrameControl
    {
        #region Overrides of FrameControl

        protected override void OnPaint(FramePaintEventArgs e)
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