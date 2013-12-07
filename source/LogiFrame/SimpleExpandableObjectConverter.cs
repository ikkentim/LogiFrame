using System;
using System.ComponentModel;
using System.Globalization;

namespace LogiFrame
{
    /// <summary>
    /// Provides a type converter to convert various objects to and from various other representations.
    /// </summary>
    public class SimpleExpandableObjectConverter : ExpandableObjectConverter
    {
        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted value.
        /// </returns>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
        /// <param name="culture">
        /// A <see cref="T:System.Globalization.CultureInfo" />. If null is passed, the current culture is
        /// assumed.
        /// </param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert. </param>
        /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to. </param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType" /> parameter is null. </exception>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            return destinationType == typeof (string) && value != null
                ? value.ToString()
                : base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
