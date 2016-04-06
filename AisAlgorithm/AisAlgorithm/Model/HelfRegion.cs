using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisAlgorithm.Model
{
    public class HelfRegion
    {
        public enum Direction
        {
            left,
            right
        }
        public string RegionName { get; set; }
        public Direction MfDirection { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double CalculateMfValue(double xValue)
        {
            double result = 0;
            if (MfDirection == Direction.left)
            {
                result = (xValue - MinValue) / (MaxValue - MinValue);
            }
            else if (MfDirection == Direction.right)
            {
                result = (MaxValue - xValue) / (MaxValue - MinValue);
            }
            return result;
        }
    }
}
