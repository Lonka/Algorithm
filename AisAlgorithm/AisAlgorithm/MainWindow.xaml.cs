using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using AisAlgorithm.Model;


namespace AisAlgorithm
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        //用漢明分群應該比較合理
        //
        private int predictionCount;
        private DataTable sourceData;

        public MainWindow()
        {
            InitializeComponent();
            List<CheckBoxListItem> items1 = new List<CheckBoxListItem>();
            items1.Add(new CheckBoxListItem(false, "即時總用電", "Real_kWh"));
            items1.Add(new CheckBoxListItem(true, "相對總用電", "Rel_kWh"));
            items1.Add(new CheckBoxListItem(false, "即時空調用電", "Real_Air_kWh"));
            items1.Add(new CheckBoxListItem(true, "相對空調用電", "Rel_Air_kWh"));
            items1.Add(new CheckBoxListItem(true, "平均日照", "Avg_Light"));
            items1.Add(new CheckBoxListItem(true, "平均氣溫", "Avg_Tp"));
            items1.Add(new CheckBoxListItem(true, "平均溼度", "Avg_Humidity"));
            items1.Add(new CheckBoxListItem(false, "風向", "Wind_Direction"));
            items1.Add(new CheckBoxListItem(false, "風速", "Wind_Speed"));
            items1.Add(new CheckBoxListItem(false, "平均舒適度", "Avg_Comfort"));
            //items1.Add(new CheckBoxListItem(false, "季節", "Season"));
            //items1.Add(new CheckBoxListItem(false, "週別", "Week"));
            //items1.Add(new CheckBoxListItem(false, "是否為假日", "Is_Holiday"));
            //items1.Add(new CheckBoxListItem(false, "是否為上班時間", "Is_Work"));
            items1.Add(new CheckBoxListItem(true, "下個15分鐘的用電", "Target_Kwh"));
            lb_Field.ItemsSource = items1;
        }

        private void btn_Execute_Click(object sender, RoutedEventArgs e)
        {
            //讀檔
            string dataPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data.csv");
            DataTable dt;
            using (FileStream fs = new FileStream(dataPath, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                dt = GetData(sr);
            }
            //留一份原始資料
            sourceData = dt.Copy();

            #region Pre-ProcessData

            dt = RemoveColumns(dt);
            //拆解類別欄位，現已沒用
            //dt = BreakData(dt);
            dt = Normalization(dt);


            //測試資料百分比
            double testDataPercentage = double.Parse(txt_TestPercentage.Text);
            dt.Columns.Add("TempIndex");

            DataTable trainData = dt.Clone();
            DataTable testData = dt.Clone();

            List<string> seasonList = new List<string>();
            seasonList.Add("Spring");
            seasonList.Add("Summer");
            seasonList.Add("Fall");
            seasonList.Add("Winter");
            int rowIndex = 1;
            foreach (string season in seasonList)
            {
                int firstRowIndex = rowIndex;
                DataTable seasonDt = dt.AsEnumerable().Where(item => item["Season"].ToString().Equals(season)).CopyToDataTable();
                rowIndex = SetRowIndex(seasonDt, firstRowIndex);
                DataTable trainDt = seasonDt.AsEnumerable().Where(item => int.Parse(item["TempIndex"].ToString()) <= (seasonDt.Rows.Count * testDataPercentage) + firstRowIndex).CopyToDataTable();
                trainData.Merge(trainDt);
                DataTable testDt = seasonDt.AsEnumerable().Where(item => int.Parse(item["TempIndex"].ToString()) > (seasonDt.Rows.Count * testDataPercentage) + firstRowIndex).CopyToDataTable();
                testData.Merge(testDt);
            }

            trainData.Columns.Remove("Season");
            testData.Columns.Remove("Season");
            trainData.Columns.Remove("TempIndex");
            testData.Columns.Remove("TempIndex");

            #endregion


            ClusteringSetting setting;
            //取得分群的設定
            if (GetClusteringSetting(out setting))
            {
                //建立分群器
                Clustering clustering = new Clustering(setting);

                //分群結果
                Dictionary<int, GroupInfo> groupData = clustering.DoClustering(trainData, 3);

                //預測
                Forecasting(testData, groupData);
            }


        }

        private int SetRowIndex(DataTable seasonDt, int rowIndex)
        {
            foreach (DataRow dr in seasonDt.Rows)
            {
                dr["TempIndex"] = rowIndex;
                rowIndex++;
            }
            return rowIndex;
        }

        #region Pre-ProcessData
        private DataTable RemoveColumns(DataTable dt)
        {
            List<string> removeColumns = new List<string>();
            removeColumns.Add("Date");
            removeColumns.Add("Split");
            //removeColumns.Add("Season");
            removeColumns.Add("Week");
            removeColumns.Add("Is_Holiday");
            removeColumns.Add("Is_Work");

            foreach (var item in lb_Field.Items)
            {
                var checkItem = ((CheckBoxListItem)item);
                if (!checkItem.Checked)
                {
                    removeColumns.Add(checkItem.Value);
                }
            }

            foreach (string removeCol in removeColumns)
            {
                if (dt.Columns.Contains(removeCol))
                {
                    dt.Columns.Remove(removeCol);
                }
            }
            return dt;
        }

        private DataTable BreakData(DataTable dt)
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

        private Dictionary<string, MaxMinValue> norCol;
        private DataTable Normalization(DataTable dt)
        {
            if (cb_normalization.IsChecked.Value)
            {
                DataTable result = dt.Clone();
                norCol = new Dictionary<string, MaxMinValue>();
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName.Equals("RowIndex") || column.ColumnName.Equals("Season"))
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
                    nDr["Season"] = dr["Season"];
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
            else
            {
                return dt;
            }
        }
        #endregion

        #region Forecast

        private void Forecasting(DataTable testData, Dictionary<int, GroupInfo> groupData)
        {
            predictionCount = int.Parse(txt_PredictionCount.Text);

            //記錄結果
            DataTable result = new DataTable();
            result.Columns.Add("OrginalValue");
            result.Columns.Add("TargetValue");
            result.Columns.Add("ForecastValue");


            double electricityValue = double.MinValue;
            PearsonDistence predictionDis = new PearsonDistence(0);
            //EuclideanDistence predictionDis = new EuclideanDistence(0);

            double similarityValue = double.MinValue;
            ClusteringSetting forecastSetting;
            if (!GetForecastClusteringSetting(out forecastSetting))
            {
                return;
            }
            Clustering clustering = new Clustering(forecastSetting);

            int count = 0;

#if(DEBUG)
            DataTable showData = testData.Clone();
            showData.Columns.Add("Similarity");
            showData.Columns.Add("SimilarityNor");
#endif

            foreach (DataRow dr in testData.Rows)
            {

#if(DEBUG)
                showData.ImportRow(dr);
#endif

                if (count > predictionCount)
                {
                    break;
                }
                count++;
                if (double.TryParse(dr["Rel_kWh"].ToString(), out electricityValue))
                {

                    //找出與該點相似的群
                    Dictionary<int, GroupInfo> forecastingGroupData = clustering.FilterGroup(dr, groupData);

                    //計算出該點與每個群的相似度
                    //todo weight應該可以再調整
                    Dictionary<int, double> weight = CaculateWeight(forecastingGroupData);

                    //每一群該點與每個點相似度
                    Dictionary<int, List<double>> similarityGroupData = new Dictionary<int, List<double>>();

                    #region 原本調不動的方法
                    //foreach (KeyValuePair<int, GroupInfo> item in forecastingGroupData)
                    //{
                    //    //該點於該群的每一點相似度
                    //    foreach (DataRow gDr in item.Value.Rows)
                    //    {
                    //        if (predictionDis.Caculate(gDr, dr, out similarityValue))
                    //        {
                    //            if (!similarityGroupData.ContainsKey(item.Key))
                    //            {
                    //                similarityGroupData.Add(item.Key, new List<double>());
                    //            }
                    //            similarityGroupData[item.Key].Add(similarityValue);
                    //        }
                    //    }
                    //    //該群的相似度平均
                    //    double similarityGroupAvg = similarityGroupData[item.Key].Average();
                    //    double similaritySum = 0;

                    //    //加總(平均 - 每個點)
                    //    foreach (double similarity in similarityGroupData[item.Key])
                    //    {
                    //        similaritySum += similarityGroupAvg - similarity;
                    //    }

                    //    //原值加上調整量
                    //    electricityValue += (similaritySum * weight[item.Key]);
                    //}
                    #endregion

                    Dictionary<int, List<double>> similarityGroupDataElec = new Dictionary<int, List<double>>();
                    double forecastValue = 0;
                    foreach (KeyValuePair<int, GroupInfo> item in forecastingGroupData)
                    {
#if(DEBUG)
                        DataRow groupAvg = showData.NewRow();
                        foreach (DataColumn col in testData.Columns)
                        {
                            if (item.Value.ColAvg.ContainsKey(col.ColumnName))
                            {
                                groupAvg[col.ColumnName] = item.Value.ColAvg[col.ColumnName];
                            }
                        }
                        groupAvg["RowIndex"] = "GroupID:" + item.Key;
                        showData.Rows.Add(groupAvg);
                        DataRow groupCalculate = showData.NewRow();
                        foreach (DataColumn col in testData.Columns)
                        {
                            if (item.Value.ColCaculate.ContainsKey(col.ColumnName))
                            {
                                groupCalculate[col.ColumnName] = item.Value.ColCaculate[col.ColumnName];
                            }
                        }
                        groupCalculate["RowIndex"] = "GroupID:" + item.Key;
                        showData.Rows.Add(groupCalculate);
#endif


                        //該點於該群的每一點相似度
                        foreach (DataRow gDr in item.Value.Rows)
                        {
                            if (predictionDis.Caculate(gDr, dr, out similarityValue))
                            {
                                if (!similarityGroupData.ContainsKey(item.Key))
                                {
                                    similarityGroupData.Add(item.Key, new List<double>());
                                }
                                if (!similarityGroupDataElec.ContainsKey(item.Key))
                                {
                                    similarityGroupDataElec.Add(item.Key, new List<double>());
                                }

                                //similarityGroupData[item.Key].Add(similarityValue);
                                double elec;
                                if (double.TryParse(gDr["Target_Kwh"].ToString(), out elec))
                                {
                                    similarityGroupData[item.Key].Add(similarityValue);

                                    similarityGroupDataElec[item.Key].Add(elec);
                                }

#if(DEBUG)
                                DataRow showRow = showData.NewRow();
                                foreach (DataColumn col in testData.Columns)
                                {
                                    showRow[col.ColumnName] = gDr[col.ColumnName];
                                }
                                showRow["Similarity"] = similarityValue;
                                showData.Rows.Add(showRow);
#endif

                            }
                        }

                        //var tt = similarityGroupData[item.Key].Aggregate(1.0, (x, y) => x * y);
                        //double minSimilarity = similarityGroupData[item.Key].Min();
                        //double maxSimilarity = similarityGroupData[item.Key].Max();
                        double sumSimilarity = similarityGroupData[item.Key].Sum();
                        for (int i = 0; i < similarityGroupData[item.Key].Count; i++)
                        {
                            //similarityGroupData[item.Key][i] = (similarityGroupData[item.Key][i] - minSimilarity) / (maxSimilarity - minSimilarity);
                            similarityGroupData[item.Key][i] = (similarityGroupData[item.Key][i]) / (sumSimilarity);
                        }
#if(DEBUG)
                        int sCount = similarityGroupData[item.Key].Count - 1;
                        for (int i = showData.Rows.Count - 1; i > showData.Rows.Count - 1 - similarityGroupData[item.Key].Count; i--)
                        {
                            showData.Rows[i]["SimilarityNor"] = similarityGroupData[item.Key][sCount];
                            sCount--;
                        }
#endif
                        double similarityTemp = 0;
                        for (int i = 0; i < similarityGroupData[item.Key].Count; i++)
                        {
                            similarityTemp += similarityGroupData[item.Key][i] * similarityGroupDataElec[item.Key][i];
                        }
                        //forecastValue += weight[item.Key] * (item.Value.ColAvg["Target_Kwh"] + ((similarityGroupDataElec[item.Key].Sum()) / similarityGroupData[item.Key].Sum()));
                        forecastValue += weight[item.Key] * (similarityTemp);
#if(DEBUG)
                        var sourceRow = showData.AsEnumerable().Where(i => i["RowIndex"].ToString().Equals(dr["RowIndex"].ToString())).FirstOrDefault();
                        if (sourceRow != null)
                        {
                            sourceRow["Similarity"] = forecastValue;
                        }
                        showData.Rows.Add(showData.NewRow());
#endif
                    }


                    //抓出目標值
                    double targetValue = double.MinValue;
                    var targetValueStr = sourceData.AsEnumerable().Where(item => item["RowIndex"].ToString().Equals(dr["RowIndex"].ToString())).Select(item => item["Target_Kwh"].ToString()).FirstOrDefault();

                    //原值
                    var relValueStr = sourceData.AsEnumerable().Where(item => item["RowIndex"].ToString().Equals(dr["RowIndex"].ToString())).Select(item => item["Rel_kWh"].ToString()).FirstOrDefault();

                    //記錄下預測結果
                    if (double.TryParse(targetValueStr, out targetValue))
                    {
                        DataRow rDr = result.NewRow();
                        rDr["OrginalValue"] = double.Parse(relValueStr);
                        rDr["ForecastValue"] = (cb_normalization.IsChecked.Value ? forecastValue * (norCol["Rel_kWh"].Max - norCol["Rel_kWh"].Min) + norCol["Rel_kWh"].Min : forecastValue);
                        rDr["TargetValue"] = targetValue;
                        result.Rows.Add(rDr);
                    }
                }
            }

            //存檔
            SaveToCSV(result, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv"));

#if(DEBUG)
            SaveToCSV(showData, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMddHHmmss_Da") + ".csv"));
#endif
            MessageBox.Show("OK");

        }


        private static Dictionary<int, double> CaculateWeight(Dictionary<int, GroupInfo> forecastingGroupData)
        {
            Dictionary<int, double> weight = new Dictionary<int, double>();

            foreach (KeyValuePair<int, GroupInfo> item in forecastingGroupData)
            {
                weight[item.Key] = item.Value.SimilarityValue / forecastingGroupData.Values.Sum(s => s.SimilarityValue);
            }
            return weight;
        }

        private bool GetForecastClusteringSetting(out ClusteringSetting forecastSetting)
        {
            bool result = true;
            forecastSetting = new ClusteringSetting();
            //forecastSetting.GroupCenter = GroupCenter.Avg;
            //forecastSetting.SimilarityMethod = SimilarityMethod.HammingDistence;
            //forecastSetting.ThresholdList.Add(0);
            //forecastSetting.ThresholdList.Add(5);            
            forecastSetting.GroupCenter = GroupCenter.Avg;
            forecastSetting.SimilarityMethod = SimilarityMethod.PearsonDistence;
            forecastSetting.ThresholdList.Add(0.95);

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
                else if (rb_Pearson.IsChecked.Value)
                {
                    setting.SimilarityMethod = SimilarityMethod.PearsonDistence;
                    double threshold = double.MinValue;
                    if (double.TryParse(txt_PearsonThreshold.Text, out threshold))
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

        public void SaveToCSV(DataTable oTable, string FilePath)
        {
            string data = "";
            StreamWriter wr = new StreamWriter(FilePath, false, System.Text.Encoding.Default);
            foreach (DataColumn column in oTable.Columns)
            {
                data += column.ColumnName + ",";
            }
            data += "\n";
            wr.Write(data);
            data = "";

            foreach (DataRow row in oTable.Rows)
            {
                foreach (DataColumn column in oTable.Columns)
                {
                    data += row[column].ToString().Trim() + ",";
                }
                data += "\n";
                wr.Write(data);
                data = "";
            }
            data += "\n";

            wr.Dispose();
            wr.Close();
        }

        #endregion


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
