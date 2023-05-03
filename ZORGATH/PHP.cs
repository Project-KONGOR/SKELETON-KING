namespace ZORGATH;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Custom PHP serialization library that focuses on performance while fixing some of the issues that PhpSerializerNET has:
///     - inability to serialize floats.
///     - inability to serialize ICollections (requires ToList() or similar workaround).
/// </summary>
public class PHP
{
    // Cache of serializer metadata, organized by type.
    private static readonly ConcurrentDictionary<Type, ISerializer> _serializersByType = new();

    private static ISerializer GetSerializerByType(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            // Unwrap nullable types.
            type = Nullable.GetUnderlyingType(type)!;
        }

        if (_serializersByType.TryGetValue(type, out var result)) return result;

        // Create and cache serializer for the type.
        ISerializer serializer;
        if (type == typeof(int)) { serializer = new IntSerializer(); }

        else if (type == typeof(string)) { serializer = new StringSerializer(); }
        else if (type == typeof(bool)) { serializer = new BoolSerializer(); }
        else if (type == typeof(double)) { serializer = new DoubleSerializer(); }
        else if (type == typeof(float)) { serializer = new FloatSerializer(); }
        else if (type == typeof(long)) { serializer = new LongSerializer(); }
        else if (type.IsEnum)
        {
            serializer = new EnumSerializer();
        }

        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            Type keyType = type.GetGenericArguments()[0];
            Type valueType = type.GetGenericArguments()[1];

            // Specialize for common types.
            if (keyType == typeof(string))
            {
                if (valueType == typeof(string)) serializer = new DictionaryStringStringSerializer();
                else serializer = new DictionaryStringObjectSerializer();
            }
            else { serializer = new DictionaryObjectObjectSerializer(); }
        }

        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            Type keyType = type.GetGenericArguments()[0];

            // Specialize for common types.
            if (keyType == typeof(string)) serializer = new ListStringSerializer();
            else serializer = new ListObjectSerializer();
        }
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(HashSet<>))
        {
            Type keyType = type.GetGenericArguments()[0];

            // Specialize for common types.
            if (keyType == typeof(string)) serializer = new HashSetStringSerializer();
            else serializer = new EnumerableObjectSerializer();
        }
        // ICollection
        else if (type.IsGenericType && type.GetInterfaces().Any(x => x == typeof(ICollection)))
        {
            Type keyType = type.GetGenericArguments()[0];
            if (keyType == typeof(string)) serializer = new CollectionStringSerializer();
            else serializer = new CollectionObjectSerializer();
        }
        // IEnumerable
        else if (type.IsGenericType && type.GetInterfaces().Any(x => x == typeof(IEnumerable)))
        {
            Type keyType = type.GetGenericArguments()[0];
            if (keyType == typeof(string)) serializer = new EnumerableStringSerializer();
            else serializer = new EnumerableObjectSerializer();
        }
        else if (type.IsGenericType)
        {
            throw new Exception("unsupported generic type");
        }
        else if (type.IsArray)
        {
            if (type.GetElementType() == typeof(string)) serializer = new ArrayStringSerializer();
            else serializer = new ArrayObjectSerializer();
        }

        else
        {
            List<Property> properties = new();
            TypeInfo typeInfo = (TypeInfo)type;

            while (typeInfo != typeof(object))
            {
                foreach (FieldInfo fieldInfo in typeInfo!.DeclaredFields)
                {
                    // Ignore static fields.
                    if (fieldInfo.Attributes.HasFlag(FieldAttributes.Static))
                    {
                        continue;
                    }
                    
                    string? name = null;
                    bool isInteger = false;
                    long key = 0;

                    // Skip backing fields.
                    if (fieldInfo.GetCustomAttribute<CompilerGeneratedAttribute>() != null) continue;

                    foreach (Attribute attr in fieldInfo.GetCustomAttributes(typeof(PhpPropertyAttribute)))
                    {
                        PhpPropertyAttribute phpPropertyAttribute = (PhpPropertyAttribute)attr;
                        name = phpPropertyAttribute.Name;
                        isInteger = phpPropertyAttribute.IsInteger;
                        key = phpPropertyAttribute.Key;
                        break;
                    }

                    properties.Add(new Property(null, fieldInfo, fieldInfo.Name, name, isInteger, key));
                }

                foreach (PropertyInfo propertyInfo in typeInfo.DeclaredProperties)
                {
                    // Ignore static properties.
                    if (propertyInfo.GetGetMethod(nonPublic: true)!.IsStatic)
                    {
                        continue;
                    }

                    string? name = null;
                    bool isInteger = false;
                    long key = 0;

                    foreach (Attribute attr in propertyInfo.GetCustomAttributes(typeof(PhpPropertyAttribute)))
                    {
                        PhpPropertyAttribute phpPropertyAttribute = (PhpPropertyAttribute)attr;
                        name = phpPropertyAttribute.Name;
                        isInteger = phpPropertyAttribute.IsInteger;
                        key = phpPropertyAttribute.Key;
                        break;
                    }

                    properties.Add(new Property(propertyInfo, null, propertyInfo.Name, name, isInteger, key));
                }

                typeInfo = (TypeInfo)typeInfo.BaseType!;
            }

            serializer = new ObjectSerializer(properties);
        }

        _serializersByType.TryAdd(type, serializer);
        return serializer;
    }

    private static void AppendString(StringBuilder sb, string value)
    {
        sb.Append('s');
        sb.Append(':');
        sb.Append(Encoding.UTF8.GetByteCount(value));
        sb.Append(':');
        sb.Append('"');
        sb.Append(value);
        sb.Append('"');
        sb.Append(';');
    }

    private static void AppendInt(StringBuilder sb, int value)
    {
        sb.Append('i');
        sb.Append(':');
        sb.Append(value);
        sb.Append(';');
    }

    private static void AppendLong(StringBuilder sb, long value)
    {
        sb.Append('i');
        sb.Append(':');
        sb.Append(value);
        sb.Append(';');
    }

    private static void AppendDouble(StringBuilder sb, double value)
    {
        sb.Append('d');
        sb.Append(':');
        sb.Append(value);
        sb.Append(';');
    }

    private static void AppendFloat(StringBuilder sb, double value)
    {
        sb.Append('d');
        sb.Append(':');
        sb.Append(value);
        sb.Append(';');
    }

    private static void AppendBool(StringBuilder sb, bool value)
    {
        sb.Append('b');
        sb.Append(':');
        sb.Append(value ? 1 : 0);
        sb.Append(';');
    }

    public static string Serialize(object value)
    {
        return Serialize(value, validate: false);
    }

    public static string Serialize(object value, bool validate)
    {
        StringBuilder sb = new();
        GetSerializerByType(value.GetType()).Serialize(sb, value);
        string treatment = sb.ToString();

        if (validate)
        {
            string control = PhpSerialization.Serialize(value);
            if (treatment != control)
            {
                return control;
            }
        }
        return treatment;
    }

    public interface ISerializer
    {
        void Serialize(StringBuilder sb, object data);
    }

    // Individual type-specific serializers follow.
    private class IntSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data) { AppendInt(sb, (int)data); }
    }

    private class StringSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data) { AppendString(sb, (string)data); }
    }

    private class BoolSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data) { AppendBool(sb, (bool)data); }
    }

    private class DoubleSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data) { AppendDouble(sb, (double)data); }
    }

    private class FloatSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data) { AppendFloat(sb, (float)data); }
    }

    private class LongSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data) { AppendLong(sb, (long)data); }
    }
    private class EnumSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data) {
            IConvertible e = (IConvertible) data;
            AppendInt(sb, e.ToInt32(null));
        }
    }

    private class ListObjectSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data)
        {
            IList value = (IList)data;
            sb.Append('a');
            sb.Append(':');
            sb.Append(value.Count);
            sb.Append(':');
            sb.Append('{');

            for (int i = 0; i < value.Count; ++i)
            {
                AppendInt(sb, i);
                object? entryValue = value[i];

                if (entryValue == null)
                {
                    sb.Append('N');
                    sb.Append(';');

                    continue;
                }

                GetSerializerByType(entryValue.GetType()).Serialize(sb, entryValue);
            }

            sb.Append('}');
        }
    }

    private class ListStringSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data)
        {
            List<string> value = (List<string>)data;
            sb.Append('a');
            sb.Append(':');
            sb.Append(value.Count);
            sb.Append(':');
            sb.Append('{');

            for (int i = 0; i < value.Count; ++i)
            {
                AppendInt(sb, i);
                AppendString(sb, value[i]);
            }

            sb.Append('}');
        }
    }

    private class HashSetStringSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data)
        {
            HashSet<string> value = (HashSet<string>)data;
            sb.Append('a');
            sb.Append(':');
            sb.Append(value.Count);
            sb.Append(':');
            sb.Append('{');

            int i = 0;
            foreach (string entryValue in value)
            {
                AppendInt(sb, i);
                AppendString(sb, entryValue);
                ++i;
            }

            sb.Append('}');
        }
    }

    private class ArrayStringSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data)
        {
            string[] value = (string[])data;
            sb.Append('a');
            sb.Append(':');
            sb.Append(value.Length);
            sb.Append(':');
            sb.Append('{');

            int i = 0;
            foreach (string entryValue in value)
            {
                AppendInt(sb, i);
                AppendString(sb, entryValue);
                ++i;
            }

            sb.Append('}');
        }
    }

    private class ArrayObjectSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data)
        {
            object[] value = (object[])data;
            sb.Append('a');
            sb.Append(':');
            sb.Append(value.Length);
            sb.Append(':');
            sb.Append('{');

            int i = 0;
            foreach (object? entryValue in value)
            {
                AppendInt(sb, i);
                if (entryValue == null)
                {
                    sb.Append('N');
                    sb.Append(';');

                    continue;
                }

                GetSerializerByType(entryValue.GetType()).Serialize(sb, entryValue);
                ++i;
            }

            sb.Append('}');
        }
    }

    private class CollectionStringSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data)
        {
            ICollection<string> value = (ICollection<string>)data;
            sb.Append('a');
            sb.Append(':');
            sb.Append(value.Count);
            sb.Append(':');
            sb.Append('{');

            int i = 0;
            foreach (string entryValue in value)
            {
                AppendInt(sb, i);
                AppendString(sb, entryValue);
                ++i;
            }

            sb.Append('}');
        }
    }

    private class CollectionObjectSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data)
        {
            ICollection value = (ICollection)data;
            sb.Append('a');
            sb.Append(':');
            sb.Append(value.Count);
            sb.Append(':');
            sb.Append('{');

            int i = 0;
            foreach (object? entryValue in value)
            {
                AppendInt(sb, i);
                if (entryValue == null)
                {
                    sb.Append('N');
                    sb.Append(';');

                    continue;
                }

                GetSerializerByType(entryValue.GetType()).Serialize(sb, entryValue);
                ++i;
            }

            sb.Append('}');
        }
    }

    private class EnumerableStringSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data)
        {
            IEnumerable<string> value = (IEnumerable<string>)data;
            sb.Append('a');
            sb.Append(':');
            sb.Append(value.Count());
            sb.Append(':');
            sb.Append('{');

            int i = 0;
            foreach (object? entryValue in value)
            {
                AppendInt(sb, i);
                if (entryValue == null)
                {
                    sb.Append('N');
                    sb.Append(';');

                    continue;
                }

                GetSerializerByType(entryValue.GetType()).Serialize(sb, entryValue);
                ++i;
            }

            sb.Append('}');
        }
    }

    private class EnumerableObjectSerializer : ISerializer
    {
        private static int Count(IEnumerable enumerable)
        {
            if (enumerable is ICollection col)
            {
                return col.Count;
            }
            int result = 0;
            IEnumerator enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext()) result++;
            return result;
        }
        public void Serialize(StringBuilder sb, object data)
        {
            IEnumerable value = (IEnumerable)data;
            sb.Append('a');
            sb.Append(':');
            sb.Append(Count(value));
            sb.Append(':');
            sb.Append('{');

            int i = 0;
            foreach (object? entryValue in value)
            {
                AppendInt(sb, i);
                if (entryValue == null)
                {
                    sb.Append('N');
                    sb.Append(';');

                    continue;
                }

                GetSerializerByType(entryValue.GetType()).Serialize(sb, entryValue);
                ++i;
            }

            sb.Append('}');
        }
    }

    private class DictionaryStringStringSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data)
        {
            Dictionary<string, string> value = (Dictionary<string, string>)data;
            sb.Append('a');
            sb.Append(':');
            sb.Append(value.Count);
            sb.Append(':');
            sb.Append('{');

            foreach (KeyValuePair<string, string> entry in value)
            {
                AppendString(sb, entry.Key);

                if (entry.Value == null)
                {
                    sb.Append('N');
                    sb.Append(';');

                    continue;
                }

                AppendString(sb, entry.Value);
            }

            sb.Append('}');
        }
    }

    private class DictionaryStringObjectSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data)
        {
            IDictionary value = (IDictionary)data;
            sb.Append('a');
            sb.Append(':');
            sb.Append(value.Count);
            sb.Append(':');
            sb.Append('{');

            foreach (object? entry in value)
            {
                DictionaryEntry dictionaryEntry = (DictionaryEntry)entry;
                AppendString(sb, (string)dictionaryEntry.Key);

                object? entryValue = dictionaryEntry.Value;

                if (entryValue == null)
                {
                    sb.Append('N');
                    sb.Append(';');

                    continue;
                }

                GetSerializerByType(entryValue.GetType()).Serialize(sb, entryValue);
            }

            sb.Append('}');
        }
    }

    private class DictionaryObjectObjectSerializer : ISerializer
    {
        public void Serialize(StringBuilder sb, object data)
        {
            IDictionary value = (IDictionary)data;
            sb.Append('a');
            sb.Append(':');
            sb.Append(value.Count);
            sb.Append(':');
            sb.Append('{');

            foreach (object? entry in value)
            {
                DictionaryEntry dictionaryEntry = (DictionaryEntry)entry;
                object entryKey = dictionaryEntry.Key;
                GetSerializerByType(entryKey.GetType()).Serialize(sb, entryKey);

                object? entryValue = dictionaryEntry.Value;

                if (entryValue == null)
                {
                    sb.Append('N');
                    sb.Append(';');

                    continue;
                }

                GetSerializerByType(entryValue.GetType()).Serialize(sb, entryValue);
            }

            sb.Append('}');
        }
    }

    private class ObjectSerializer : ISerializer
    {
        private readonly List<Property> Properties;

        public ObjectSerializer(List<Property> properties) => Properties = properties;

        public void Serialize(StringBuilder sb, object data)
        {
            int numNonNullProperties = 0;
            foreach (Property property in Properties)
            {
                var propertyInfo = property.PropertyInfo;
                object? value = propertyInfo != null ? propertyInfo.GetValue(data) : property.FieldInfo!.GetValue(data);
                if (value != null) ++numNonNullProperties;
            }

            sb.Append("a:");
            sb.Append(numNonNullProperties);
            sb.Append(":{");

            foreach (Property property in Properties)
            {
                var propertyInfo = property.PropertyInfo;
                object? value = propertyInfo != null ? propertyInfo.GetValue(data) : property.FieldInfo!.GetValue(data);
                if (value == null) continue;

                if (property.CustomName != null) { AppendString(sb, property.CustomName); }
                else if (property.IsInteger)
                {
                    sb.Append('i');
                    sb.Append(':');
                    sb.Append(property.Key);
                    sb.Append(';');
                }
                else { AppendString(sb, property.Name); }

                GetSerializerByType(value.GetType()).Serialize(sb, value);
            }

            sb.Append('}');
        }
    }

    // Metadata used by ObjectSerializer which uses reflection to walk the object.
    public readonly struct Property
    {
        public readonly PropertyInfo? PropertyInfo;
        public readonly FieldInfo? FieldInfo;
        public readonly string Name;
        public readonly string? CustomName;
        public readonly bool IsInteger;
        public readonly long Key;

        public Property(PropertyInfo? propertyInfo, FieldInfo? fieldInfo, string name, string? customName, bool isInteger, long key)
        {
            PropertyInfo = propertyInfo;
            FieldInfo = fieldInfo;
            Name = name;
            CustomName = customName;
            IsInteger = isInteger;
            Key = key;
        }
    }
}
