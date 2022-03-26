
namespace ClassLibrary
{
    public struct VMAccuracy
    {
        // fields
        private string more_info;

        // properties

        public VMGrid Grid { get; set; }
        public double MaxDiff { get; set; }    // value of maximum absolute difference in HA and EP modes
        public double MaxDiffArgument { get; set; }

        public double Value_VML_HA { get; set; }
        public double Value_VML_LA { get; set; }
        public double Value_VML_EP { get; set; }

        public string MoreInfo
        {
            get
            {
                if (Grid == null) return "";
                return $"Max abs {MaxDiff}\nis reached on argument {MaxDiffArgument}";
            }
            set
            {
                more_info = value;
            }
        }

        // string methods

        public override string ToString()
        {
            string grid_output = Grid.ToString();
            string accuracy_output = $"Accuracy info:\nMax Difference (HA and EP): {MaxDiff}, " +
                $"Argument: {MaxDiffArgument}, Value HA: {Value_VML_HA}, Value LA: {Value_VML_LA}, " +
                $"Value EP: {Value_VML_EP}\n";
            return grid_output + accuracy_output;
        }
    }
}
