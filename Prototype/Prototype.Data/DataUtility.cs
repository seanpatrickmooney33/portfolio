using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Prototype.Data
{
    public static class DataUtility
    {

        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static readonly Dictionary<Type, List<PropertyInfo>> DateTimePropertiesLookUpTable = new Dictionary<Type, List<PropertyInfo>>();

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return ((attributes != null) && (attributes.Length > 0)) ? attributes[0].Description : value.ToString();
        } // GetEnumDescription

        public static T GetEnumAttributes<T>(Enum value) where T : Attribute
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            T[] attributes = fieldInfo.GetCustomAttributes(typeof(T), false) as T[];

            return ((attributes != null) && (attributes.Length > 0)) ? attributes[0] : null;
        }

        public static T GetAttributes<T>(Type value) where T : Attribute
        {
            T[] attributes = value.GetCustomAttributes(typeof(T), false) as T[];
            return ((attributes != null) && (attributes.Length > 0)) ? attributes[0] : null;
        }

        public static T GetEnum<T>(Object value, Type customAttributeType, String customAttributeField)
        {
            Type type = typeof(T);
            if (type.IsEnum == false) { throw new InvalidOperationException(String.Format("{0} is not an enum type.", type.ToString())); }

            var list = type.GetFields().Where(x => GetCustomAttributes(x, customAttributeType, customAttributeField, value)).ToList();

            return (list.Count > 0) ? (T)list[0].GetRawConstantValue() : default(T);
        }

        public static void ConvertAllDateTimeToUTC<T>(T obj) where T : class
        {
            if (obj != null)
            {
                UpdateAllDateTime(obj, true);
            }
        }

        public static void MarkAllDateTimeToUTC<T>(T obj) where T : class
        {
            if (obj != null)
            {
                UpdateAllDateTime(obj, false);
            }
        }

        private static Boolean GetCustomAttributes(ICustomAttributeProvider source, Type customAttributeType, String customAttributeField, Object value)
        {
            var attributes = source.GetCustomAttributes(customAttributeType, false) as Attribute[];
            return ((attributes != null) && (attributes.Length > 0)) ?
                    attributes[0].GetType().GetProperty(customAttributeField).GetValue(attributes[0], null).Equals(value) :
                    false;
        }

        private static List<PropertyInfo> GetPropertyInfoList(Type objType)
        {
            List<PropertyInfo> propertyList = null;
            PropertyInfo[] properties = null;

            if (DateTimePropertiesLookUpTable.ContainsKey(objType) == false)
            {
                properties = objType.GetProperties();
                propertyList = new List<PropertyInfo>();
                DateTimePropertiesLookUpTable.Add(objType, propertyList);
                foreach (PropertyInfo property in properties)
                {
                    if ((property.PropertyType == typeof(DateTime)) ||
                         (property.PropertyType == typeof(Nullable<DateTime>)))
                    {
                        propertyList.Add(property);
                    }
                }
            }
            else
            {
                propertyList = DateTimePropertiesLookUpTable[objType];
            }

            return propertyList;
        }

        private static void UpdateAllDateTime<T>(T obj, Boolean convertToUTC)
        {
            Type objType = obj.GetType();
            List<PropertyInfo> propertyList = GetPropertyInfoList(objType);

            foreach (PropertyInfo property in propertyList)
            {
                if (property.PropertyType == typeof(DateTime))
                {
                    DateTime date = (DateTime)property.GetValue(obj, null);
                    date = (convertToUTC == true) ? date.ToUniversalTime() : DateTime.SpecifyKind(date, DateTimeKind.Utc);
                    property.SetValue(obj, date, null);
                }
                else if (property.PropertyType == typeof(Nullable<DateTime>))
                {
                    DateTime? date = property.GetValue(obj, null) as DateTime?;
                    if (date.HasValue == true)
                    {
                        date = (convertToUTC == true) ? date.Value.ToUniversalTime() : DateTime.SpecifyKind(date.Value, DateTimeKind.Utc);
                        property.SetValue(obj, date, null);
                    }
                }
            }

        }
    }
}
