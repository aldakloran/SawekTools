using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SawekTools {
    public class NaturalSort : IComparer, IComparer<string> {
        #region Variables

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int StrCmpLogicalW(string x, string y);

        private readonly ListSortDirection _direction;
        private readonly List<string> _SortMemberPath;

        #endregion

        #region Constructors

        public NaturalSort() {
            _direction = ListSortDirection.Ascending;
            _SortMemberPath = new List<string>();
        }

        public NaturalSort(ListSortDirection direction) {
            _direction = direction;
            _SortMemberPath = new List<string>();
        }

        public NaturalSort(ListSortDirection direction, params string[] SortMemberPath) {
            _direction = direction;
            _SortMemberPath = SortMemberPath.ToList();
        }

        #endregion

        #region Methods

        public int Compare(string x, string y) {
            switch (_direction) {
                case ListSortDirection.Ascending:
                    return StrCmpLogicalW(x, y);
                case ListSortDirection.Descending:
                    return StrCmpLogicalW(y, x);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public int Compare(object x, object y) {
            if (!_SortMemberPath.Any())
                return Compare(x?.ToString(), y?.ToString());

            string val1 = string.Empty;
            string val2 = string.Empty;

            foreach (var sort_path in _SortMemberPath) {
                foreach (PropertyInfo property in x.GetType().GetProperties()) {
                    if (property.Name == sort_path) {
                        val1 += " " + property.GetValue(x)?.ToString();
                        val2 += " " + property.GetValue(y)?.ToString();
                    }
                }
            }

            return Compare(val1.Trim(), val2.Trim());
        }

        #endregion
    }
}