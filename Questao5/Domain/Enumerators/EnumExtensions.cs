using System;
using System.Linq;
using System.Reflection;
using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Enumerators
{
    public static class EnumExtensions
    {
        public static char GetEnumCharValue(this Enum enumValue)
        {
            var type = enumValue.GetType();
            var memberInfo = type.GetMember(enumValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(EnumCharValueAttribute), false);
                if (attrs.Length > 0)
                {
                    return ((EnumCharValueAttribute)attrs[0]).CharValue;
                }
            }
            throw new InvalidOperationException("EnumCharValueAttribute not found on enum.");
        }
    }
}
