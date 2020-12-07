using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prototype.Data
{
    public static class EnumHelper<T> where T : Enum
    {
        public static T GetByAbreviation(String abv)
        {
            return Enum.GetValues(typeof(T))
                    .OfType<T>()
                    .FirstOrDefault<T>(x => {
                        DisplayAttribute attributes = DataUtility.GetEnumAttributes<DisplayAttribute>(x);
                        return attributes.ShortName.Equals(abv);
                    });
        }

        public static T GetByName(String Name)
        {
            return Enum.GetValues(typeof(T))
                    .OfType<T>()
                    .FirstOrDefault<T>(x => {
                        DisplayAttribute attributes = DataUtility.GetEnumAttributes<DisplayAttribute>(x);
                        return attributes.Description.Equals(Name);
                    });
        }

        public static String GetName(T tEnum)
        {
            DisplayAttribute attribute = DataUtility.GetEnumAttributes<DisplayAttribute>(tEnum);
            return attribute.Name;
        }

        public static String GetAbbreviation(T tEnum)
        {
            DisplayAttribute attribute = DataUtility.GetEnumAttributes<DisplayAttribute>(tEnum);
            return attribute.ShortName;
        }

        public static List<T> GetEnumList()
        {
            return ((T[])Enum.GetValues(typeof(T))).ToList();
        }
    }
}
