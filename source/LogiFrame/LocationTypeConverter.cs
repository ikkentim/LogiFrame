// LocationTypeConverter.cs
// 
// LogiFrame rendering library.
// Copyright (C) 2013 Tim Potze
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
using System.ComponentModel;
using System.Globalization;

namespace LogiFrame
{
    /// <summary>
    ///     Provides a type converter to convert LogiFrame.Location objects to and from various other representations.
    /// </summary>
    public class LocationTypeConverter : ExpandableObjectConverter
    {
        /// <summary>
        ///     Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Object" /> that represents the converted value.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
        /// <param name="culture">
        ///     A <see cref="T:System.Globalization.CultureInfo" />. If null is passed, the current culture is
        ///     assumed.
        /// </param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
        /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to. </param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType" /> parameter is null. </exception>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (destinationType == typeof (string) && value is Location)
                return (value as Location).ToString();

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}