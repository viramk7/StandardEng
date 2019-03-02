using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardEng.Common
{
    public class Enums
    {
        public enum NotifyType
        {
            /// <summary>
            /// Success Enum Value setting.
            /// </summary>
            [Display(Name = "Success")]
            [Description("Success")]
            Success = 1,

            /// <summary>
            /// Error Enum Value setting.
            /// </summary>
            [Display(Name = "Error")]
            [Description("Error")]
            Error = 0
        }

        public enum ReceiptType
        {
            To,
            Cc,
            Bcc,
            Attachment
        }
    }

    public static class EnumExtension
    {
        /// <summary>
        /// The get description.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetDescription(this Enum element)
        {
            var type = element.GetType();
            var memberInfo = type.GetMember(Convert.ToString(element));
            if (memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return Convert.ToString(element);
        }
    }
}
