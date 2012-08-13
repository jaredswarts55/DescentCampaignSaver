using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mapping
{
    public class TypeConversionMethodAttribute : Attribute
    {
        IMappingConverter converter = null;
        public TypeConversionMethodAttribute(Type t)
        {
            if (typeof(IMappingConverter).IsAssignableFrom(t))
            {
                converter = Activator.CreateInstance(t) as IMappingConverter;
            }
            else
                throw new Exception("Invalid IMappingConverter");
        }
        public object Parse(string item)
        {
            return converter.ConversionMethod(item);
        }
    }

    public interface IMappingConverter
    {
        object ConversionMethod(string item);
    }

    public static class Extensions
    {
        /// <summary>
        /// Maps an enumerable of enumerable of paired values to a Key / Value to allow settings properties via reflection in another object.
        /// </summary>
        /// <typeparam name="TReflected">Enumerable of Enumerable of value pairs</typeparam>
        /// <typeparam name="TKey">The Property Name selector type</typeparam>
        /// <typeparam name="TValue">The Value selector type</typeparam>
        /// <param name="toMap">Enumerable of value pairs</param>
        /// <param name="keySelector">Selector for Property Name</param>
        /// <param name="valueSelector">Selector for Value</param>
        /// <returns></returns>
        public static ReflectionInfo MapSingle<TReflected, TKey, TValue>(this IEnumerable<TReflected> toMap,
                                                                         Func<TReflected, TKey> keySelector,
                                                                         Func<TReflected, TValue> valueSelector)
        {
            var reflectInfo = new ReflectionInfo { SourceType = typeof(TReflected) };
            foreach (TReflected mapValue in toMap)
                reflectInfo.Add(keySelector(mapValue), valueSelector(mapValue));
            return reflectInfo;
        }

        /// <summary>
        /// Maps an enumerable of paired values to a Key / Value to allow setting properties via reflection in another object.
        /// </summary>
        /// <typeparam name="TReflected">Enumerable of value pairs</typeparam>
        /// <typeparam name="TKey">The Property Name selector type</typeparam>
        /// <typeparam name="TValue">The Value selector type</typeparam>
        /// <param name="toMapArray">Enumerable of value pairs</param>
        /// <param name="keySelector">Selector for Property Name</param>
        /// <param name="valueSelector">Selector for Value</param>
        /// <returns>Enumerable of ReflectionInfo Key / Value Set</returns>
        public static IEnumerable<ReflectionInfo> Map<TReflected, TKey, TValue>(
            this IEnumerable<IEnumerable<TReflected>> toMapArray, Func<TReflected, TKey> keySelector,
            Func<TReflected, TValue> valueSelector)
        {
            foreach (var map in toMapArray)
                yield return MapSingle(map, keySelector, valueSelector);
        }

        public static IEnumerable<ReflectionInfo> MapCsvUsingHeader(this IEnumerable<IEnumerable<string>> csvArray)
        {
            var csvData = csvArray.ToList();
            var header = csvData.First().ToArray();
            List<ReflectionInfo> rInfos = new List<ReflectionInfo>();
            foreach (var item in csvData.Skip(1))
            {
                var itemA = item.ToArray();
                var rInfo = new ReflectionInfo();
                for (int i = 0; i < header.Count();i++ )
                {
                    rInfo[header[i]] = itemA[i];
                }
                rInfos.Add(rInfo);
            }
            return rInfos.AsEnumerable();
        }

        /// <summary>
        /// Iterates through the properties in an object and creates a Key / Value set to allow setting properties via reflection in another object.
        /// </summary>
        /// <param name="toReflect">Object to reflect</param>
        /// <param name="bindingFlags">BindingFlags in GetProperties Command Defaults to Public</param>
        /// <returns>ReflectionInfo Key / Value Set</returns>
        public static ReflectionInfo ReflectSingle(this object toReflect,
                                                   BindingFlags bindingFlags =
                                                       (BindingFlags.Public | BindingFlags.Instance))
        {
            PropertyInfo[] props = toReflect.GetType().GetProperties(bindingFlags);
            var rInfo = new ReflectionInfo();
            rInfo.SourceType = toReflect.GetType();
            foreach (PropertyInfo prop in props)
                rInfo.Add(prop.Name, prop.GetValue(toReflect, new Object[] { }));
            return rInfo;
        }

        /// <summary>
        /// Iterates through each property of each object in the enumerable and generates an enumerable of enumerables of Key / Value pairs
        /// to allow setting properties via reflection in another object.
        /// </summary>
        /// <param name="toReflectArray">Enumerable of objects to reflect</param>
        /// <param name="bindingFlags">BindingFlags in GetProperties Command Defaults to Public</param>
        /// <returns>Enumerable of ReflectionInfo Key / Value Set</returns>
        public static IEnumerable<ReflectionInfo> Reflect(this IEnumerable<object> toReflectArray,
                                                          BindingFlags bindingFlags =
                                                              (BindingFlags.Public | BindingFlags.Instance))
        {
            foreach (object b in toReflectArray)
            {
                yield return b.ReflectSingle(bindingFlags);
            }
        }

        /// <summary>
        /// Uses ReflectionInfo to generate an instance of the set type populating the data using reflection.
        /// </summary>
        /// <typeparam name="T">Object Type to create and populate</typeparam>
        /// <param name="reflectedArray">ReflectionInfo Key/Value Set</param>
        /// <param name="bindingFlags">BindingFlags in GetProperties Command Defaults to Public</param>
        /// <returns>New instance of T</returns>
        public static T IntoSingle<T>(this ReflectionInfo reflectedArray,
                                      BindingFlags bindingFlags = (BindingFlags.Public | BindingFlags.Instance))
        {
            PropertyInfo[] props = typeof(T).GetProperties(bindingFlags);
            var empty = Activator.CreateInstance<T>();
            foreach (var reflected in reflectedArray)
            {
                PropertyInfo prop = props.FirstOrDefault(x => x.Name == reflected.Key.ToString().Trim());
                if (prop != null)
                {
                    var mappingConverter = prop.GetCustomAttributes(typeof(TypeConversionMethodAttribute), false).Cast<TypeConversionMethodAttribute>().FirstOrDefault();
                    if (mappingConverter != null)
                    {
                        prop.SetValue(empty, mappingConverter.Parse(reflected.Value.ToString()), null);
                    }
                    else
                    {
                        TypeConverter converter = TypeDescriptor.GetConverter(prop.PropertyType);
                        prop.SetValue(empty, converter.ConvertFrom(reflected.Value), null);
                    }
                }
            }
            return empty;
        }

        /// <summary>
        /// Uses each ReflectionInfo in the enumerable to generate an instance of the set type populating the data using reflection.
        /// </summary>
        /// <typeparam name="T">Object Type to create and populate</typeparam>
        /// <param name="reflectedArray">IEnumerable of ReflectionInfo Key/Value Set</param>
        /// <param name="bindingFlags"> </param>
        /// <returns>Enumerable of newly created instances of T</returns>
        public static IEnumerable<T> Into<T>(this IEnumerable<ReflectionInfo> reflectedArray,
                                             BindingFlags bindingFlags = (BindingFlags.Public | BindingFlags.Instance))
        {
            foreach (ReflectionInfo reflect in reflectedArray)
            {
                yield return reflect.IntoSingle<T>(bindingFlags);
            }
        }

        /// <summary>
        /// Uses each ReflectionInfo in the enumerable to generate a Datatable using the Type name as the table name,
        /// the properties as columns and the set of property values as rows.
        /// </summary>
        /// <param name="reflectedArray">IEnumerable of ReflectionInfo Key/Value Set</param>
        /// <returns>DataTable populated with a row for each object</returns>
        public static DataTable IntoTable(this IEnumerable<ReflectionInfo> reflectedArray)
        {
            var dt = new DataTable(reflectedArray.First().SourceType.Name);
            foreach (var item in reflectedArray.First())
            {
                var dc = new DataColumn(item.Key.ToString(), item.Value.GetType());
                dt.Columns.Add(dc);
            }
            foreach (ReflectionInfo rows in reflectedArray)
            {
                DataRow dr = dt.NewRow();
                foreach (var row in rows)
                {
                    dr[row.Key.ToString()] = row.Value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Prints out a reflected or mapped set of data into a single string for logging or debugging purposes.
        /// </summary>
        /// <param name="reflected">ReflectionInfo Key/Value Set</param>
        /// <returns>Formatted string</returns>
        public static String IntoSingleLineString(this ReflectionInfo reflected)
        {
            var sb = new StringBuilder();
            foreach (var pair in reflected)
            {
                sb.AppendFormat("{0}: {1}, ", pair.Key, pair.Value);
            }
            return sb.ToString().Trim(' ', ',');
        }

        #region Nested type: ReflectionInfo

        /// <summary>
        /// Set of Key/Value pairs
        /// </summary>
        public class ReflectionInfo : Dictionary<object, object>
        {
            public Type SourceType { get; set; }
        }

        #endregion
    }
}