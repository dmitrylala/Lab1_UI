
namespace ClassLibrary
{
    public struct VMTime
    {
        // fields
        private string more_info;

        // properties

        public VMGrid Grid { get; set; }
        public double Time_VML_HA { get; set; }
        public double Time_VML_LA { get; set; }
        public double Time_VML_EP { get; set; }
        public double Coef_LA_HA { get; set; }
        public double Coef_EP_HA { get; set; }

        public string MoreInfo
        {
            get
            {
                if (Grid == null) return "";
                return $"Coef time LA / HA: {Coef_LA_HA}, coef time EP / HA: {Coef_EP_HA}";
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
            string time_output = $"Time info:\nTime HA: {Time_VML_HA}, Time LA: {Time_VML_LA}, " +
                $"Time EP: {Time_VML_EP}, Coef LA/HA: {Coef_LA_HA}, Coef EP/HA: {Coef_EP_HA}\n";
            return grid_output + time_output;
        }
    }
}
