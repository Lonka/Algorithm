using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinqToExcel;

namespace WM_Fuzzy
{
    public partial class Form1 : Form
    {
        int trainPercentage = 70;

        List<EnergyColumn> energyColumns = new List<EnergyColumn>();
        List<Feature> features = new List<Feature>();
        string forecastColumnName = "Forecast_Total_kWh";

        public Form1()
        {
            InitializeComponent();


            //Forecast();

        }

        private void Forecast()
        {
            energyColumns = new List<EnergyColumn>();
            features = new List<Feature>();
            // 對應Excel的欄位名，把目標欄位放最後一個
            //energyConsumption.Add(new EnergyConsumption("Date", 3));
            //energyConsumption.Add(new EnergyConsumption("Insolation", 3));
            //energyConsumption.Add(new EnergyConsumption("Humidity", 3));
            //energyConsumption.Add(new EnergyConsumption("Wind_Direction", 3));
            //energyConsumption.Add(new EnergyConsumption("Wind_Velocity", 3));
            //energyConsumption.Add(new EnergyConsumption("Air_kWh", 3));
            //energyConsumption.Add(new EnergyConsumption("Total_kWh", 3));
            energyColumns.Add(new EnergyColumn("Temperature", "溫度", int.Parse(txt_TempCount.Text)));
            energyColumns.Add(new EnergyColumn("Comfort", "舒適度", int.Parse(txt_ComCount.Text)));
            energyColumns.Add(new EnergyColumn("Air_Relatively_kWh", "空調每日用電", int.Parse(txt_AirCount.Text)));
            energyColumns.Add(new EnergyColumn("Total_Relatively_kWh", "總用電", int.Parse(txt_TotalCount.Text)));
            DataTable resultData = new DataTable();



            string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EnergyComsumptionData.xlsx");
            var excelFile = new ExcelQueryFactory(fileName);
            excelFile.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace;
            var excelSheet = excelFile.Worksheet("Energy Comsumption");

            #region 設定training及test data的數量
            int totalCount = excelSheet.Count();
            int trainCount = totalCount * trainPercentage / 100;
            int testCount = totalCount - trainCount;
            #endregion

            #region 設定training data及test data
            var trainData = excelSheet.Take(trainCount);
            DateTime trainLastDate = DateTime.Now;
            DateTime.TryParse(trainData.Last()["Date"], out trainLastDate);
            var testData = excelSheet.AsEnumerable().Where(item =>
            {
                DateTime date = DateTime.Now;
                DateTime.TryParse(item["Date"].ToString(), out date);
                return date > trainLastDate;
            });
            #endregion

            #region 產生各feature的Region
            int energyCount = 0;
            foreach (EnergyColumn ec in energyColumns)
            {
                resultData.Columns.Add(ec.Name);
                List<HelfRegion> helfRegions = GenHelfRegion(ec, excelSheet);
                List<Region> regions = GenRegion(helfRegions);
                features.Add(new Feature()
                {
                    FeatureName = ec.Name,
                    HelfRegions = helfRegions,
                    Regions = regions,
                    IsTargetValue = (energyCount == energyColumns.Count - 1 ? true : false)
                });
                energyCount++;
            }
            resultData.Columns.Add(forecastColumnName);
            #endregion

            #region 算出所有training data的rule
            Dictionary<int, RuleType> ruleBase = new Dictionary<int, RuleType>();
            int ruleIndex = 0;
            foreach (var dataRow in trainData)
            {
                List<RuleFeature> ruleFeatures = new List<RuleFeature>();

                string rule = string.Empty;
                foreach (Feature feature in features)
                {
                    double value = 0;
                    double.TryParse(dataRow[feature.FeatureName].ToString(), out value);
                    HelfRegion helfRegion = GetMembershipFunction(value, feature.HelfRegions);
                    double mfValue = helfRegion.CalculateMfValue(value);
                    ruleFeatures.Add(new RuleFeature()
                    {
                        MFValue = mfValue,
                        RegionName = helfRegion.RegionName,
                        Feature = feature
                    });
                    if (!feature.IsTargetValue)
                    {
                        if (helfRegion == null)
                        {
                            rule += "-";
                        }
                        rule += helfRegion.RegionName;
                    }
                    else
                    {
                        ruleBase.Add(ruleIndex, new RuleType(rule, helfRegion.RegionName, ruleFeatures));
                    }
                }
                ruleIndex++;
            }
            #endregion

            #region filter rule 全相同的
            ruleBase = ruleBase.Distinct(item => item.Value.XRule + item.Value.YRule).ToDictionary(item => item.Key, item => item.Value);
            #endregion

            dataGridView1.DataSource = ruleBase.Select(item => new
            {
                RuleBase = item.Value.XRule.ToString() + item.Value.YRule.ToString(),
                //itemX = item.Value.XRule,
                //Value = DegreeLevel(item.Value.RuleFeature)
            }).ToList();

            #region filter x rule相同的by相似度
            var groupXRule = ruleBase.GroupBy(item => item.Value.XRule).Select(item => new
            {
                Key = item.Key,
                Value = item
            });
            ruleBase = new Dictionary<int, RuleType>();
            foreach (var item in groupXRule)
            {
                double maxMF = 0;
                double tempMF = 0;
                int ruleId = 0;
                RuleType ruleType = null;
                foreach (var gItem in item.Value)
                {
                    tempMF = DegreeLevel(gItem.Value.RuleFeature);
                    if (tempMF > maxMF)
                    {
                        maxMF = tempMF;
                        ruleId = gItem.Key;
                        ruleType = gItem.Value;
                    }
                }
                ruleBase.Add(ruleId, ruleType);
            }
            #endregion

            //dataGridView2.DataSource = ruleBase.Select(item => new
            //    {
            //        item = item.Value.XRule.ToString() + item.Value.YRule.ToString(),
            //        itemX = item.Value.XRule,
            //        Value = DegreeLevel(item.Value.RuleFeature)
            //    }).ToList();

            #region Start Forecast

            List<Forecast> fs = new List<Forecast>();
            foreach (var dataRow in testData)
            {
                DataRow dr = resultData.NewRow();
                // list<> rule的mf值
                List<double> amds = new List<double>();
                // list<> rule的mf*中心值
                List<double> amdMclCenterVals = new List<double>();
                foreach (var rule in ruleBase)
                {
                    //list<> mf值
                    List<double> mfs = new List<double>();
                    //list<> target region 中心值
                    double targetCenterVal = 0;
                    foreach (var ruleFeature in rule.Value.RuleFeature)
                    {
                        Feature feature = ruleFeature.Feature;
                        double value = 0;
                        double.TryParse(dataRow[feature.FeatureName].ToString(), out value);
                        dr[feature.FeatureName] = value;
                        if (feature.IsTargetValue)
                        {
                            // 抓出target region的中心值 並存在tg list
                            targetCenterVal = ruleFeature.Feature.GetCenterValueByRegionName(ruleFeature.RegionName); //todo 算中心值    
                        }
                        else
                        {
                            // 算出value在該region的mf值 ruleFeature.RegionName && value 並存入mf list
                            double mfValue = CalculateMfValue(value, feature.HelfRegions, ruleFeature.RegionName);
                            mfs.Add(mfValue);
                        }
                    }
                    // 把mf list值 乘起來 並放入 rule mf list
                    double mclMfsValue = DegreeLevel(mfs);
                    amds.Add(mclMfsValue);
                    // 把上面乘好的mf值再去乘中心值 並放入 rule * mf list
                    amdMclCenterVals.Add(mclMfsValue * targetCenterVal);

                }
                //sum(rule的mf*中心值) / sum(rule的mf值) 就會是預測值
                double forecastValue = amdMclCenterVals.Sum() / amds.Sum();
                dr[forecastColumnName] = forecastValue;
                resultData.Rows.Add(dr);
                //fs.Add(new Forecast()
                //{
                //    before = double.Parse(dataRow["Total_Relatively_kWh"].ToString()),
                //    after = forecastValue
                //});

            }

            dataGridView2.DataSource = resultData;
            chart1.DataSource = resultData;
            chart1.DataBind();
            #endregion

        }

        private double CalculateMfValue(double value, List<HelfRegion> helfRegions, string regionName)
        {
            double mfValue = 0;
            var helfRegionsByValue = helfRegions.Where(item => value < item.MaxValue && value >= item.MinValue && item.RegionName.Equals(regionName)).FirstOrDefault();
            if (helfRegionsByValue != null)
            {
                mfValue = helfRegionsByValue.CalculateMfValue(value);
            }
            return mfValue;
        }

        private double DegreeLevel(List<RuleFeature> ruleMfList)
        {
            double result = 1;
            foreach (var mfItem in ruleMfList)
            {
                result *= mfItem.MFValue;
            }
            return result;
        }

        private double DegreeLevel(Dictionary<string, double> mfItemList)
        {
            double result = 1;
            foreach (var mfItem in mfItemList)
            {
                result *= mfItem.Value;
            }
            return result;
        }

        private double DegreeLevel(List<double> mfValueList)
        {
            double result = 1;
            foreach (double mfValue in mfValueList)
            {
                result *= mfValue;
            }
            return result;
        }

        private HelfRegion GetMembershipFunction(double value, List<HelfRegion> helfRegions)
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

        private List<HelfRegion> GenHelfRegion(EnergyColumn ec, LinqToExcel.Query.ExcelQueryable<Row> excelSheet)
        {
            double maxValue = excelSheet.AsEnumerable().Max(item =>
                {
                    double value = 0;
                    double.TryParse(item[ec.Name].ToString(), out value);
                    return value;
                });
            double minValue = excelSheet.AsEnumerable().Min(item =>
                {
                    double value = 0;
                    double.TryParse(item[ec.Name].ToString(), out value);
                    return value;
                });

            double step = (maxValue - minValue) / (ec.RegionCount - 1);
            List<HelfRegion> regions = new List<HelfRegion>();
            int regionNameChar = 65;

            double tempValue = minValue;
            for (double i = 0; i < ec.RegionCount - 1; i += 1)
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

        private List<Region> GenRegion(List<HelfRegion> helfRegions)
        {
            List<Region> result = new List<Region>();
            double tempMin = 0;
            int i = 0;
            foreach (HelfRegion range in helfRegions)
            {
                Region region = new Region();
                if (i == 0 || range.MfDirection == HelfRegion.Direction.left)
                    tempMin = range.MinValue;
                if (range.MfDirection == HelfRegion.Direction.right)
                {
                    region.RegionName = range.RegionName;
                    region.MinValue = tempMin;
                    region.MaxValue = range.MaxValue;
                    region.CenterValue = region.CalculateCenterValue((tempMin.Equals(range.MinValue) ? "right" : "triangle"));
                    result.Add(region);
                }
                if (i == helfRegions.Count - 1)
                {
                    region.RegionName = range.RegionName;
                    region.MinValue = range.MinValue;
                    region.MaxValue = range.MaxValue;
                    region.CenterValue = region.CalculateCenterValue("left");
                    result.Add(region);
                }
                i++;
            }
            return result;
        }

        private void btn_Forecast_Click(object sender, EventArgs e)
        {
            Forecast();
        }

    }

    public static class EnumerableExtender
    {
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
        {
            return source.GroupBy(selector).Select(item => item.First());
        }
        //public static IEnumerable<T> DistinctMFValue<T, TKey>(this IEnumerable<T> source, Func<T,TKey > selector)
        //{
        //    return source.GroupBy( ).Select(item => item.First());
        //}

        //   public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        //(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        //   {
        //       HashSet<TKey> knownKeys = new HashSet<TKey>();
        //       foreach (TSource element in source)
        //       {
        //           if (knownKeys.Add(keySelector(element)))
        //           {
        //               yield return element;
        //           }
        //       }
        //   }



        //private sealed class Impl<T> : IEqualityComparer<T, T>
        //{
        //    private Func<T, T, bool> m_del;
        //    private IEqualityComparer<T> m_comp;
        //    public Impl(Func<T, T, bool> del)
        //    {
        //        m_del = del;
        //        m_comp = EqualityComparer<T>.Default;
        //    }
        //    public bool Equals(T left, T right)
        //    {
        //        return m_del(left, right);
        //    }
        //    public int GetHashCode(T value)
        //    {
        //        return m_comp.GetHashCode(value);
        //    }
        //}
        //public static IEqualityComparer<T, T> Create<T>(Func<T, T, bool> del)
        //{
        //    return new Impl<T>(del);
        //}
    }



}
