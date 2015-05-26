using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WM_Fuzzy
{
    public class Forecast
    {
        public double before { get; set; }
        public double after { get; set; }
    }

    public class EnergyColumn
    {
        public EnergyColumn(string _name, string _chiName,int _regionCount)
        {
            ChiName = _chiName;
            Name = _name;
            RegionCount = _regionCount;
        }
        public string Name { get; set; }
        public string ChiName { get; set; }
        /// <summary>
        /// 要切割的Region個數
        /// </summary>
        public int RegionCount { get; set; }
    }

    public class RuleType
    {
        public RuleType(string _xRule, string _yRule, List<RuleFeature> _ruleFeature)
        {
            RuleFeature = _ruleFeature;
            XRule = _xRule;
            YRule = _yRule;
        }
        public string XRule { get; set; }
        public string YRule { get; set; }
        //算各feature的mf值及所屬region
        public List<RuleFeature> RuleFeature { get; set; }

    }
    public class RuleFeature
    {
        public string RegionName { get; set; }
        public Feature Feature { get; set; }
        public double MFValue { get; set; }
    }


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

    public class Region
    {
        public string RegionName { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double CenterValue { get; set; }

        public double CalculateCenterValue(string type)
        {
            double result = 0;
            switch (type)
            {
                case "left":
                    result = MaxValue;
                    break;
                case "right":
                    result = MinValue;
                    break;
                case "triangle":
                    result = (MaxValue + MinValue) / 2;
                    break;
            }
            return result;
        }
    }

    public class Feature
    {
        public string FeatureName { get; set; }
        public List<Region> Regions { get; set; }
        public List<HelfRegion> HelfRegions { get; set; }
        public bool IsTargetValue { get; set; }


        public double GetCenterValueByRegionName(string regionName)
        {
            return Regions.Where(item => item.RegionName.Equals(regionName)).FirstOrDefault().CenterValue;
        }
    }
}
