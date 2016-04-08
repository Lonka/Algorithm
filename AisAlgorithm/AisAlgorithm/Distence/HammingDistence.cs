using AisAlgorithm.Model;
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

        public HammingDistence(List<double> _params)
        {
            colThreshold = _params[0];
            errorPercentage = _params[1];
            if (_params.Count >= 3)
            {
                compareTarget = (_params[2].Equals(0) ? false : true);
            }
        }
        private double colThreshold = 0;//小於都可以

        private double errorPercentage = 40;

        private bool compareTarget = App.CompareTarget;
        public bool Caculate(System.Data.DataRow targetDr, System.Data.DataRow dr, out double similarityValue)
        {
            Dictionary<string, double> colCaculate = new Dictionary<string, double>();
            double orginalValue = double.MinValue;
            similarityValue = double.MinValue;
            foreach (DataColumn dc in targetDr.Table.Columns)
            {
                if (dc.ColumnName != "GroupId" && dc.ColumnName != "RowIndex" && dc.ColumnName != "Date"
                    && double.TryParse(targetDr[dc.ColumnName].ToString(), out orginalValue))
                {
                    colCaculate[dc.ColumnName] = orginalValue;
                }
            }
            return colCaculate.Count == dr.Table.Columns.Count - 2 && Caculate(colCaculate, dr, out similarityValue);
        }
        public bool Caculate(Dictionary<string, double> colCaculate, DataRow dr, out double similarityValue)
        {
            Model.GroupInfo group = new Model.GroupInfo();
            group.ColCaculate = colCaculate;
            return Caculate(group, dr, out similarityValue);
        }

        public bool Caculate(Model.GroupInfo groupInfo, DataRow dr, out double similarityValue)
        {
            similarityValue = double.MinValue;
            double newValue = double.MinValue;
            int colOverThreshold = 0;
            double colCount = 0;
            foreach (KeyValuePair<string, double> col in groupInfo.ColCaculate)
            {
                if (!compareTarget && col.Key.Equals("Target_Kwh"))
                {
                    continue;
                }

                if (double.TryParse(dr[col.Key].ToString(), out newValue))
                {
                    double error = 0;
                    if (groupInfo.Fuzzy)
                    {
                        HelfRegion region = CommonUtil.GetMembershipFunction(newValue, Parameter.GetInstance().FuzzyCol[col.Key]);
                        if (region.RegionName != groupInfo.FuzzyValue[col.Key])
                        {
                            error = 100;
                        }
                    }
                    else
                    {
                        //http://zh.wikihow.com/%E8%AE%A1%E7%AE%97%E7%99%BE%E5%88%86%E6%AF%94%E5%8F%98%E5%8C%96
                        if (col.Value == 0)
                        {
                            error = Math.Abs(newValue) * 100;
                        }
                        else
                        {
                            //error = Math.Abs((newValue - col.Value) / col.Value) * 100;
                            error = Math.Abs((newValue - col.Value) / (Parameter.GetInstance().NorCol[col.Key].Max - Parameter.GetInstance().NorCol[col.Key].Min)) * 100;
                        }
                    }
                    colCount++;
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
            //similarityValue = colOverThreshold > 0 ? 0 : 1; // (colCount - colOverThreshold) / colCount;
            similarityValue = (colCount - colOverThreshold) / colCount;
            return true;
        }
    }
}
