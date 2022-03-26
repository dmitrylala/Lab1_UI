using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows;
using ClassLibrary;

namespace App
{
    public class ViewData : INotifyPropertyChanged
    {

        // events
        public event PropertyChangedEventHandler PropertyChanged;

        private void Collection_changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            VMBenchmarkChanged = true;
        }

        // constructor
        public ViewData()
        {
            Benchmark = new VMBenchmark();
            FunctionList = new();
            VMBenchmarkChanged = false;
            Benchmark.Time.CollectionChanged += Collection_changed;
            Benchmark.Accuracy.CollectionChanged += Collection_changed;
        }

        /*
            --------------------------
                    Properties
            --------------------------
        */

        private bool _VMBenchmarkChanged;
        public bool VMBenchmarkChanged 
        { 
            get { return _VMBenchmarkChanged; }
            set
            {
                if (value != _VMBenchmarkChanged)
                {
                    _VMBenchmarkChanged = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VMBenchmarkChanged)));
                }
            }    
        }

        public VMBenchmark Benchmark { get; set; }

        public VMfConv FunctionList { get; set; }



        /*
            --------------------------
                    Methods
            --------------------------
        */

        // add methods

        public void AddVMTime(VMGrid grid)
        {
            try
            {
                Benchmark.AddVMTime(grid);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error occured: {error.Message}.", "Add error", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }

        public void AddVMAccuracy(VMGrid grid)
        {
            try
            {
                Benchmark.AddVMAccuracy(grid);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error occured: {error.Message}.", "Add error", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }


        // save and load methods

        public bool Save(string filename)
        {
            try
            {
                StreamWriter writer = new StreamWriter(filename, false);

                try
                {
                    // time measures collection
                    writer.WriteLine(Benchmark.Time.Count);
                    foreach (VMTime item in Benchmark.Time)
                    {
                        // writing grid info
                        writer.WriteLine(item.Grid.Length);
                        writer.WriteLine($"{item.Grid.LeftEnd}");
                        writer.WriteLine($"{item.Grid.RightEnd}");
                        writer.WriteLine((int)item.Grid.Function);

                        // writing time info
                        writer.WriteLine($"{item.Time_VML_HA}");
                        writer.WriteLine($"{item.Time_VML_LA}");
                        writer.WriteLine($"{item.Time_VML_EP}");
                        writer.WriteLine($"{item.Coef_LA_HA}");
                        writer.WriteLine($"{item.Coef_EP_HA}");
                    }

                    // accuracy measures collection
                    writer.WriteLine(Benchmark.Accuracy.Count);
                    foreach (VMAccuracy item in Benchmark.Accuracy)
                    {
                        // writing grid info
                        writer.WriteLine(item.Grid.Length);
                        writer.WriteLine($"{item.Grid.LeftEnd}");
                        writer.WriteLine($"{item.Grid.RightEnd}");
                        writer.WriteLine((int)item.Grid.Function);

                        // writing accuracy info
                        writer.WriteLine($"{item.MaxDiff}");
                        writer.WriteLine($"{item.MaxDiffArgument}");
                        writer.WriteLine($"{item.Value_VML_HA}");
                        writer.WriteLine($"{item.Value_VML_LA}");
                        writer.WriteLine($"{item.Value_VML_EP}");
                    }
                }
                catch (Exception ex)
                {
                    Benchmark.Time.Clear();
                    Benchmark.Accuracy.Clear();
                    MessageBox.Show($"Unable to save file: {ex.Message}.", "Save error", MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                    writer.Close();
                    return false;
                }
                finally
                {
                    writer.Close();
                }
            }
            catch (Exception error)
            {
                Benchmark.Time.Clear();
                Benchmark.Accuracy.Clear();
                MessageBox.Show($"Unable to save file: {error.Message}.", "Save error", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public bool Load(string filename)
        {
            try
            {
                StreamReader reader = new StreamReader(filename);

                try
                {
                    Benchmark.Time.Clear();
                    Benchmark.Accuracy.Clear();

                    // reading time collection
                    int count_time = Int32.Parse(reader.ReadLine());
                    for (int i = 0; i < count_time; ++i)
                    {
                        VMGrid grid = new VMGrid
                        {
                            Length = Int32.Parse(reader.ReadLine()),
                            LeftEnd = double.Parse(reader.ReadLine()),
                            RightEnd = double.Parse(reader.ReadLine()),
                            Function = (VMf)int.Parse(reader.ReadLine())
                        };

                        VMTime time = new VMTime
                        {
                            Grid = grid,
                            Time_VML_HA = double.Parse(reader.ReadLine()),
                            Time_VML_LA = double.Parse(reader.ReadLine()),
                            Time_VML_EP = double.Parse(reader.ReadLine()),
                            Coef_LA_HA = double.Parse(reader.ReadLine()),
                            Coef_EP_HA = double.Parse(reader.ReadLine())
                        };

                        Benchmark.Time.Add(time);
                    }

                    // reading accuracy collection
                    int count_accuracy = Int32.Parse(reader.ReadLine());
                    for (int i = 0; i < count_accuracy; ++i)
                    {
                        VMGrid grid = new VMGrid
                        {
                            Length = Int32.Parse(reader.ReadLine()),
                            LeftEnd = double.Parse(reader.ReadLine()),
                            RightEnd = double.Parse(reader.ReadLine()),
                            Function = (VMf)int.Parse(reader.ReadLine())
                        };

                        VMAccuracy accuracy = new VMAccuracy
                        {
                            Grid = grid,
                            MaxDiff = double.Parse(reader.ReadLine()),
                            MaxDiffArgument = double.Parse(reader.ReadLine()),
                            Value_VML_HA = double.Parse(reader.ReadLine()),
                            Value_VML_LA = double.Parse(reader.ReadLine()),
                            Value_VML_EP = double.Parse(reader.ReadLine()),
                        };

                        Benchmark.Accuracy.Add(accuracy);
                    }
                }
                catch (Exception ex)
                {
                    Benchmark.Time.Clear();
                    Benchmark.Accuracy.Clear();
                    MessageBox.Show($"Unable to load file: {ex.Message}.", "Load error", MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                    reader.Close();
                    return false;
                }
                finally
                {
                    reader.Close();
                }
            } 
            catch (Exception error)
            {
                Benchmark.Time.Clear();
                Benchmark.Accuracy.Clear();
                MessageBox.Show($"Unable to load file: {error.Message}.", "Load error", MessageBoxButton.OK, 
                    MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
