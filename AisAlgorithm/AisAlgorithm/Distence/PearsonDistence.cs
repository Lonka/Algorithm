using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AisAlgorithm
{
    internal class PearsonDistence : ISimilarity
    {
        public PearsonDistence(double _similarityThreshold)
        {
            similarityThreshold = _similarityThreshold;
        }
        private double similarityThreshold = 0.9;

        public bool Caculate(System.Data.DataRow targetDr, System.Data.DataRow dr, out double similarityValue)
        {
            Dictionary<string, double> colCaculate = new Dictionary<string, double>();
            double orginalValue = double.MinValue;
            similarityValue = double.MinValue;
            foreach (DataColumn dc in targetDr.Table.Columns)
            {
                if (dc.ColumnName != "GroupId"
                    && double.TryParse(targetDr[dc.ColumnName].ToString(), out orginalValue))
                {
                    colCaculate[dc.ColumnName] = orginalValue;
                }
            }

            return colCaculate.Count == dr.Table.Columns.Count && Caculate(colCaculate, dr, out similarityValue);
        }
        public bool Caculate(Dictionary<string, double> colCaculate, System.Data.DataRow dr, out double similarityValue)
        {
            similarityValue = double.MinValue;
            double groupAvg = 0;
            double selfAvg = 0;
            double orginalValue = double.MinValue;
            Dictionary<string, double> selfValue = new Dictionary<string, double>();
            foreach (KeyValuePair<string, double> col in colCaculate)
            {
                if (col.Key == "RowIndex" || col.Key == "Target_Kwh")
                {
                    continue;
                }
                groupAvg += col.Value;
                if (double.TryParse(dr[col.Key].ToString(), out orginalValue))
                {
                    selfValue[col.Key] = orginalValue;
                    selfAvg += orginalValue;
                }
                else
                {
                    return false;
                }
            }
            groupAvg = groupAvg / (colCaculate.Count - 2);
            selfAvg = selfAvg / (colCaculate.Count - 2);

            double numerator = 0;
            double denominatorLeft = 0;
            double denominatorRight = 0;
            foreach (KeyValuePair<string, double> col in colCaculate)
            {
                if (col.Key == "RowIndex" || col.Key == "Target_Kwh")
                {
                    continue;
                }

                numerator += (selfValue[col.Key] - selfAvg) * (col.Value - groupAvg);
                denominatorLeft += Math.Pow((selfValue[col.Key] - selfAvg), 2);
                denominatorRight += Math.Pow((col.Value - groupAvg), 2);
            }

            //similarityValue = (numerator / (Math.Sqrt(denominatorLeft) + Math.Sqrt(denominatorRight))) / (colCaculate.Count-2);
            double similarity = (numerator / (Math.Sqrt(denominatorLeft * denominatorRight)));

            if (similarity > similarityThreshold)
            {
                similarityValue = similarity;
                return true;
            }
            return false ;

        }
    }
}
