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
using AisAlgorithm.Category;


namespace AisAlgorithm
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        //用漢明分群應該比較合理
        private int PredictionCount;
        private DataTable SourceData;

        public MainWindow()
        {
            InitializeComponent();
            Parameter.GetInstance().NorCol = new Dictionary<string, MaxMinValue>();
            Parameter.GetInstance().FuzzyCol = new Dictionary<string, List<HelfRegion>>();
            Parameter.GetInstance().Items = new List<EnergyColumn>();
            Parameter.GetInstance().Items.Add(new EnergyColumn(true, "相對總用電", "Rel_kWh", 3));
            Parameter.GetInstance().Items.Add(new EnergyColumn(true, "相對空調用電", "Rel_Air_kWh", 3));
            Parameter.GetInstance().Items.Add(new EnergyColumn(true, "平均氣溫", "Avg_Tp", 3));
            Parameter.GetInstance().Items.Add(new EnergyColumn(false, "平均溼度", "Avg_Humidity", 3));
            Parameter.GetInstance().Items.Add(new EnergyColumn(false, "即時總用電", "Real_kWh", 3));
            Parameter.GetInstance().Items.Add(new EnergyColumn(false, "即時空調用電", "Real_Air_kWh", 3));
            Parameter.GetInstance().Items.Add(new EnergyColumn(true, "平均日照", "Avg_Light", 3));
            Parameter.GetInstance().Items.Add(new EnergyColumn(false, "風向", "Wind_Direction", 3));
            Parameter.GetInstance().Items.Add(new EnergyColumn(false, "風速", "Wind_Speed", 3));
            Parameter.GetInstance().Items.Add(new EnergyColumn(false, "平均舒適度", "Avg_Comfort", 3));
            //items1.Add(new CheckBoxListItem(false, "季節", "Season"));
            //items1.Add(new CheckBoxListItem(false, "週別", "Week"));
            //items1.Add(new CheckBoxListItem(false, "是否為假日", "Is_Holiday"));
            //items1.Add(new CheckBoxListItem(false, "是否為上班時間", "Is_Work"));
            Parameter.GetInstance().Items.Add(new EnergyColumn(true, "下個15分鐘的用電", "Target_Kwh", 3));

            //cb_fuzzy.IsChecked = true;
            //cb_normalization.IsChecked = true;
            foreach (EnergyColumn cb in Parameter.GetInstance().Items)
            {
                cb.IsFuzzy = cb_fuzzy.IsChecked.Value;
            }

            lb_Field.ItemsSource = Parameter.GetInstance().Items;
        }

        private void btn_Execute_Click(object sender, RoutedEventArgs e)
        {

            //GroupCenter gcc = GroupCenter.Avg;
            //GroupCenter ffc = GroupCenter.Avg;
            //for (int cc = 0; cc <= 1; cc++)
            //{
            //    if (cc == 1)
            //    {
            //        gcc = GroupCenter.First;
            //    }
            //    for (int fc = 0; fc <= 1; fc++)
            //    {
            //        if (fc == 1)
            //        {
            //            ffc = GroupCenter.First;
            //        }
            //        else
            //        {
            //            ffc = GroupCenter.Avg;
            //        }
            //for (int i = 3; i <= 10; i++)
            //{
            //    for (int j = i + 2; j <= i + 6; j++)
            //    {
            //        Execute(GroupCenter.First, GroupCenter.Avg, i, j);
            //    }
            //}
            //    }
            //}

            // Execute(GroupCenter.First, GroupCenter.Avg,10, 10);

            //Execute(GroupCenter.First, GroupCenter.Avg, 0.03, 0.03);

            Execute(GroupCenter.First, GroupCenter.Avg, 2, 3);
            //Execute(GroupCenter.First, GroupCenter.Avg, 3, 7);
            // Execute(GroupCenter.First, GroupCenter.Avg, 3, 8);
            //Execute(GroupCenter.First, GroupCenter.Avg, 4, 6);
            //Execute(GroupCenter.First, GroupCenter.Avg, 4, 7);
            //Execute(GroupCenter.First, GroupCenter.Avg, 4, 8);
            //Execute(GroupCenter.First, GroupCenter.Avg, 4, 9);
            //Execute(GroupCenter.First, GroupCenter.Avg, 5, 2);
            //Execute(GroupCenter.First, GroupCenter.Avg, 5, 3);
            //Execute(GroupCenter.First, GroupCenter.Avg, 5, 4);
            //Execute(GroupCenter.First, GroupCenter.Avg, 1, 2);
            //Execute(GroupCenter.First, GroupCenter.Avg, 2, 3);
            //Execute(GroupCenter.First, GroupCenter.Avg, 3, 5);
            //Execute(GroupCenter.First, GroupCenter.Avg, 5, 10);

            //Execute(GroupCenter.First, GroupCenter.Avg, 1, 2);

            //GOOD
            //Execute(GroupCenter.First, GroupCenter.Avg, 2, 3);
            //Execute(GroupCenter.First, GroupCenter.Avg, 2,5);
            //Execute(GroupCenter.First, GroupCenter.Avg, 3, 6);
            //Execute(GroupCenter.First, GroupCenter.Avg, 4, 7);


            //Execute(GroupCenter.First, GroupCenter.Avg, 5, 8);

            //Execute(GroupCenter.First, GroupCenter.First, 2, 3);
            //Execute(GroupCenter.Avg, GroupCenter.Avg, 2, 3);
            //Execute(GroupCenter.Avg, GroupCenter.First, 2, 3);


            //for (int i = 1; i <= 5; i++)
            //{
            //    for (int j = 1; j <= 5; j++)
            //    {
            //        Execute(i, j);
            //    }
            //}
            MessageBox.Show("OK");

        }

        private void Execute(GroupCenter clusterCenter, GroupCenter forecastCenter, double clusterIndex, double forecastIndex)
        {
            Parameter.GetInstance().NorCol = new Dictionary<string, MaxMinValue>();
            Parameter.GetInstance().FuzzyCol = new Dictionary<string, List<HelfRegion>>();
            bool HasData = false;
            DataTable trainData = new DataTable();
            trainData.TableName = "Train";
            DataTable testData = new DataTable();
            testData.TableName = "Test";

            //讀檔
            string dataPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data.csv");
            DataTable dt;
            using (FileStream fs = new FileStream(dataPath, FileMode.Open))
            {
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                dt = GetData(sr);
            }
            //留一份原始資料
            SourceData = dt.Copy();

            #region Pre-ProcessData

            dt = RemoveColumns(dt);
            //拆解類別欄位，現已沒用
            //dt = BreakData(dt);
            dt = Normalization(dt);

            Fuzzy(dt);

            if (!HasData)
            {


                //測試資料百分比
                double testDataPercentage = double.Parse(txt_TestPercentage.Text);
                dt.Columns.Add("TempIndex");

                trainData = dt.Clone();
                testData = dt.Clone();
                trainData.TableName = "Train";
                testData.TableName = "Test";


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

            }
            #endregion


            ClusteringSetting setting;
            //取得分群的設定
            if (GetClusteringSetting(clusterCenter, clusterIndex, out setting))
            {
                Dictionary<int, GroupInfo> groupData = new Dictionary<int, GroupInfo>(); ;
                HasData = false;

                if (!HasData)
                {
                    //建立分群器
                    Clustering clustering = new Clustering(setting);

                    //分群結果
                    groupData = clustering.DoClustering(trainData, 3);

                }
                //預測
                Forecasting(clusterCenter, forecastCenter, clusterIndex, forecastIndex, testData, groupData);
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
            //removeColumns.Add("Date");
            removeColumns.Add("Split");
            //removeColumns.Add("Season");
            removeColumns.Add("Week");
            removeColumns.Add("Is_Holiday");
            removeColumns.Add("Is_Work");

            foreach (var item in lb_Field.Items)
            {
                var checkItem = ((EnergyColumn)item);
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


        private DataTable Normalization(DataTable dt)
        {
            DataTable result = dt;
            Dictionary<string, MaxMinValue> norCol;

            if (cb_normalization.IsChecked.Value)
            {
                result = CommonUtil.Normalization(dt, out norCol);
            }
            else
            {
                CommonUtil.SetMaxMin(dt, out norCol);
            }
            Parameter.GetInstance().NorCol = norCol;
            return result;
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



        private void Fuzzy(DataTable dt)
        {
            if (cb_fuzzy.IsChecked.Value)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName.Equals("RowIndex") || column.ColumnName.Equals("Season"))
                    {
                        continue;
                    }
                    var collection = Parameter.GetInstance().Items.Where(item => item.Value.Equals(column.ColumnName));
                    if (collection.Any())
                    {
                        List<HelfRegion> helfRegion = CommonUtil.GenHelfRegion(collection.FirstOrDefault(), dt);
                        Parameter.GetInstance().FuzzyCol.Add(column.ColumnName, helfRegion);
                    }
                }
            }
        }
        #endregion

        #region Forecast

        private void Forecasting(GroupCenter clusterCenter, GroupCenter forecastCenter, double clusterIndex, double forecastIndex, DataTable testData, Dictionary<int, GroupInfo> groupData)
        {
            PredictionCount = int.Parse(txt_PredictionCount.Text);

            //記錄結果
            DataTable result = new DataTable();
            result.Columns.Add("RowIndex");
            result.Columns.Add("OrginalValue");
            result.Columns.Add("TargetValue");
            result.Columns.Add("ForecastValue");


            double electricityValue = double.MinValue;
            List<double> forecastParams = new List<double>();
            forecastParams.Add(-1.1);
            forecastParams.Add(0);

            PearsonDistence predictionPearsonDis = new PearsonDistence(forecastParams);

            forecastParams = new List<double>();
            forecastParams.Add(99);
            forecastParams.Add(clusterIndex);
            forecastParams.Add(0);
            HammingDistence predictionHammingDis = new HammingDistence(forecastParams);

            //EuclideanDistence predictionDis = new EuclideanDistence(0);

            double similarityValue = double.MinValue;
            ClusteringSetting forecastSetting;
            if (!GetForecastClusteringSetting(forecastCenter, forecastIndex, out forecastSetting))
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
            TimeSpan targetTime;
            //testData.Columns.Remove("Target_Kwh");
            foreach (DataRow dr in testData.Rows)
            {
                targetTime = DateTime.Parse(dr["Date"].ToString()).TimeOfDay;
#if(DEBUG)
                showData.ImportRow(dr);
#endif

                if (PredictionCount > 0 && count > PredictionCount)
                {
                    break;
                }
                count++;
                if (double.TryParse(dr["Rel_kWh"].ToString(), out electricityValue))
                {

                    //找出與該點相似的群
                    Dictionary<int, GroupInfo> forecastingGroupData = clustering.FilterGroup(dr, groupData);
                    //double tempForecastIndex = forecastIndex;
                    //while (forecastingGroupData.Count == 0)//&& tempForecastIndex <= forecastIndex + 1)
                    //{
                    //    tempForecastIndex++;
                    //    ClusteringSetting tempForecastSetting;
                    //    GetForecastClusteringSetting(forecastCenter, tempForecastIndex, out tempForecastSetting);
                    //    Clustering TempClustering = new Clustering(tempForecastSetting);
                    //    forecastingGroupData = TempClustering.FilterGroup(dr, groupData);
                    //}

                    //計算出該點與每個群的相似度
                    //todo weight應該可以再調整
                    Dictionary<int, double> weight = CaculateWeight(forecastingGroupData);

                    //每一群該點與每個點相似度
                    Dictionary<int, List<double>> similarityGroupData = new Dictionary<int, List<double>>();


                    Dictionary<int, List<double>> similarityGroupDataElec = new Dictionary<int, List<double>>();
                    double forecastValue = forecastingGroupData.Count == 0 ? -66666 : electricityValue;
                    //double forecastValue = 0;

                    double forecastValueTemp = 0;
                    List<double> similarityData = new List<double>();
                    foreach (KeyValuePair<int, GroupInfo> item in forecastingGroupData)
                    {

                        //var dateColl =item.Value.Rows.Where(arg =>
                        //    {
                        //        return DateTime.Parse(arg["Date"].ToString()).TimeOfDay.TotalMinutes >= targetTime.TotalMinutes - 15 && DateTime.Parse(arg["Date"].ToString()).TimeOfDay.TotalMinutes <= targetTime.TotalMinutes + 15;
                        //    });
                        //if (!dateColl.Any())
                        //{
                        //    forecastValueTemp = item.Value.ColAvg["Target_Kwh"];
                        //}
                        //else
                        //{
                        //    forecastValueTemp = dateColl.Average(arg => double.Parse(arg["Target_Kwh"].ToString()));
                        //}
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
                        //DataRow groupCalculate = showData.NewRow();
                        //foreach (DataColumn col in testData.Columns)
                        //{
                        //    if (item.Value.ColCaculate.ContainsKey(col.ColumnName))
                        //    {
                        //        groupCalculate[col.ColumnName] = item.Value.ColCaculate[col.ColumnName];
                        //    }
                        //}
                        //groupCalculate["RowIndex"] = "GroupID:" + item.Key;
                        //showData.Rows.Add(groupCalculate);
#endif


                        //該點於該群的每一點相似度
                        foreach (DataRow gDr in item.Value.Rows)
                        {
                            TimeSpan currentTime = DateTime.Parse(gDr["Date"].ToString()).TimeOfDay;
                            //bool isCurrent = (currentTime.TotalMinutes >= targetTime.TotalMinutes - 15 && currentTime.TotalMinutes <= targetTime.TotalMinutes + 15);
                            if (predictionHammingDis.Caculate(gDr, dr, out similarityValue))
                            //if (predictionPearsonDis.Caculate(gDr, dr, out similarityValue))
                            {
                                //for pearson
                                //if(similarityValue < 0.9995)
                                //{
                                //    similarityValue = 0;
                                //}

                                //for hamming
                                if (similarityValue < 1 && similarityValue > 0)
                                {
                                    //if (!isCurrent)
                                    //{
                                    similarityValue = 0;
                                    //}
                                }
                                else if (similarityValue == 1)
                                {
                                    //if (isCurrent)
                                    //{
                                    //    similarityValue = similarityValue * 1;
                                    //}
                                    //else
                                    //{
                                    similarityValue = similarityValue * 1;
                                    //}
                                }
                                else if (similarityValue == 0)// && isCurrent)
                                {
                                    //similarityValue = 1;
                                }



                                if (!similarityGroupData.ContainsKey(item.Key))
                                {
                                    similarityGroupData.Add(item.Key, new List<double>());
                                }
                                if (!similarityGroupDataElec.ContainsKey(item.Key))
                                {
                                    similarityGroupDataElec.Add(item.Key, new List<double>());
                                }

                                //similarityGroupData[item.Key].Add(similarityValue);
                                if (similarityValue > 0 )//&& isCurrent)
                                {
                                    double elec;
                                    if (double.TryParse(gDr["Target_Kwh"].ToString(), out elec))
                                    {
                                        similarityData.Add(elec);

                                        similarityGroupData[item.Key].Add(similarityValue);

                                        similarityGroupDataElec[item.Key].Add(elec);
                                    }
                                }

#if(DEBUG)
                                if (similarityValue > 0 )//&& isCurrent)
                                {
                                    //印內容
                                    DataRow showRow = showData.NewRow();
                                    foreach (DataColumn col in testData.Columns)
                                    {
                                        showRow[col.ColumnName] = gDr[col.ColumnName];
                                    }
                                    showRow["Similarity"] = similarityValue;
                                    showData.Rows.Add(showRow);
                                }
#endif

                            }
                        }

                        //var tt = similarityGroupData[item.Key].Aggregate(1.0, (x, y) => x * y);
                        //double minSimilarity = similarityGroupData[item.Key].Min();
                        //double maxSimilarity = similarityGroupData[item.Key].Max();
                        double sumSimilarity = similarityGroupData[item.Key].Sum();
                        sumSimilarity = (sumSimilarity == 0 ? 1 : sumSimilarity);
                        //for (int i = 0; i < similarityGroupData[item.Key].Count; i++)
                        //{
                        //    //similarityGroupData[item.Key][i] = (similarityGroupData[item.Key][i] - minSimilarity) / (maxSimilarity - minSimilarity);
                        //    similarityGroupData[item.Key][i] = (similarityGroupData[item.Key][i]) / (sumSimilarity);
                        //}
                        //sumSimilarity = similarityGroupData[item.Key].Sum();
                        //sumSimilarity = (sumSimilarity == 0 ? 1 : sumSimilarity);

#if(DEBUG)
                        //印內容 用不到了
                        //int sCount = similarityGroupData[item.Key].Count - 1;
                        //for (int i = showData.Rows.Count - 1; i > showData.Rows.Count - 1 - similarityGroupData[item.Key].Count; i--)
                        //{
                        //    showData.Rows[i]["SimilarityNor"] = similarityGroupData[item.Key][sCount];
                        //    sCount--;
                        //}
#endif
                        double similarityTemp = 0;
                        for (int i = 0; i < similarityGroupData[item.Key].Count; i++)
                        {
                            similarityTemp += similarityGroupData[item.Key][i] * (similarityGroupDataElec[item.Key][i] - electricityValue);
                        }
                        ///forecastValueTemp = forecastValueTemp * weight[item.Key];
                        ///
                        forecastValueTemp += weight[item.Key] * (similarityTemp / sumSimilarity);

                        forecastValue += forecastValueTemp;


                        //forecastValue += weight[item.Key] * (item.Value.ColAvg["Target_Kwh"] + ((similarityGroupDataElec[item.Key].Sum()) / similarityGroupData[item.Key].Sum()));
                        //forecastValue += weight[item.Key] * (similarityTemp);
                    }



                    //if (similarityData.Count == 0)
                    //{
                    //    forecastValue = -66666;
                    //}
                    //else
                    //{
                    //    forecastValue = similarityData.Average() + forecastValueTemp;

                    //}
#if(DEBUG)
                    //寫入該筆的預測結果
                    var sourceRow = showData.AsEnumerable().Where(i => i["RowIndex"].ToString().Equals(dr["RowIndex"].ToString())).FirstOrDefault();
                    if (sourceRow != null)
                    {
                        sourceRow["Similarity"] = forecastValue;
                    }
                    showData.Rows.Add(showData.NewRow());
#endif

                    //抓出目標值
                    double targetValue = double.MinValue;
                    var targetValueStr = SourceData.AsEnumerable().Where(item => item["RowIndex"].ToString().Equals(dr["RowIndex"].ToString())).Select(item => item["Target_Kwh"].ToString()).FirstOrDefault();

                    //原值
                    var relValueStr = SourceData.AsEnumerable().Where(item => item["RowIndex"].ToString().Equals(dr["RowIndex"].ToString())).Select(item => item["Rel_kWh"].ToString()).FirstOrDefault();

                    //記錄下預測結果
                    if (double.TryParse(targetValueStr, out targetValue))
                    {
                        DataRow rDr = result.NewRow();
                        rDr["RowIndex"] = dr["RowIndex"].ToString();
                        rDr["OrginalValue"] = double.Parse(relValueStr);
                        rDr["ForecastValue"] = (cb_normalization.IsChecked.Value ? forecastValue * (Parameter.GetInstance().NorCol["Rel_kWh"].Max - Parameter.GetInstance().NorCol["Rel_kWh"].Min) + Parameter.GetInstance().NorCol["Rel_kWh"].Min : forecastValue);
                        rDr["TargetValue"] = targetValue;
                        result.Rows.Add(rDr);
                    }
                }
            }

            var rowCount = result.AsEnumerable().Where(item => double.Parse(item["ForecastValue"].ToString()) > 0).Count();
            var maeValue = result.AsEnumerable().Where(item => double.Parse(item["ForecastValue"].ToString()) > 0).Sum(item => Math.Abs(double.Parse(item["TargetValue"].ToString()) - double.Parse(item["ForecastValue"].ToString()))) / rowCount;
            var mseValue = result.AsEnumerable().Where(item => double.Parse(item["ForecastValue"].ToString()) > 0).Sum(item => Math.Pow(double.Parse(item["TargetValue"].ToString()) - double.Parse(item["ForecastValue"].ToString()), 2)) / rowCount;
            var rmseValue = Math.Sqrt(mseValue);

            DataRow mse = result.NewRow();
            mse["RowIndex"] = -1;
            mse["OrginalValue"] = "MAE:" + maeValue;
            mse["TargetValue"] = "MSE:" + mseValue;
            mse["ForecastValue"] = "RMSE:" + rmseValue;

            result.Rows.InsertAt(mse, 0);

            //存檔
            SaveToCSV(result, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + clusterCenter.ToString() + "_" + forecastCenter.ToString() + "_" + rmseValue.ToString("#.###") + "_clu" + clusterIndex + "_fore" + forecastIndex + ".csv"));

#if(DEBUG)
            SaveToCSV(showData, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMddHHmmss_Da") + ".csv"));
#endif


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

        private bool GetForecastClusteringSetting(GroupCenter forecastCenter, double forecastIndex, out ClusteringSetting forecastSetting)
        {
            bool result = true;
            forecastSetting = new ClusteringSetting();
            forecastSetting.GroupCenter = forecastIndex > 0 ? forecastCenter : GroupCenter.Avg;
            forecastSetting.SimilarityMethod = SimilarityMethod.HammingDistence;
            forecastSetting.ThresholdList.Add(0);
            forecastSetting.Fuzzy = cb_fuzzy.IsChecked.Value;
            if (cb_fuzzy.IsChecked.Value)
            {
                forecastSetting.ThresholdList.Add(0);
            }
            else
            {
                forecastSetting.ThresholdList.Add(forecastIndex > 0 ? forecastIndex : 10);
            }
            forecastSetting.ThresholdList.Add(0);

            //forecastSetting.GroupCenter = GroupCenter.Avg;
            //forecastSetting.SimilarityMethod = SimilarityMethod.PearsonDistence;
            //forecastSetting.ThresholdList.Add(0.95);

            return result;
        }

        private bool GetClusteringSetting(GroupCenter clusterCenter, double clusterIndex, out ClusteringSetting setting)
        {
            bool result = true;
            setting = new ClusteringSetting();
            setting.Fuzzy = cb_fuzzy.IsChecked.Value;
            try
            {
                if (clusterIndex > 0)
                {
                    setting = new ClusteringSetting();
                    setting.GroupCenter = clusterCenter;
                    setting.SimilarityMethod = SimilarityMethod.HammingDistence;
                    setting.ThresholdList.Add(0);
                    setting.Fuzzy = cb_fuzzy.IsChecked.Value;
                    if (cb_fuzzy.IsChecked.Value)
                    {
                        setting.ThresholdList.Add(0);
                    }
                    else
                    {
                        setting.ThresholdList.Add(clusterIndex);
                    }
                }
                else
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

        public static bool ObjectSerialize(string fileName, object obj, int backupSec = 3600)
        {
            bool result = false;
            try
            {
                DateTime fileUpdateTime = DateTime.MinValue;
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                if (File.Exists(filePath))
                {
                    fileUpdateTime = File.GetLastWriteTime(filePath);
                }
                if (DateTime.Now.Subtract(fileUpdateTime).TotalSeconds > backupSec)
                {
                    Polenter.Serialization.SharpSerializer serializer = new Polenter.Serialization.SharpSerializer();
                    serializer.Serialize(obj, filePath);
                    //File.WriteAllText(filePath, EncryptDecrypt.Encrypt(File.ReadAllText(filePath)));
                }
                result = true;
            }
            catch (Exception e)
            {
            }
            return result;
        }

        public static bool ObjectDeserialize(string fileName, out object obj)
        {
            bool result = false;
            obj = null;
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                if (File.Exists(filePath))
                {
                    //File.WriteAllText(filePath + ".temp", EncryptDecrypt.Decrypt(File.ReadAllText(filePath)));
                    Polenter.Serialization.SharpSerializer serializer = new Polenter.Serialization.SharpSerializer();
                    obj = serializer.Deserialize(filePath);
                    result = true;
                }
            }
            catch (Exception e)
            {
            }
            return result;
        }

        private void cb_fuzzy_Checked(object sender, RoutedEventArgs e)
        {
            foreach (EnergyColumn cb in Parameter.GetInstance().Items)
            {
                cb.IsFuzzy = cb_fuzzy.IsChecked.Value;
            }
            lb_Field.Items.Refresh();
            if (cb_fuzzy.IsChecked.Value)
            {
                rb_Hamming.IsChecked = cb_fuzzy.IsChecked.Value;
                txt_HamThresholdErrorPercentage.Text = "0";
            }
            txt_HamThresholdErrorPercentage.IsEnabled = !cb_fuzzy.IsChecked.Value;
            rb_Eucidean.IsEnabled = !cb_fuzzy.IsChecked.Value;
            txt_EucThreshold.IsEnabled = !cb_fuzzy.IsChecked.Value;
            rb_Pearson.IsEnabled = !cb_fuzzy.IsChecked.Value;
            txt_PearsonThreshold.IsEnabled = !cb_fuzzy.IsChecked.Value;

            //lb_Field.ItemsSource = null;
            //lb_Field.ItemsSource = Items;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            lb_Field.Items.Refresh();
            //lb_Field.ItemsSource = null;
            //lb_Field.ItemsSource = Items;
        }



    }
}
