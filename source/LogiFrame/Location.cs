using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogiFrame
{
    /// <summary>
    /// Represents a location of a component
    /// </summary>
    class Location
    {
        #region Properties
        private int x;

        /// <summary>
        /// The x-coordinate of the LogiFrame.Location
        /// </summary>
        public int X 
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        private int y;
        /// <summary>
        /// The y-coordinate of the LogiFrame.Location
        /// </summary>
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        #endregion


    }
}
