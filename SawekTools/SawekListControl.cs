using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SawekTools {
    public static class SawekListControl {
        private const DispatcherPriority TaskPriority = DispatcherPriority.Background;
        private static readonly object LockMe = new object();
        private static Task StopTask;
        public static bool IsWorking => OperationsList.Any(x => x.Status == DispatcherOperationStatus.Executing || x.Status == DispatcherOperationStatus.Pending);

        public static void Cancel() {
            StopTask = Task.Factory.StartNew(() => OperationsList.ForEach(x => x?.Abort()));
        }

        public static IList<string> Wyswietl<T>(List<T> listToView, ObservableCollection<T> currentList) {
            return ScalListy(listToView, currentList, true);
        }

        public static IList<string> WyswietlAll<T>(List<T> listToView, ObservableCollection<T> currentList) {
            return ScalListy(listToView, currentList, false);
        }

        public static DispatcherOperation LastOperation => OperationsList.LastOrDefault();

        private static readonly List<DispatcherOperation> OperationsList = new List<DispatcherOperation>();
        private static IList<string> propertyDiff;

        private static IList<string> ScalListy<T>(IReadOnlyCollection<T> listToView, ICollection<T> currentList, bool oneByOne) {
            StopTask?.Wait();
            LastOperation?.Wait(); // w8 for last operation
            OperationsList.Where(x => x == null || x.Status == DispatcherOperationStatus.Aborted || x.Status == DispatcherOperationStatus.Completed).ToList().ForEach(x => OperationsList.Remove(x));

            lock (LockMe) {
                propertyDiff = new List<string>();
            }
            
            if (listToView == null) { // remove all items and skip other pocedures
                OperationsList?.Add(System.Windows.Application.Current.Dispatcher.BeginInvoke(TaskPriority, new Action(currentList.Clear)));
                lock (LockMe) {
                    return propertyDiff;
                }
            }

            var toRemove = currentList.Except(listToView.OfType<T>(), new CustomObjectEqueal<T>()).ToList(); // find items to remove
            var toAdd   = listToView.Except(currentList.OfType<T>(), new CustomObjectEqueal<T>()).ToList(); // find items to add

            if (oneByOne) {
                foreach (T item in toRemove)
                    OperationsList?.Add(System.Windows.Application.Current.Dispatcher?.BeginInvoke(TaskPriority, new Action(() => {
                        currentList.Remove(item);
                    })));

                foreach (T item in toAdd)
                    OperationsList?.Add(System.Windows.Application.Current.Dispatcher?.BeginInvoke(TaskPriority, new Action(() => {
                        currentList.Add(item);
                    })));
            }
            else {
                OperationsList?.Add(System.Windows.Application.Current.Dispatcher?.BeginInvoke(TaskPriority, new Action(() => {
                    foreach (T item in toRemove)
                        currentList.Remove(item);

                    foreach (T item in toAdd)
                        currentList.Add(item);
                })));
            }

            lock (LockMe) {
                if (!propertyDiff.Any() && toAdd.Count > 0)
                    propertyDiff.Add("Add");
                if (!propertyDiff.Any() && toRemove.Count > 0)
                    propertyDiff.Add("Remove");

                return propertyDiff;
            }
        }

        private class CustomObjectEqueal<T> : EqualityComparer<T> {
            public override bool Equals(T x, T y) {
                if (x is IEquatable<T> || y is IEquatable<T>) 
                    return x?.Equals(y) ?? false;

                foreach (PropertyInfo field in typeof(T).GetProperties()) {
                    var obj1 = field.GetValue(x);
                    var obj2 = field.GetValue(y);

                    var additionalCondition = (obj1?.GetType().GetInterfaces().Contains(typeof(IComparable)) ?? false) || (obj2?.GetType().GetInterfaces().Contains(typeof(IComparable)) ?? false);

                    if (obj1 is ICollection || obj2 is ICollection) {
                        if (obj1 is IStructuralEquatable || obj2 is IStructuralEquatable) { // handling for simple arrays np. int[]
                            if (((IStructuralEquatable) obj1).StructuralEquals((IStructuralEquatable) obj2)) {
                                if (!field.Name.ToUpper().StartsWith("ID")) {
                                    lock (LockMe) {
                                        propertyDiff.Add(field.Name);
                                    }
                                }

                                return false;
                            }
                        }
                        else {            // not handled arrays | complex arrays
                            return false; // complex types must implement IEquatable interface
                        }
                    }

                    if (additionalCondition && !Equals(obj1, obj2)) { // check only those objects that fit conditions to automatic compare
                        if (!field.Name.ToUpper().StartsWith("ID")) {
                            lock (LockMe) {
                                propertyDiff.Add(field.Name);
                            }
                        }

                        return false;
                    }
                }

                return true;
            }

            public override int GetHashCode(T obj) {
                if (obj is IEquatable<T> equatable)
                    return equatable.GetHashCode();

                return typeof(T).GetProperties().FirstOrDefault(x => x.Name.ToUpper().StartsWith("ID"))?.GetHashCode() ?? -1;
            }
        }
    }
}