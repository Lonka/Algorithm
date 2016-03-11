using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisAlgorithm
{
    internal class HammingDistence : ISimilarity
    {
        public HammingDistence(double _colThreshold, double _errorPercentage)
        {
            colThreshold = _colThreshold;
            errorPercentage = _errorPercentage;
        }
        private double colThreshold = 0;//小於都可以

        private double errorPercentage = 40;
        public bool Caculate(Dictionary<string, double> colCaculate, DataRow dr, out double similarityValue)
        {
            similarityValue = double.MinValue;
            double newValue = double.MinValue;
            int colOverThreshold = 0;
            foreach (KeyValuePair<string, double> col in colCaculate)
            {
                if (double.TryParse(dr[col.Key].ToString(), out newValue))
                {
                    double error = 0;
                    //http://zh.wikihow.com/%E8%AE%A1%E7%AE%97%E7%99%BE%E5%88%86%E6%AF%94%E5%8F%98%E5%8C%96
                    if (col.Value == 0)
                    {
                        error = Math.Abs(newValue) * 100;
                    }
                    else
                    {
                        error = Math.Abs((newValue - col.Value) / col.Value) * 100;
                    }

                    if (error > errorPercentage)
                    {
                        colOverThreshold++;
                        if (colOverThreshold > colThreshold)
                        {
                            return false;
                        }
                    }
                }
            }
            similarityValue = 1;
            return true;
        }

    }
}
