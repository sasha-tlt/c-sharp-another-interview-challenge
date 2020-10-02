namespace CSharp.Test.TestConsoleApp.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class ObjectFieldComparer
    {
        public static bool Compare(object first, object second, Type type)
        {
            return Compare(first, second, type, new List<KeyValuePair<int, int>>());
        }

        private static bool Compare(object first, object second, Type type, List<KeyValuePair<int, int>> compareHashes)
        {
            if (first == null && second == null)
            {
                return true;
            }

            if (first == null || second == null)
            {
                return false;
            }

            if (compareHashes.Any(h => (h.Key == first.GetHashCode() && h.Value == second.GetHashCode()) || (h.Key == second.GetHashCode() && h.Value == first.GetHashCode())))
            {
                return true;
            }

            compareHashes.Add(new KeyValuePair<int, int>(first.GetHashCode(), second.GetHashCode()));
            foreach (var field in type.GetFields().Where(f => f.IsPublic))
            {
                if (field.FieldType == typeof(string))
                {
                    if (string.Compare((string)field.GetValue(first), (string)field.GetValue(second), StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        return false;
                    }
                }
                else if (field.FieldType.IsClass)
                {
                    if (!Compare(field.GetValue(first), field.GetValue(second), field.FieldType, compareHashes))
                    {
                        return false;
                    }
                }
                else if (!field.GetValue(first).Equals(field.GetValue(second)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
