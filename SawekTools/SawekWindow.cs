using System;
using System.Diagnostics;
using System.Windows;

namespace SawekTools {
    public class SawekWindow {
        public static Window MainWindow => Application.Current.Dispatcher?.Invoke(() => Application.Current.MainWindow);
        public static WindowCollection AllWindows => Application.Current.Dispatcher?.Invoke(() => Application.Current.Windows);

        public static MessageBoxResult Message(string message, string title, MessageBoxButton buttons, MessageBoxImage image) {
            return Application.Current.Dispatcher.Invoke(() => MessageBox.Show(MainWindow, message, title, buttons, image));
        }

        public static MessageBoxResult Message(string message, string title, MessageBoxButton buttons) {
            return Application.Current.Dispatcher.Invoke(() => MessageBox.Show(MainWindow, message, title, buttons));
        }

        public static MessageBoxResult Message(string message, string title) {
            return Application.Current.Dispatcher.Invoke(() => MessageBox.Show(MainWindow, message, title));
        }

        public static MessageBoxResult Message(string message) {
            return Application.Current.Dispatcher.Invoke(() => MessageBox.Show(MainWindow, message));
        }

        public static MessageBoxResult Message(Window window, string message, string title, MessageBoxButton button, MessageBoxImage image) {
            return Application.Current.Dispatcher.Invoke(() => MessageBox.Show(window, message, title, button, image));
        }

        public static MessageBoxResult Message(Window window, string message, string title, MessageBoxButton button) {
            return Application.Current.Dispatcher.Invoke(() => MessageBox.Show(window, message, title, button));
        }

        public static MessageBoxResult Message(Window window, string message, string title) {
            return Application.Current.Dispatcher.Invoke(() => MessageBox.Show(window, message, title));
        }

        public static MessageBoxResult Message(Window window, string message) {
            return Application.Current.Dispatcher.Invoke(() => MessageBox.Show(window, message));
        }

        public static T OpenOne<T>() where T : Window {
            Debug.WriteLine($@"-----------------=============#### Open window: {typeof(T).Name} ####=============-----------------");
            foreach (object item in AllWindows) {
                if (!(item is T one)) continue;

                one.Focus();
                one.Visibility = Visibility.Visible;
                return one;
            }

            if (!(Activator.CreateInstance(typeof(T), true) is T okno)) return null;

            okno.Owner = MainWindow;
            okno.Show();
            okno.Focus();
            okno.Closed += (s, e) => okno.Owner.Focus();
            return okno;
        }

        public static T OpenOne<T>(params object[] args) where T : Window {
            Debug.WriteLine($@"-----------------=============#### Open window: {typeof(T).Name} ####=============-----------------");
            foreach (object item in AllWindows) {
                if (!(item is T one)) continue;

                one.Focus();
                one.Visibility = Visibility.Visible;
                return one;
            }

            if (!(Activator.CreateInstance(typeof(T), args) is T okno)) return null;

            okno.Owner = MainWindow;
            okno.Show();
            okno.Focus();
            okno.Closed += (s, e) => okno.Owner.Focus();
            return okno;

        }

        public static T OpenOne<T>(Window owner) where T : Window {
            Debug.WriteLine($@"-----------------=============#### Open window: {typeof(T).Name} ####=============-----------------");
            foreach (object item in AllWindows) {
                if (!(item is T one)) continue;

                one.Focus();
                one.Visibility = Visibility.Visible;
                return one;
            }

            if (!(Activator.CreateInstance(typeof(T), true) is T okno)) return null;

            okno.Owner = owner;
            okno.Show();
            okno.Focus();
            okno.Closed += (s, e) => okno.Owner.Focus();
            return okno;
        }

        public static T OpenOne<T>(Window owner, params object[] args) where T : Window {
            Debug.WriteLine($@"-----------------=============#### Open window: {typeof(T).Name} ####=============-----------------");
            foreach (object item in AllWindows) {
                if (!(item is T one)) continue;

                one.Focus();
                one.Visibility = Visibility.Visible;
                return one;
            }

            if (!(Activator.CreateInstance(typeof(T), args) is T okno)) return null;

            okno.Owner = owner;
            okno.Show();
            okno.Focus();
            okno.Closed += (s, e) => okno.Owner.Focus();
            return okno;

        }

        public static void CloseAll<T>() where T : Window {
            foreach (object item in AllWindows)
                if (item is T okno)
                    okno.Close();
        }
    }
}