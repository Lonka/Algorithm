using System;
using System.Collections.Generic;
using System.Data;
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

        public bool Caculate(System.Data.DataRow targetDr, System.Data.DataRow dr, out double similarityValue)
        {
            Dictionary<string, double> colCaculate = new Dictionary<string, double>();
            double orginalValue = double.MinValue;
            similarityValue = double.MinValue;
            foreach (DataColumn dc in targetDr.Table.Columns)
            {
                if ((dc.ColumnName != "GroupId" && dc.ColumnName != "RowIndex" && dc.ColumnName != "Date")
                    && double.TryParse(targetDr[dc.ColumnName].ToString(), out orginalValue))
                {
                    colCaculate[dc.ColumnName] = orginalValue;
                }
            }

            return colCaculate.Count == dr.Table.Columns.Count -2 && Caculate(colCaculate, dr, out similarityValue);
        }
        public bool Caculate(Dictionary<string, double> colCaculate, System.Data.DataRow dr, out double similarityValue)
        {
            Model.GroupInfo group = new Model.GroupInfo();
            group.ColCaculate = colCaculate;
            return Caculate(group, dr, out similarityValue);
        }


        public bool Caculate(Model.GroupInfo groupInfo, DataRow dr, out double similarityValue)
        {
            similarityValue = double.MinValue;
            double orginalValue = double.MinValue;
            Dictionary<string, double> tempValues = new Dictionary<string, double>();
            foreach (KeyValuePair<string, double> col in groupInfo.ColCaculate)
            {
                if (!App.CompareTarget && col.Key.Equals("Target_Kwh"))
                {
                    continue;
                }
                if (double.TryParse(dr[col.Key].ToString(), out orginalValue) && !tempValues.ContainsKey(col.Key))
                {
                    tempValues.Add(col.Key, orginalValue);
                }
            }

            if (tempValues.Count == groupInfo.ColCaculate.Count - 1)
            {
                double distince = 0;
                foreach (KeyValuePair<string, double> col in groupInfo.ColCaculate)
                {
                    if (!App.CompareTarget && col.Key.Equals("Target_Kwh"))
                    {
                        continue;
                    }
                    distince += Math.Pow(tempValues[col.Key] - col.Value, 2);
                }
                distince = Math.Sqrt(distince);
                double similarity = 1;
                if (distince != 0)
                {
                    similarity = 1 / distince;
                }
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
