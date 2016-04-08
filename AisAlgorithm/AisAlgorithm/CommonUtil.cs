using AisAlgorithm.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisAlgorithm
{
    internal class CommonUtil
    {
        public static HelfRegion GetMembershipFunction(double value, List<HelfRegion> helfRegions)
        {
            HelfRegion result = null;
            var helfRegionsByValue = helfRegions.Where(item => value < item.MaxValue && value >= item.MinValue);
            if (helfRegionsByValue.Any())
            {
                double mfValue = 0;
                foreach (var item in helfRegionsByValue)
                {
                    if (item.CalculateMfValue(value) > mfValue)
                    {
                        mfValue = item.CalculateMfValue(value);
                        result = item;
                    }
                }
            }
            else
            {
                result = helfRegions[helfRegions.Count - 1];
            }
            return result;
        }

        public static List<HelfRegion> GenHelfRegion(EnergyColumn ec, DataTable dt)
        {
            double maxValue = dt.AsEnumerable().Max(item =>
            {
                double value = 0;
                double.TryParse(item[ec.Value].ToString(), out value);
                return value;
            });
            double minValue = dt.AsEnumerable().Min(item =>
            {
                double value = 0;
                double.TryParse(item[ec.Value].ToString(), out value);
                return value;
            });

            double step = (maxValue - minValue) / (ec.MemberFunctionRegion - 1);
            List<HelfRegion> regions = new List<HelfRegion>();
            int regionNameChar = 65;

            double tempValue = minValue;
            for (double i = 0; i < ec.MemberFunctionRegion - 1; i += 1)
            {
                regions.Add(new HelfRegion()
                {
                    MinValue = tempValue,
                    MaxValue = tempValue + step,
                    MfDirection = HelfRegion.Direction.right,
                    RegionName = Convert.ToChar(regionNameChar).ToString()
                });
                regionNameChar++;
                regions.Add(new HelfRegion()
                {
                    MinValue = tempValue,
                    MaxValue = tempValue + step,
                    MfDirection = HelfRegion.Direction.left,
                    RegionName = Convert.ToChar(regionNameChar).ToString()
                });
                tempValue += step;
            }
            return regions;
        }

        public static DataTable Normalization(DataTable dt, out Dictionary<string, MaxMinValue> norCol)
        {

            DataTable result = dt.Clone();
            SetMaxMin(dt,out norCol);
            double value = double.MinValue;
            foreach (DataRow dr in dt.Rows)
            {
                DataRow nDr = result.NewRow();
                nDr["RowIndex"] = dr["RowIndex"];
                nDr["Season"] = dr["Season"];
                nDr["Date"] = dr["Date"];
                foreach (KeyValuePair<string, MaxMinValue> item in norCol)
                {
                    if (double.TryParse(dr[item.Key].ToString(), out value))
                    {
                        nDr[item.Key] = (value - item.Value.Min) / (item.Value.Max - item.Value.Min);
                    }
                }
                result.Rows.Add(nDr);
            }
            return result;

        }

        public static void SetMaxMin(DataTable dt,out  Dictionary<string, MaxMinValue> norCol)
        {
            norCol = new Dictionary<string, MaxMinValue>();
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName.Equals("RowIndex") || column.ColumnName.Equals("Season") || column.ColumnName.Equals("Date"))
                {
                    continue;
                }
                List<double> colList = dt.AsEnumerable().Select(item => double.Parse(item[column.ColumnName].ToString())).Distinct().ToList();
                norCol.Add(column.ColumnName, new MaxMinValue() { Max = colList.Max(), Min = colList.Min() });
            }
        }

        public static DataTable BreakData(DataTable dt)
        {
            List<string> breakColumns = new List<string>();
            breakColumns.Add("Season");
            breakColumns.Add("Week");
            breakColumns.Add("Is_Holiday");
            breakColumns.Add("Is_Work");

            foreach (string breakColumn in breakColumns)
            {
                if (dt.Columns.Contains(breakColumn))
                {
                    var colTypes = dt.AsEnumerable().Select(item => item[breakColumn].ToString()).Distinct();
                    foreach (var colType in colTypes)
                    {
                        if (!string.IsNullOrEmpty(colType))
                        {
                            DataColumn dc = new DataColumn();
                            dc.ColumnName = breakColumn + "_" + colType;
                            dc.DefaultValue = 0;
                            dt.Columns.Add(dc);
                        }
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        dr[breakColumn + "_" + dr[breakColumn]] = 1;
                    }

                    dt.Columns.Remove(breakColumn);
                }
            }
            return dt;
        }


    }
}
