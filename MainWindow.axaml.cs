using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NodeRunner
{
    public partial class MainWindow : Window
    {
        private Process? _homeProcess; // Made nullable
        private int _pendingBalance = 0;
        private string? _scriptsFolderPath; // Made nullable

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            ResultScreen = this.FindControl<TextBox>("ResultScreen");
            HomeButtonText = this.FindControl<TextBlock>("HomeButtonText");
            PendingBalance = this.FindControl<TextBox>("PendingBalance");
        }

        private async void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_scriptsFolderPath))
            {
                ResultScreen.Text += "\nERROR: No folder selected.";
                return;
            }

            if (_homeProcess == null || (_homeProcess != null && _homeProcess.HasExited))
            {
                try
                {
                    _homeProcess = StartScript("start.bat");
                    HomeButtonText.Text = "Stop Nodes";
                }
                catch (Exception ex)
                {
                    ResultScreen.Text += "\nERROR: " + ex.Message;
                }
            }
            else
            {
                try
                {
                    // Run the process termination in a separate task to avoid blocking the UI thread
                    await Task.Run(() =>
                    {
                        if (_homeProcess != null)
                        {
                            KillProcessTree(_homeProcess);
                            _homeProcess.WaitForExit(); // Ensure the process has exited
                        }
                    });

                    _homeProcess = null;
                    HomeButtonText.Text = "Run Nodes";
                    ResultScreen.Text += "\nScript stopped.";
                }
                catch (Exception ex)
                {
                    ResultScreen.Text += "\nERROR: " + ex.Message;
                }
            }
        }

        private Process StartScript(string scriptFileName)
        {
            if (_scriptsFolderPath == null)
            {
                throw new InvalidOperationException("Scripts folder path is not set.");
            }

            var processInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"{_scriptsFolderPath}\\{scriptFileName}\"",
                WorkingDirectory = _scriptsFolderPath,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = new Process
            {
                StartInfo = processInfo
            };

            process.OutputDataReceived += (sender, args) => Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                if (args.Data != null)
                {
                    ResultScreen.Text += "\n" + args.Data;
                    this.FindControl<ScrollViewer>("ResultScreenScrollViewer").ScrollToEnd();
                }
            });

            process.ErrorDataReceived += (sender, args) => Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                if (args.Data != null)
                {
                    ResultScreen.Text += "\nERROR: " + args.Data;
                    this.FindControl<ScrollViewer>("ResultScreenScrollViewer").ScrollToEnd();
                }
            });

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return process;
        }

        private void NodesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Reset the pending balance to zero before running the script
                _pendingBalance = 0;
                PendingBalance.Text = "Pending Balance: 0";

                RunScriptAsync("check.bat");
            }
            catch (Exception ex)
            {
                ResultScreen.Text += "\nERROR: " + ex.Message;
            }
        }

        private void BalancesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RunScriptAsync("claim.bat");
                _pendingBalance = 0;
                PendingBalance.Text = "Pending Balance: 0";
            }
            catch (Exception ex)
            {
                ResultScreen.Text += "\nERROR: " + ex.Message;
            }
        }

        private async void RunScriptAsync(string scriptFileName)
        {
            try
            {
                await Task.Run(() =>
                {
                    var processInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c \"{_scriptsFolderPath}\\{scriptFileName}\"",
                        WorkingDirectory = _scriptsFolderPath,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    var process = new Process
                    {
                        StartInfo = processInfo
                    };

                    process.OutputDataReceived += (sender, args) => Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                    {
                        if (args.Data != null)
                        {
                            ResultScreen.Text += "\n" + args.Data;
                            UpdatePendingBalance(args.Data);
                            this.FindControl<ScrollViewer>("ResultScreenScrollViewer").ScrollToEnd();
                        }
                    });

                    process.ErrorDataReceived += (sender, args) => Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                    {
                        if (args.Data != null)
                        {
                            ResultScreen.Text += "\nERROR: " + args.Data;
                            this.FindControl<ScrollViewer>("ResultScreenScrollViewer").ScrollToEnd();
                        }
                    });

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                });
            }
            catch (Exception ex)
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    ResultScreen.Text += "\nERROR: " + ex.Message;
                });
            }
        }

        private void UpdatePendingBalance(string output)
        {
            var match = Regex.Match(output, @"Rewards to claim for NodeKey \(\d+\): (\d{3})");
            if (match.Success)
            {
                _pendingBalance += int.Parse(match.Groups[1].Value);
                PendingBalance.Text = $"Pending Balance: {_pendingBalance}";
            }
        }

        private void KillProcessTree(Process process)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = $"/T /F /PID {process.Id}",
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using (var killProcess = new Process { StartInfo = startInfo })
            {
                killProcess.Start();
                killProcess.WaitForExit();
            }
        }

        private async void SelectScriptsFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog();
            var result = await dialog.ShowAsync(this);

            if (!string.IsNullOrWhiteSpace(result))
            {
                // Save the selected folder path to a variable or a file
                // This is your "Scripts" folder
                _scriptsFolderPath = result;
            }
        }
    }
}
