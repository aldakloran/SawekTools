using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SawekTools {
    public static class SawekCollectionExtensions {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action) {
            foreach (var objectInQueue in collection)
                action(objectInQueue); 
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> entities, string propertyName) {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
                return entities;

            var propertyInfo = entities.First().GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return entities.OrderBy(e => propertyInfo.GetValue(e, null));
        }

        public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> entities, string propertyName) {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
                return entities;

            var propertyInfo = entities.First().GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return entities.OrderByDescending(e => propertyInfo.GetValue(e, null));
        }
    }
}