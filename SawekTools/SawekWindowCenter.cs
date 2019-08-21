using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SawekTools {
    public class SawekWindowCenter {
        private readonly Window _window;
        private Window Owner => _window.Owner;

        public SawekWindowCenter(Window window, Task init = null) {
            _window            = window;
            _window.Visibility = Visibility.Collapsed;

            if (init != null) {
                _window.Loaded += (se, ev) => {
                    Owner.LocationChanged += ReCenter;
                    Owner.PreviewKeyDown  += Escape_close;
                };
                init.ContinueWith((x) => ReCenter());
            }
            else {
                _window.Loaded += (se, ev) => {
                    Owner.LocationChanged += ReCenter;
                    Owner.PreviewKeyDown  += Escape_close;
                    ReCenter();
                };
            }

            _window.Closed += (se, ev) => {
                Owner.LocationChanged -= ReCenter;
                Owner.PreviewKeyDown  -= Escape_close;
            };
            _window.SizeChanged += ReCenter;

            _window.PreviewKeyDown += Escape_close;

            window.Focus();
        }

        private void Escape_close(object se = null, KeyEventArgs ev = null) {
            if (ev == null) return;

            if (ev.Key == Key.Escape)
                _window.Close();
        }

        private void ReCenter(object se = null, EventArgs ev = null) {
            Debug.WriteLine(@"---# Move window to center");
            _window.Dispatcher?.Invoke(() => {
                _window.Left = Owner.Left + ((Owner.Width - _window.Width) / 2);
                _window.Top  = Owner.Top + ((Owner.Height - _window.Height) / 2);

                if (_window.Visibility == Visibility.Collapsed)
                    _window.Visibility = Visibility.Visible;
            });
        }
    }
}