using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data;
using System.Windows.Media.Animation;

namespace AisAlgorithm
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();





        }

        private DataTable RemoveColumns(DataTable dt, List<string> removeColumns)
        {
            foreach (string removeCol in removeColumns)
            {
                if (dt.Columns.Contains(removeCol))
                {
                    dt.Columns.Remove(removeCol);
                }
            }
            return dt;
        }

        private DataTable BreakData(DataTable dt, List<string> breakColumns)
        {
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

        private DataTable GetData(StreamReader sr)
        {
            DataTable dt = new DataTable();
            string lineStr = string.Empty;
            char[] separators = { ',' };
            string[] tokens;
            int rowIndex = 0;
            while ((lineStr = sr.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(lineStr))
                {
                    continue;
                }
                tokens = lineStr.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (rowIndex == 0)
                {
                    dt.Columns.Add("RowIndex");
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        dt.Columns.Add(tokens[i]);
                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["RowIndex"] = rowIndex;
                    for (int i = 1; i <= tokens.Length; i++)
                    {
                        dr[i] = tokens[i - 1];
                    }
                    dt.Rows.Add(dr);
                }
                rowIndex++;
            }
            return dt;
        }

        private void btn_Execute_Click(object sender, RoutedEventArgs e)
        {
            string dataPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data.csv");

            FileStream fs = new FileStream(dataPath, FileMode.Open);
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            DataTable dt = GetData(sr);

            #region Pre-ProcessData
            List<string> removeColumns = new List<string>();
            removeColumns.Add("Date");
            removeColumns.Add("Split");
            removeColumns.Add("Real_kWh");
            //removeColumns.Add("Rel_kWh");
            removeColumns.Add("Real_Air_kWh");
            //removeColumns.Add("Rel_Air_kWh");
            //removeColumns.Add("Avg_Light");
            //removeColumns.Add("Avg_Tp");
            //removeColumns.Add("Avg_Humidity");
            removeColumns.Add("Wind_Direction");
            removeColumns.Add("Wind_Speed");
            removeColumns.Add("Avg_Comfort");
            removeColumns.Add("Season");
            removeColumns.Add("Week");
            removeColumns.Add("Is_Holiday");
            removeColumns.Add("Is_Work");
            removeColumns.Add("Target_Kwh");
            dt = RemoveColumns(dt, removeColumns);


            //List<string> breakColumns = new List<string>();
            //breakColumns.Add("Season");
            //breakColumns.Add("Week");
            //breakColumns.Add("Is_Holiday");
            //breakColumns.Add("Is_Work");
            //dt = BreakData(dt, breakColumns);

            DataTable norData = Normalization(dt);

            double testDataPercentage = 0.5;
            DataTable trainData = norData.AsEnumerable().Where(item => int.Parse(item["RowIndex"].ToString()) <= norData.Rows.Count * testDataPercentage).CopyToDataTable();
            DataTable testData = norData.AsEnumerable().Where(item => int.Parse(item["RowIndex"].ToString()) > norData.Rows.Count * testDataPercentage).CopyToDataTable();

            #endregion

            ClusteringSetting setting;
            if (GetClusteringSetting(out setting))
            {
                Clustering clustering = new Clustering(setting);
                Dictionary<int, GroupInfo> groupData = clustering.DoClustering(trainData, 3);

                Forecasting(testData, groupData, clustering);
            }


        }

        private void Forecasting(DataTable testData, Dictionary<int, GroupInfo> groupData, Clustering clustering)
        {
            double electricityValue = double.MinValue;
            foreach (DataRow dr in testData.Rows)
            {
                if (double.TryParse(dr["Rel_kWh"].ToString(), out electricityValue))
                {
                    Dictionary<int, GroupInfo> forecastingGroupData = clustering.FilterGroup(dr, groupData);
                }
            }
        }

        private DataTable Normalization(DataTable dt)
        {
            DataTable result = dt.Clone();
            Dictionary<string, MaxMinValue> norCol = new Dictionary<string, MaxMinValue>();
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName.Equals("RowIndex"))
                {
                    continue;
                }
                List<double> colList = dt.AsEnumerable().Select(item => double.Parse(item[column.ColumnName].ToString())).Distinct().ToList();
                norCol.Add(column.ColumnName, new MaxMinValue() { Max = colList.Max(), Min = colList.Min() });
            }
            double value = double.MinValue;
            foreach (DataRow dr in dt.Rows)
            {
                DataRow nDr = result.NewRow();
                nDr["RowIndex"] = dr["RowIndex"];
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

        private bool GetClusteringSetting(out ClusteringSetting setting)
        {
            bool result = true;
            setting = new ClusteringSetting();
            try
            {
                if (rb_Hamming.IsChecked.Value)
                {
                    setting.SimilarityMethod = SimilarityMethod.HammingDistence;
                    double threshold = double.MinValue;
                    if (double.TryParse(txt_HamThreshold.Text, out threshold))
                    {
                        setting.ThresholdList.Add(threshold);
                    }
                    if (double.TryParse(txt_HamThresholdErrorPercentage.Text, out threshold))
                    {
                        setting.ThresholdList.Add(threshold);
                    }
                    if (setting.ThresholdList.Count != 2)
                    {
                        result = false;
                    }
                }
                else if (rb_Eucidean.IsChecked.Value)
                {
                    setting.SimilarityMethod = SimilarityMethod.EucideanDistence;
                    double threshold = double.MinValue;
                    if (double.TryParse(txt_EucThreshold.Text, out threshold))
                    {
                        setting.ThresholdList.Add(threshold);
                    }
                    if (setting.ThresholdList.Count != 1)
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }

                if (rb_GroupAvg.IsChecked.Value)
                {
                    setting.GroupCenter = GroupCenter.Avg;
                }
                else if (rb_GroupFirst.IsChecked.Value)
                {
                    setting.GroupCenter = GroupCenter.First;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception e)
            {
            }
            return result;

        }

        private void btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting();
            setting.WindowStyle = WindowStyle.None;
            setting.AllowsTransparency = true;
            setting.Background = Brushes.Green;
            DoubleAnimation animFadeIn = new DoubleAnimation();
            animFadeIn.From = 0;
            animFadeIn.To = 1;
            animFadeIn.Duration = new Duration(TimeSpan.FromSeconds(2));
            setting.BeginAnimation(Window.OpacityProperty, animFadeIn);
            setting.ShowDialog();
        }
    }
}
