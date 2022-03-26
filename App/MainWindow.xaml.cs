using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClassLibrary;


namespace App
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // properties

        public ViewData Data { get; set; }
        public VMGrid AppGrid { get; set; } = new VMGrid
        {
            Length = 10,
            LeftEnd = 0.0,
            RightEnd = 1.0,
            Function = VMf.vmdSin
        };


        // MainWindow
        public MainWindow()
        {
            InitializeComponent();
            Data = new ViewData();
            DataContext = this;
            AddTime.IsEnabled = false;
            AddAccuracy.IsEnabled = false;
        }

        // "new" command
        private void New(object sender, RoutedEventArgs args)
        {
            try
            {
                if (display_save_msg())
                {
                    Data.Benchmark.Time.Clear();
                    Data.Benchmark.Accuracy.Clear();
                    Data.VMBenchmarkChanged = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // "open" command
        private void Open(object sender, RoutedEventArgs args)
        {
            try
            {
                if (display_save_msg())
                {
                    // open file dialog box
                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
                    {
                        FileName = "",
                        DefaultExt = ".txt",
                        Filter = "Text documents (.txt)|*.txt"
                    };

                    bool? result = dlg.ShowDialog();

                    if (result == true)
                    {
                        // open document
                        string filename = dlg.FileName;
                        bool status = Data.Load(filename);
                        Data.VMBenchmarkChanged = false;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // "SaveAs" command
        private void SaveAs(object sender, RoutedEventArgs args)
        {
            try
            {
                // save file dialog box
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "Data",
                    DefaultExt = ".txt",
                    Filter = "Text documents (.txt)|*.txt"
                };

                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    // save document
                    string filename = dlg.FileName;
                    bool status = Data.Save(filename);
                    Data.VMBenchmarkChanged = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // add VMTime command
        private void add_vmtime_cmd(object sender, RoutedEventArgs args)
        {
            try
            {
                Data.Benchmark.AddVMTime(AppGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // add VMAccuracy command
        private void add_vmaccuracy_cmd(object sender, RoutedEventArgs args)
        {
            try
            {
                Data.Benchmark.AddVMAccuracy(AppGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // menu click processing
        private void click_menuitem(object sender, RoutedEventArgs args)
        {
            if (Data.FunctionList.SelectedFunc == null)
            {
                MessageBox.Show("Выберите функцию");
                return;
            }

            MenuItem menu_item = (MenuItem)args.Source;
            try
            {
                AppGrid.Function = Data.FunctionList.SelectedFunc.Function;
                switch (menu_item.Header.ToString())
                {
                    case "Add VMTime":
                        Data.Benchmark.AddVMTime(AppGrid);
                        break;
                    case "Add VMAccuracy":
                        Data.Benchmark.AddVMAccuracy(AppGrid);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // displays message box if file was saved
        public bool display_save_msg()
        {
            try
            {
                if (Data.VMBenchmarkChanged)
                {
                    MessageBoxResult choise = MessageBox.Show($"Save changes to file?", "Lab 1", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    switch (choise)
                    {
                        // user aborted operation
                        case MessageBoxResult.Cancel:
                            return false;

                        // saving file
                        case MessageBoxResult.Yes:
                            // default file settings
                            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
                            {
                                FileName = "Data",
                                DefaultExt = ".txt",
                                Filter = "Text documents (.txt)|*.txt"

                            };

                            // show box
                            bool? result = dlg.ShowDialog();

                            if (result == true)
                            {
                                // save document
                                string filename = dlg.FileName;
                                bool status = Data.Save(filename);
                                return status;
                            }
                            else
                            {
                                // user closed dialog
                                return false;
                            }
                        case MessageBoxResult.No:
                            return true;
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        // checks if VMBenchmark was changed
        protected override void OnClosing(System.ComponentModel.CancelEventArgs args)
        {
            if (display_save_msg())
                base.OnClosing(args);
        }

        // text box processing

        private void TextPreview1(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private void TextPreview2(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _) && e.Text != ",")
            {
                e.Handled = true;
            }
        }

        private void TextBoxLengthChanged(object sender, TextChangedEventArgs e)
        {
            bool enable_input = (TextBoxLength.Text.Length != 0) && (TextBoxLeft.Text.Length != 0) && (TextBoxRight.Text.Length != 0);
            AddTime.IsEnabled = enable_input;
            AddAccuracy.IsEnabled = enable_input;
            try
            {
                AppGrid.Length = Convert.ToInt32(TextBoxLength.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBoxLeftChanged(object sender, TextChangedEventArgs e)
        {
            bool enable_input = (TextBoxLength.Text.Length != 0) && (TextBoxLeft.Text.Length != 0) && (TextBoxRight.Text.Length != 0);
            AddTime.IsEnabled = enable_input;
            AddAccuracy.IsEnabled = enable_input;
            try
            {
                AppGrid.LeftEnd = Convert.ToDouble(TextBoxLeft.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBoxRightChanged(object sender, TextChangedEventArgs e)
        {
            bool enable_input = (TextBoxLength.Text.Length != 0) && (TextBoxLeft.Text.Length != 0) && (TextBoxRight.Text.Length != 0);
            AddTime.IsEnabled = enable_input;
            AddAccuracy.IsEnabled = enable_input;
            try
            {
                AppGrid.RightEnd = Convert.ToDouble(TextBoxRight.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
