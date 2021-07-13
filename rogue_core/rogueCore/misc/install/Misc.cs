using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;

namespace rogue_core.rogueCore.install
{
    class Misc
    {
    }
    public abstract class MyEnumeration : IComparable
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        protected MyEnumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : MyEnumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as MyEnumeration;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object other) => Id.CompareTo(((MyEnumeration)other).Id);

        // Other utility methods ...
    }
    public abstract class EnumBaseType<T> where T : EnumBaseType<T>
    {
        protected static List<T> enumValues = new List<T>();
        public readonly long Key;
        public readonly string Value;

        public EnumBaseType(long key, string value) 
        {
            Key = key;
            Value = value;
            enumValues.Add((T)this);
        }

        protected static ReadOnlyCollection<T> GetBaseValues()
        {
            return enumValues.AsReadOnly();
        }

        protected static T GetBaseByKey(long key)
        {
            foreach (T t in enumValues)
            {
                if (t.Key == key) return t;
            }
            return null;
        }
        internal static T GetByString(string key)
        {
            foreach (T t in enumValues)
            {
                if (t.Value == key) return t;
            }
            return null;
        }
        public override string ToString()
        {
            return Value;
        }
    }

}
