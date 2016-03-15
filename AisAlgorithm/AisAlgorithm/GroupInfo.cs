using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisAlgorithm
{
    internal class GroupInfo
    {
        private GroupCenter m_center;
        public GroupInfo(GroupCenter _center)
        {
            m_center = _center;
        }

        public GroupInfo()
        {
            m_center = GroupCenter.First;
        }
        public int GroupId { get; set; }

        private List<DataRow> _row = new List<DataRow>();
        public List<DataRow> Rows
        {
            get
            {
                return _row;
            }
            set
            {
                _row = value;
            }
        }

        private Dictionary<string, double> _colSum = new Dictionary<string, double>();
        public Dictionary<string, double> ColSum
        {
            get
            {
                return _colSum;
            }
            set
            {
                _colSum = value;
            }
        }

        private Dictionary<string, double> _colAvg = new Dictionary<string, double>();
        public Dictionary<string, double> ColAvg
        {
            get
            {
                return _colAvg;
            }
            set
            {
                _colAvg = value;
            }
        }

        private Dictionary<string, double> _colCaculate = new Dictionary<string, double>();
        public Dictionary<string, double> ColCaculate
        {
            get
            {
                return _colCaculate;
            }
            set
            {
                _colCaculate = value;
            }
        }

        public double SimilarityValue { get; set; }

        public bool Add(DataRow dr)
        {
            bool result = false;
            try
            {
                double value = double.MinValue;
                bool checkValue = true;
                Dictionary<string, double> tempValue = new Dictionary<string, double>();
                foreach (DataColumn col in dr.Table.Columns)
                {
                    if (!col.ColumnName.Equals("GroupId"))
                    {
                        if (!double.TryParse(dr[col.ColumnName].ToString(), out value))
                        {
                            checkValue = false;
                            break;
                        }
                        if (!col.ColumnName.Equals("RowIndex"))
                        {
                            tempValue[col.ColumnName] = value;
                        }
                    }
                }

                if (checkValue)
                {
                    Rows.Add(dr);
                    foreach (string colName in tempValue.Keys)
                    {
                        if (!ColSum.ContainsKey(colName))
                        {
                            ColSum.Add(colName, 0);
                            ColAvg.Add(colName, 0);
                            ColCaculate.Add(colName, 0);
                            if (m_center == GroupCenter.First)
                            {
                                ColCaculate[colName] = tempValue[colName];
                            }
                        }
                        ColSum[colName] += tempValue[colName];
                        ColAvg[colName] = ColSum[colName] / Rows.Count;

                        if (m_center == GroupCenter.Avg)
                        {
                            ColCaculate[colName] = ColAvg[colName];
                        }
                    }
                    result = true;
                }
            }
            catch (Exception e)
            {
                //Log
            }
            return result;
        }
    }
}
