using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisAlgorithm
{
    internal class EuclideanDistence : ISimilarity
    {

        public EuclideanDistence(double _similarityThreshold)
        {
            similarityThreshold = _similarityThreshold;
        }
        private double similarityThreshold = 0.1;
        public bool Caculate(Dictionary<string, double> colCaculate, System.Data.DataRow dr, out double similarityValue)
        {
            similarityValue = double.MinValue;
            double orginalValue = double.MinValue;
            Dictionary<string, double> tempValues = new Dictionary<string, double>();
            foreach (KeyValuePair<string, double> col in colCaculate)
            {
                if (double.TryParse(dr[col.Key].ToString(), out orginalValue) && !tempValues.ContainsKey(col.Key))
                {
                    tempValues.Add(col.Key, orginalValue);
                }
            }

            if (tempValues.Count == colCaculate.Count)
            {
                double distince = 0;
                foreach (KeyValuePair<string, double> col in colCaculate)
                {
                    distince += Math.Pow(tempValues[col.Key] - col.Value, 2);
                }
                distince = Math.Sqrt(distince);
                double similarity = 1 / (distince + 1);
                if (similarity > similarityThreshold)
                {
                    similarityValue = similarity;
                    return true;
                }
            }
            return false;
        }
    }
}
