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
            double orginalValue = double.MinValue;
            int colOverThreshold = 0;
            foreach (KeyValuePair<string, double> col in colCaculate)
            {
                if (double.TryParse(dr[col.Key].ToString(), out orginalValue))
                {
                    double error = Math.Abs((col.Value - orginalValue) / orginalValue) * 100;
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
