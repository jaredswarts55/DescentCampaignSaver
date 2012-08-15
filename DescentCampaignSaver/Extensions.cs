namespace DescentCampaignSaver
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// The type conversion method attribute.
    /// </summary>
    public class TypeConversionMethodAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// The converter.
        /// </summary>
        private readonly IMappingConverter converter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeConversionMethodAttribute"/> class.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <exception cref="Exception">
        /// Throws exception if the type is not an IMappingConverter
        /// </exception>
        public TypeConversionMethodAttribute(Type t)
        {
            if (typeof(IMappingConverter).IsAssignableFrom(t))
            {
                this.converter = Activator.CreateInstance(t) as IMappingConverter;
            }
            else
            {
                throw new Exception("Invalid IMappingConverter");
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The parse.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The System.Object.
        /// </returns>
        public object Parse(string item)
        {
            return this.converter.ConversionMethod(item);
        }

        #endregion
    }

    /// <summary>
    /// The MappingConverter interface.
    /// </summary>
    public interface IMappingConverter
    {
        #region Public Methods and Operators

        /// <summary>
        /// The conversion method.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The System.Object.
        /// </returns>
        object ConversionMethod(string item);

        #endregion
    }

    /// <summary>
    /// The extensions.
    /// </summary>
    public static class Extensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Uses each ReflectionInfo in the enumerable to generate an instance of the set type populating the data using reflection.
        /// </summary>
        /// <typeparam name="T">
        /// Object Type to create and populate
        /// </typeparam>
        /// <param name="reflectedArray">
        /// IEnumerable of ReflectionInfo Key/Value Set
        /// </param>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// Enumerable of newly created instances of T
        /// </returns>
        public static IEnumerable<T> Into<T>(
            this IEnumerable<ReflectionInfo> reflectedArray, 
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            foreach (ReflectionInfo reflect in reflectedArray)
            {
                yield return reflect.IntoSingle<T>(bindingFlags);
            }
        }

        /// <summary>
        /// Uses ReflectionInfo to generate an instance of the set type populating the data using reflection.
        /// </summary>
        /// <typeparam name="T">
        /// Object Type to create and populate
        /// </typeparam>
        /// <param name="reflectedArray">
        /// ReflectionInfo Key/Value Set
        /// </param>
        /// <param name="bindingFlags">
        /// BindingFlags in GetProperties Command Defaults to Public
        /// </param>
        /// <returns>
        /// New instance of T
        /// </returns>
        public static T IntoSingle<T>(
            this ReflectionInfo reflectedArray, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            PropertyInfo[] props = typeof(T).GetProperties(bindingFlags);
            var empty = Activator.CreateInstance<T>();
            foreach (var reflected in reflectedArray)
            {
                PropertyInfo prop = props.FirstOrDefault(x => x.Name == reflected.Key.ToString().Trim());
                if (prop != null)
                {
                    var mappingConverter =
                        prop.GetCustomAttributes(typeof(TypeConversionMethodAttribute), false).Cast
                            <TypeConversionMethodAttribute>().FirstOrDefault();
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
        /// Prints out a reflected or mapped set of data into a single string for logging or debugging purposes.
        /// </summary>
        /// <param name="reflected">
        /// ReflectionInfo Key/Value Set
        /// </param>
        /// <returns>
        /// Formatted string
        /// </returns>
        public static string IntoSingleLineString(this ReflectionInfo reflected)
        {
            var sb = new StringBuilder();
            foreach (var pair in reflected)
            {
                sb.AppendFormat("{0}: {1}, ", pair.Key, pair.Value);
            }

            return sb.ToString().Trim(' ', ',');
        }

        /// <summary>
        /// Uses each ReflectionInfo in the enumerable to generate a Datatable using the Type name as the table name,
        /// the properties as columns and the set of property values as rows.
        /// </summary>
        /// <param name="reflectedArray">
        /// IEnumerable of ReflectionInfo Key/Value Set
        /// </param>
        /// <returns>
        /// DataTable populated with a row for each object
        /// </returns>
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
        /// Maps an enumerable of paired values to a Key / Value to allow setting properties via reflection in another object.
        /// </summary>
        /// <typeparam name="TReflected">
        /// Enumerable of Enumerable of value pairs
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The Property Name selector type
        /// </typeparam>
        /// <typeparam name="TValue">
        /// The Value selector type
        /// </typeparam>
        /// <param name="toMapArray">
        /// Enumerable of value pairs
        /// </param>
        /// <param name="keySelector">
        /// Selector for Property Name
        /// </param>
        /// <param name="valueSelector">
        /// Selector for Value
        /// </param>
        /// <returns>
        /// Enumerable of ReflectionInfo Key / Value Set
        /// </returns>
        public static IEnumerable<ReflectionInfo> Map<TReflected, TKey, TValue>(
            this IEnumerable<IEnumerable<TReflected>> toMapArray, 
            Func<TReflected, TKey> keySelector, 
            Func<TReflected, TValue> valueSelector)
        {
            return toMapArray.Select(map => MapSingle(map, keySelector, valueSelector));
        }

        /// <summary>
        /// The map csv using header.
        /// </summary>
        /// <param name="csvArray">
        /// The csv array.
        /// </param>
        /// <returns>
        /// The System.Collections.Generic.IEnumerable`1[T -&gt; Mapping.Extensions+ReflectionInfo].
        /// </returns>
        public static IEnumerable<ReflectionInfo> MapCsvUsingHeader(this IEnumerable<IEnumerable<string>> csvArray)
        {
            var csvData = csvArray.ToList();
            var header = csvData.First().ToArray();
            var rInfos = new List<ReflectionInfo>();
            foreach (var item in csvData.Skip(1))
            {
                var itemA = item.ToArray();
                var rInfo = new ReflectionInfo();
                for (int i = 0; i < header.Count(); i++)
                {
                    rInfo[header[i]] = itemA[i];
                }

                rInfos.Add(rInfo);
            }

            return rInfos.AsEnumerable();
        }

        /// <summary>
        /// Maps an enumerable of enumerable of paired values to a Key / Value to allow settings properties via reflection in another object.
        /// </summary>
        /// <typeparam name="TReflected">
        /// Enumerable of Enumerable of value pairs
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The Property Name selector type
        /// </typeparam>
        /// <typeparam name="TValue">
        /// The Value selector type
        /// </typeparam>
        /// <param name="toMap">
        /// Enumerable of value pairs
        /// </param>
        /// <param name="keySelector">
        /// Selector for Property Name
        /// </param>
        /// <param name="valueSelector">
        /// Selector for Value
        /// </param>
        /// <returns>
        /// The Mapping.Extensions+ReflectionInfo.
        /// </returns>
        public static ReflectionInfo MapSingle<TReflected, TKey, TValue>(
            this IEnumerable<TReflected> toMap, 
            Func<TReflected, TKey> keySelector, 
            Func<TReflected, TValue> valueSelector)
        {
            var reflectInfo = new ReflectionInfo { SourceType = typeof(TReflected) };
            foreach (TReflected mapValue in toMap)
            {
                reflectInfo.Add(keySelector(mapValue), valueSelector(mapValue));
            }

            return reflectInfo;
        }

        /// <summary>
        /// Iterates through each property of each object in the enumerable and generates an enumerable of enumerables of Key / Value pairs
        /// to allow setting properties via reflection in another object.
        /// </summary>
        /// <param name="toReflectArray">
        /// Enumerable of objects to reflect
        /// </param>
        /// <param name="bindingFlags">
        /// BindingFlags in GetProperties Command Defaults to Public
        /// </param>
        /// <returns>
        /// Enumerable of ReflectionInfo Key / Value Set
        /// </returns>
        public static IEnumerable<ReflectionInfo> Reflect(
            this IEnumerable<object> toReflectArray, 
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            foreach (object b in toReflectArray)
            {
                yield return b.ReflectSingle(bindingFlags);
            }
        }

        /// <summary>
        /// Iterates through the properties in an object and creates a Key / Value set to allow setting properties via reflection in another object.
        /// </summary>
        /// <param name="toReflect">
        /// Object to reflect
        /// </param>
        /// <param name="bindingFlags">
        /// BindingFlags in GetProperties Command Defaults to Public
        /// </param>
        /// <returns>
        /// ReflectionInfo Key / Value Set
        /// </returns>
        public static ReflectionInfo ReflectSingle(
            this object toReflect, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            PropertyInfo[] props = toReflect.GetType().GetProperties(bindingFlags);
            var rInfo = new ReflectionInfo { SourceType = toReflect.GetType() };
            foreach (PropertyInfo prop in props)
            {
                rInfo.Add(prop.Name, prop.GetValue(toReflect, new object[] { }));
            }

            return rInfo;
        }

        #endregion

        /// <summary>
        /// Set of Key/Value pairs
        /// </summary>
        public class ReflectionInfo : Dictionary<object, object>
        {
            #region Public Properties

            /// <summary>
            /// Gets or sets the source type.
            /// </summary>
            public Type SourceType { get; set; }

            #endregion
        }
    }
}