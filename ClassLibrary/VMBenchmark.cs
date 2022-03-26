using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ClassLibrary
{
    public class VMBenchmark : INotifyPropertyChanged
    {
        // external function using MKL
        [DllImport("..\\..\\..\\..\\..\\x64\\Debug\\DynamicLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int calculate_function(int length, double[] vector, int function_code,
            double[] res_HA, double[] res_LA, double[] res_EP, double[] time);

        // events handling
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void collection_changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinTimeCoefs)));
        }

        /*
            --------------------------
                    Properties
            --------------------------
        */

        // contains results of computation time
        public ObservableCollection<VMTime> Time { get; set; }

        private VMTime selected_time;
        public VMTime SelectedTime
        {
            get { return selected_time; }
            set
            {
                selected_time = value;
                OnPropertyChanged(nameof(SelectedTime));
            }
        }

        // contains results of computation accuracy
        public ObservableCollection<VMAccuracy> Accuracy { get; set; }

        private VMAccuracy selected_accuracy;
        public VMAccuracy SelectedAccuracy
        {
            get { return selected_accuracy; }
            set
            {
                selected_accuracy = value;
                OnPropertyChanged(nameof(SelectedAccuracy));
            }
        }

        // getting minimum coefs of time comparisons
        // first - Coef_LA_HA, second - Coef_EP_HA
        public string MinTimeCoefs
        {
            get
            {
                if (Time.Count == 0) return "";
                double min_la_ha = Time.Min(time => time.Coef_LA_HA);
                double min_ep_ha = Time.Min(time => time.Coef_EP_HA);
                return $"Min LA / HA coef: {min_la_ha}, min EP / HA coef: {min_ep_ha}";
            }
        }

        // constructor
        public VMBenchmark()
        {
            Time = new ObservableCollection<VMTime>();
            Accuracy = new ObservableCollection<VMAccuracy>();
            Time.CollectionChanged += collection_changed;
        }


        /*
            --------------------------
                    Methods
            --------------------------
        */

        // add methods
        
        // adding new VMTime object to collection
        public void AddVMTime(VMGrid grid)
        {
            // creating vector of grid points
            double[] vector = new double[grid.Length];
            for (int i = 0; i < grid.Length; ++i)
                vector[i] = grid.LeftEnd + (i * grid.Dx);

            // vectors for results
            double[] res_HA = new double[grid.Length];
            double[] res_LA = new double[grid.Length];
            double[] res_EP = new double[grid.Length];
            double[] time = new double[3];          // time: HA, LA, EP

            // calling funtion to calculate results
            int status = calculate_function(grid.Length, vector, (int)grid.Function, res_HA, res_LA, res_EP, time);
            if (status != 0)
                throw new InvalidCastException($"MKL function exited with code: {status}");

            // creating new VMTime object
            VMTime item = new VMTime
            {
                Grid = grid,
                Time_VML_HA = time[0],
                Time_VML_LA = time[1],
                Time_VML_EP = time[2],
                Coef_LA_HA = time[1] / time[0],
                Coef_EP_HA = time[2] / time[0]
            };

            Time.Add(item);
            OnPropertyChanged(nameof(MinTimeCoefs));
        }

        // adding new VMAccuracy object to collection
        public void AddVMAccuracy(VMGrid grid)
        {
            // creating vector of grid points
            double[] vector = new double[grid.Length];
            for (int i = 0; i < grid.Length; ++i)
                vector[i] = grid.LeftEnd + i * grid.Dx;

            // vectors for results
            double[] res_HA = new double[grid.Length];
            double[] res_LA = new double[grid.Length];
            double[] res_EP = new double[grid.Length];
            double[] time = new double[3];

            // calling funtion to calculate results
            int status = calculate_function(grid.Length, vector, (int)grid.Function, res_HA, res_LA, res_EP, time);
            if (status != 0)
                throw new InvalidCastException($"MKL function exited with code: {status}");

            // creating new VMTime object
            VMAccuracy item = new VMAccuracy();
            item.Grid = grid;
            item.MaxDiff = 0;
            for (int i = 0; i < grid.Length; ++i)
                if (Math.Abs(res_HA[i] - res_EP[i]) > item.MaxDiff)
                {
                    item.MaxDiff = Math.Abs(res_HA[i] - res_EP[i]);
                    item.MaxDiffArgument = vector[i];
                    item.Value_VML_HA = res_HA[i];
                    item.Value_VML_LA = res_LA[i];
                    item.Value_VML_EP = res_EP[i];
                }

            Accuracy.Add(item);
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < Accuracy.Count; ++i)
            {
                result += $"Test #{i + 1}:\n";
                result += Accuracy[i].ToString();
            }
            for (int i = 0; i < Time.Count; ++i)
            {
                result += $"Test #{i + 1}:\n";
                result += Time[i].ToString();
            }
            return result;
        }
    }
}
