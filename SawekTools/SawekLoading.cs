using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SawekTools {
    public class SawekLoading : IDisposable {
        private readonly string ID = DateTime.Now.Ticks.ToString();
        private const DispatcherPriority Priority = DispatcherPriority.Normal;
        private Window MainWindow;
        private int max;
        private Grid loadingGrid;
        private ProgressBar loadingBar;
        private int actualProgress;
        private bool infinity;

        public SawekLoading(int max = int.MaxValue, Window window = null) {
            Application.Current.Dispatcher?.Invoke(Priority, new Action(() => {
                MainWindow = window ?? Application.Current.MainWindow;
                this.max = max > 0 ? max - 1 : 10;
                this.infinity = max < 0;
                this.actualProgress = 0;

                CreateLoading();
            }));

            Timer aTimer = new Timer(80);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e) {
            try {
                ((Timer)sender).Enabled = false;
                Application.Current.Dispatcher?.Invoke(Priority, new Action(() => {
                    if (loadingGrid == null) return;

                    Debug.WriteLine(@"Show loading screen");
                    loadingGrid.Visibility = Visibility.Visible;
                }));
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }

        public void SetMax(int max) {
            try {
                if (MainWindow == null) return;

                Application.Current.Dispatcher?.Invoke(Priority, new Action(() => {
                    if (loadingBar != null) {
                        loadingBar.Maximum = max;
                        loadingBar.Value = 0;
                    }

                    this.max = max;
                    this.actualProgress = 0;
                }));
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Next() {
            try {
                if (MainWindow == null) return;

                actualProgress++;
                Application.Current.Dispatcher?.Invoke(Priority, new Action(() => SetValue(actualProgress)));
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }

        public void SetValue(int value) {
            try {
                if (MainWindow == null) return;

                Application.Current.Dispatcher?.Invoke(Priority, new Action(() => {
                    try {
                        if (value < max)
                            loadingBar.Value = value;
                        else
                            Close();
                    }
                    catch {
                        //ignore
                    }
                }));
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }

        private void CreateLoading() {
            try {
                if (MainWindow.FindInWindow<Grid>().Any(x => x.Name == "MainGrid" + ID)) return;

                Grid newGrid = new Grid {
                    Background = new SolidColorBrush(Color.FromArgb(102, 0, 0, 0)),
                    Name = "MainGrid" + ID,
                    Visibility = Visibility.Hidden
                };

                Grid innerGrid = new Grid {
                    Background = new ImageBrush() {
                        Opacity = 0.7d
                    },
                    Height = 30d,
                    Width = MainWindow.Width / 3
                };

                ProgressBar progress = new ProgressBar {
                    Maximum = max,
                    Name = @"PROGRESSBAR_" + ID,
                    Margin = new Thickness(3),
                    IsIndeterminate = infinity
                };

                innerGrid.Children.Add(progress);
                newGrid.Children.Add(innerGrid);

                Debug.WriteLine(@"Add loading controls to window");
                MainWindow.FindInWindow<Grid>().FirstOrDefault()?.Children.Add(newGrid);

                loadingGrid = newGrid;
                loadingBar = progress;
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Close() {
            try {
                if (MainWindow == null) return;

                Debug.WriteLine(@"Closing loading screen");
                Application.Current.Dispatcher?.Invoke(Priority, new Action(() => {
                    MainWindow.FindInWindow<Grid>().FirstOrDefault()?.Children.Remove(loadingGrid);

                    max = 0;
                    actualProgress = 0;
                    MainWindow = null;
                    loadingGrid = null;
                    loadingBar = null;
                }));
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Dispose() => Close();
    }
}