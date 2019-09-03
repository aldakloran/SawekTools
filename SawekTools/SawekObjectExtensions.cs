using System.Collections.Generic;

namespace SawekTools {
    public static class SawekObjectExtensions {
        public static ICollection<string> CopyObjectToObject<T>(this T TargetObject, T SourceObject) where T : class {
            var diffList = new List<string>();

            foreach (var field in typeof(T).GetProperties()) {
                if (!field.CanWrite) continue;

                var targetValue = field.GetValue(TargetObject);
                var sourceValue = field.GetValue(SourceObject);

                if (targetValue == null && sourceValue == null) continue;
                if (targetValue != null && targetValue.Equals(sourceValue)) continue;

                field.SetValue(TargetObject, sourceValue);
                diffList.Add(field.Name);
            }

            return diffList;
        }
    }
}