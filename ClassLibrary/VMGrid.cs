
namespace ClassLibrary
{
    public class VMGrid
    {
        // properties

        public int Length { get; set; }
        public double LeftEnd { get; set; }
        public double RightEnd { get; set; }
        public double Dx
        {
            get { return (RightEnd - LeftEnd) / Length; }
        }
        public VMf Function { get; set; }

        // string methods

        public override string ToString() => $"Grid info:\nLength: {Length}, Left end: {LeftEnd}, " +
            $"Right end: {RightEnd}, dx: {Dx}, func: {Function}\n";
    }
}
