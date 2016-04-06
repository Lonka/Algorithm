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
                if (dc.ColumnName != "GroupId" && dc.ColumnName != "RowIndex"
                    && double.TryParse(targetDr[dc.ColumnName].ToString(), out orginalValue))
                {
                    colCaculate[dc.ColumnName] = orginalValue;
                }
            }

            return colCaculate.Count == dr.Table.Columns.Count-1 && Caculate(colCaculate, dr, out similarityValue);
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
            double groupAvg = 0;
            double selfAvg = 0;
            double orginalValue = double.MinValue;
            Dictionary<string, double> selfValue = new Dictionary<string, double>();
            int avgCount = 0;
            foreach (KeyValuePair<string, double> col in groupInfo.ColCaculate)
            {
                if (!App.CompareTarget && col.Key == "Target_Kwh")
                {
                    continue;
                }
                groupAvg += col.Value;
                if (double.TryParse(dr[col.Key].ToString(), out orginalValue))
                {
                    selfValue[col.Key] = orginalValue;
                    selfAvg += orginalValue;
                    avgCount++;
                }
                else
                {
                    return false;
                }
            }
            groupAvg = groupAvg / avgCount;
            selfAvg = selfAvg / avgCount;

            double numerator = 0;
            double denominatorLeft = 0;
            double denominatorRight = 0;
            foreach (KeyValuePair<string, double> col in groupInfo.ColCaculate)
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
            if (double.IsNaN(similarity))
            {
                similarity = 0;
            }
            if (similarity > similarityThreshold)
            {
                similarityValue = similarity;
                return true;
            }
            return false;
        }
    }
}
