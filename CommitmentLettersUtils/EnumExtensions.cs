using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DrLogy.CommitmentLettersUtils
{
    public static class EnumExtensions
    {

        /// <summary>
        /// Gets the string of an DescriptionAttribute of an Enum.
        /// </summary>
        /// <param name="value">The Enum value for which the description is needed.</param>
        /// <returns>If a DescriptionAttribute is set it return the content of it.
        /// Otherwise just the raw name as string.</returns>
        public static string Description(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string description = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(description);
            DescriptionAttribute[] attributes =
               (DescriptionAttribute[])
             fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }

            return description;
        }
    }

}
