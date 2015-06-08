using LinqToExcel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace BackPropagationNeuralNetwork
{
    public partial class ElectricityForecast : Form
    {
        static List<Neural> ms_neural = null;
        int m_trainPercentage = 70;
        string m_excelFileName = "EnergyComsumptionData.csv";
        string m_excelSheetName = "Energy Comsumption";
        List<EnergyColumn> energyColumns = null;
        //多層暫不打算寫
        int m_heddenLayerSize = 1;
        int m_heddenNeuralSize = 5;
        int m_outputNeuralSize = 1;
        double m_learningRate = 0.7;
        double m_momentumFactor = 0.5;
        int m_trainLoopCount = 20;
        public ElectricityForecast()
        {
            InitializeComponent();

            //double xx = Math.Exp(-0.4);

            //double yy = 1 / (1 + xx);

            DataTable cbBindData = new DataTable();
            cbBindData.Columns.Add("Text");
            cbBindData.Columns.Add("Value");
            DataRow dr = cbBindData.NewRow();
            dr["Text"] = "高相關";
            dr["Value"] = "high";
            cbBindData.Rows.Add(dr);
            dr = cbBindData.NewRow();
            dr["Text"] = "中相關";
            dr["Value"] = "mid";
            cbBindData.Rows.Add(dr);
            dr = cbBindData.NewRow();
            dr["Text"] = "低相關";
            dr["Value"] = "low";
            cbBindData.Rows.Add(dr);


            cb_Total_Relatively_kWh.DataSource = cbBindData.Copy();
            cb_Total_Relatively_kWh.SelectedValue = "mid";

            cb_Total_kWh.DataSource = cbBindData.Copy();
            cb_Total_kWh.SelectedValue = "mid";

            cb_Avg_Temperature.DataSource = cbBindData;
            cb_Avg_Temperature.SelectedValue = "mid";

            cb_Avg_Illumination.DataSource = cbBindData.Copy();
            cb_Avg_Illumination.SelectedValue = "high";

            cb_Season.DataSource = cbBindData.Copy();
            cb_Season.SelectedValue = "low";

            cb_Work.DataSource = cbBindData.Copy();
            cb_Work.SelectedValue = "low";

            cb_Holiday.DataSource = cbBindData.Copy();
            cb_Holiday.SelectedValue = "low";
        }

        //類神精網路
        private void NeuralNetwork()
        {
            ms_neural = new List<Neural>();
            energyColumns = new List<EnergyColumn>();
            energyColumns.Add(new EnergyColumn("Total_kWh", "即時總用電", DataType.Continuous, cb_Total_kWh.SelectedValue.ToString()));
            energyColumns.Add(new EnergyColumn("Total_Relatively_kWh", "相對總用電", DataType.Continuous, cb_Total_Relatively_kWh.SelectedValue.ToString()));
            energyColumns.Add(new EnergyColumn("Avg_Temperature", "平均溫度", DataType.Continuous, cb_Avg_Temperature.SelectedValue.ToString()));
            energyColumns.Add(new EnergyColumn("Avg_Illumination", "平均日照量", DataType.Continuous, cb_Avg_Illumination.SelectedValue.ToString()));
            //energyColumns.Add(new EnergyColumn("Season", "季節", DataType.Category, cb_Season.SelectedValue.ToString()));
            //energyColumns.Add(new EnergyColumn("Holiday", "假日", DataType.Category, cb_Holiday.SelectedValue.ToString()));
            //energyColumns.Add(new EnergyColumn("Work", "工作", DataType.Category, cb_Work.SelectedValue.ToString()));
            energyColumns.Add(new EnergyColumn("Target", "目標電力", DataType.Continuous, string.Empty));

            //Load File
            //For All Data
            var excelSheet = LoadData();

            //For Filter Data
            var FilterData = excelSheet;
            /*.Where(
            item => (
            item["Season"].ToString().Equals("Summer")
             item["Season"].ToString().Equals("Spring")
             item["Season"].ToString().Equals("Winter")
             item["Season"].ToString().Equals("Fall")
            )

             item["Work"].ToString().Equals("TRUE")
            )*/
            ;

            //For All Data
            //FindRange(excelSheet);

            //For Filter Data
            FindRange(FilterData);

            #region 設定training及testing data
            //For All Data
            //int totalCount = excelSheet.Count();

            //For Filter Data
            int totalCount = FilterData.Count();
            int trainCount = totalCount * m_trainPercentage / 100;
            int testCount = totalCount - trainCount;
            //For All Data
            //var trainExcelData = excelSheet.Take(trainCount);
            //var testingExcelData = excelSheet.Skip(trainCount);

            //For Filter Data
            var trainExcelData = FilterData.Take(trainCount);
            var testingExcelData = FilterData.Skip(trainCount);
            #endregion

            //正規化
            DataTable trainData = Normalization(trainExcelData);
            DataTable testingData = Normalization(testingExcelData);

            //Initial Weight
            InitialWeight(trainData.Clone());

            #region training

            TrainingModel(trainData);

            #endregion

            //TODO del
            Dictionary<string, double> outputWeightShow = new Dictionary<string, double>();
            foreach (var xx in ms_neural)
            {
                foreach (var x in xx.Weights)
                {
                    outputWeightShow.Add(xx.Name + x.Name, x.Weight);
                }
            }
            dataGridView3.DataSource = outputWeightShow.ToList();


            #region testing

            DataTable forecastData = Testing(testingExcelData, testingData);
            dataGridView1.DataSource = forecastData;
            double mae = calculateMAE(forecastData, "Target", "Target_Forecast");
            lbl_Mae.Text = mae.ToString("#.#####");
            for (int i = 0; i < energyColumns.Count; i++)
            {
                dataGridView1.Columns[i].HeaderText = energyColumns[i].ChiName;
            }
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].HeaderText = "預測電力";
            #endregion

            chart1.DataSource = forecastData;
            chart1.DataBind();


        }

        private double calculateMAE(DataTable forecastData, string p1, string p2)
        {
            double sum = 0;
            double count = 0;
            foreach (DataRow dr in forecastData.Rows)
            {
                double forecastValue = 0;
                if (double.TryParse(dr[p2].ToString(), out forecastValue))
                {
                    if (!double.IsNaN(forecastValue))
                    {
                        sum += Math.Abs(double.Parse(dr[p1].ToString()) - forecastValue);
                        count++;
                    }
                }
            }
            return sum / count;
        }


        private DataTable Testing(IQueryable<Row> testingExcelData, DataTable testingData)
        {
            int rowIndex = 0;
            //每筆輸入值
            DataTable forecastData = new DataTable();
            foreach (EnergyColumn eng in energyColumns)
            {
                forecastData.Columns.Add(eng.Name);
                if (eng.Name.IndexOf("Target") > -1)
                {
                    forecastData.Columns.Add(eng.Name + "_Forecast");
                }
            }
            foreach (var row in testingExcelData)
            {
                DataRow dr = forecastData.NewRow();
                foreach (EnergyColumn eng in energyColumns)
                {
                    dr[eng.Name] = row[eng.Name];
                    if (eng.Name.IndexOf("Target") > -1)
                    {
                        //TODO 沒處理類別的輸出值及多個連續值
                        dr[eng.Name] = row[eng.Name];
                        Dictionary<string, double> outputValue = ForwardPropagation(testingData, rowIndex);
                        double realForecastValue = RestoreForecastValue(outputValue["Neural" + m_heddenLayerSize + "_" + eng.Name], eng);
                        dr[eng.Name + "_Forecast"] = realForecastValue;
                        continue;
                    }
                }
                forecastData.Rows.Add(dr);

                rowIndex++;
            }
            return forecastData;
        }

        private double RestoreForecastValue(double sourceValue, EnergyColumn eng)
        {
            return (sourceValue * (eng.MaxValue - eng.MinValue)) + eng.MinValue;
        }


        //暫存每個thread的輸出值
        static Dictionary<string, double> ms_tempOutputValues = null;
        //看thread結束狀態
        volatile static string ms_stopFlog = string.Empty;
        volatile static object stopLock = new object();
        static string ms_threadCount = string.Empty;
        private void TrainingModel(DataTable sourceData)
        {
            int i = 0;
            while (i < m_trainLoopCount)
            {
                //每筆輸入值
                for (int rowIndex = 0; rowIndex < sourceData.Rows.Count; rowIndex++)
                {
                    Dictionary<string, double> outputValue = ForwardPropagation(sourceData, rowIndex);
                    double effectValue = CalculateEffect(outputValue, sourceData.Rows[rowIndex]);
                    //門檻 break

                    BackPropagation(sourceData.Rows[rowIndex]);
                }
                i++;
            }
        }

        //算學習效率值，越小越好
        private double CalculateEffect(Dictionary<string, double> outputValue, DataRow dataRow)
        {
            var targetCol = dataRow.Table.Columns.Cast<DataColumn>().Where(item => item.ColumnName.IndexOf("Target") > -1).ToList();
            double result = 0;
            foreach (var col in targetCol)
            {
                result += Math.Pow(double.Parse(dataRow[col].ToString()) - outputValue["Neural" + (m_heddenLayerSize) + "_" + col.ColumnName], 2);
            }
            return result / 2;
        }

        //Back Propagation 修改權重
        private void BackPropagation(DataRow dataRow)
        {
            var layerList = ms_neural.AsEnumerable().Select(item => item.Layer).Distinct().OrderByDescending(item => item).ToList();
            //從後面開始回來算差距量修改
            for (int layer = 0; layer < layerList.Count(); layer++)
            {
                List<Neural> selNeural = ms_neural.Where(item => item.Layer.Equals(layerList[layer])).ToList();
                foreach (Neural neural in selNeural)
                {
                    double outputValue = neural.OutputValue;
                    //最後一層
                    if (layer == 0)
                    {
                        double targetValue = double.Parse(dataRow[neural.Name.Replace("Neural" + (m_heddenLayerSize) + "_", string.Empty)].ToString());
                        //算差距量
                        neural.GapValue = (targetValue - outputValue) * (outputValue * (1 - outputValue));
                    }
                    else
                    {
                        List<Neural> parentNeural = ms_neural.Where(item => item.Layer.Equals(layerList[layer - 1])).ToList();
                        double sum = 0;
                        foreach (Neural pNeural in parentNeural)
                        {
                            //pNeural.GapValue*
                            double toPerentWeight = pNeural.Weights.Where(item => item.Name.Equals("W_" + neural.Name)).FirstOrDefault().Weight;
                            sum += pNeural.GapValue * toPerentWeight;
                        }
                        neural.GapValue = (sum) * (outputValue * (1 - outputValue));
                        //算法不一樣
                    }
                }
            }
            //從後面開始回來算差距量修改
            for (int layer = 0; layer < layerList.Count(); layer++)
            {
                List<Neural> selNeural = ms_neural.Where(item => item.Layer.Equals(layerList[layer])).ToList();
                foreach (Neural neural in selNeural)
                {
                    //修正所有權重
                    foreach (NeuralWeight nWeight in neural.Weights)
                    {
                        double correctionValue = (m_learningRate * neural.GapValue * nWeight.SourceOutputValue) + (m_momentumFactor * nWeight.PreviousCorrectionValue);
                        nWeight.Weight += correctionValue;
                        nWeight.PreviousCorrectionValue = correctionValue;
                    }
                }
            }
        }

        #region Forward Propagation
        //前傳
        private Dictionary<string, double> ForwardPropagation(DataTable sourceData, int rowIndex)
        {
            #region 輸入層
            //輸出格式應為
            //{[Hedden00:0.21],[Hedden01:0.245]...}
            ms_tempOutputValues = new Dictionary<string, double>();
            ms_stopFlog = "".PadLeft(m_heddenNeuralSize, '0'); ;
            for (int heddenNeuralIndex = 0; heddenNeuralIndex < m_heddenNeuralSize; heddenNeuralIndex++)
            {
                //每個hedden都自己開thread做
                string heddenName = "Neural0_" + heddenNeuralIndex;
                ms_tempOutputValues.Add(heddenName, 0);
                StartInputThread(heddenNeuralIndex, sourceData.Rows[rowIndex], heddenName);
            }
            ms_threadCount = "".PadLeft(m_heddenNeuralSize, '1');
            //等所有的hedden thread跑完
            Dictionary<string, double> inputValue = WaitAndGenData();


            #endregion

            #region 隱藏層(目前用不到)
            if (m_heddenLayerSize > 1)
            {
                ms_stopFlog = "".PadLeft(m_heddenNeuralSize, '0'); ;
                ms_tempOutputValues = new Dictionary<string, double>();
                for (int heddenLayerIndex = 1; heddenLayerIndex < m_heddenLayerSize; heddenLayerIndex++)
                {
                    for (int heddenNeuralIndex = 0; heddenNeuralIndex < m_heddenNeuralSize; heddenNeuralIndex++)
                    {

                        //每個hedden都自己開thread做
                        string heddenName = "Neural" + heddenLayerIndex + "_" + heddenNeuralIndex;
                        ms_tempOutputValues.Add(heddenName, 0);
                        StartHeddenThread(heddenNeuralIndex, inputValue, heddenName);
                    }
                    ms_threadCount = "".PadLeft(m_heddenNeuralSize, '1');
                    //等所有的hedden thread跑完
                    inputValue = WaitAndGenData();
                }
            }
            #endregion

            #region 輸出層
            ms_tempOutputValues = new Dictionary<string, double>();
            var targetCol = sourceData.Columns.Cast<DataColumn>().Where(item => item.ColumnName.IndexOf("Target") > -1).ToList();
            ms_stopFlog = "".PadLeft(targetCol.Count, '0'); ;
            for (int outputIndex = 0; outputIndex < targetCol.Count; outputIndex++)
            {
                //每個hedden都自己開thread做
                string OutputName = "Neural" + m_heddenLayerSize + "_" + targetCol[outputIndex].ColumnName;
                ms_tempOutputValues.Add(OutputName, 0);
                StartHeddenThread(outputIndex, inputValue, OutputName);
            }
            ms_threadCount = "".PadLeft(targetCol.Count, '1');
            //等所有的hedden thread跑完
            inputValue = WaitAndGenData();


            #endregion

            return inputValue;
        }

        //開input層到隱藏層的thead
        private static void StartInputThread(int threadId, DataRow dr, string heddenName)
        {
            Input2HeddenThreadClass input2HeddenThreadClass = new Input2HeddenThreadClass();
            input2HeddenThreadClass.ThreadName = heddenName;
            input2HeddenThreadClass.InputData = dr;
            input2HeddenThreadClass.ThreadID = threadId;
            Thread heddenThread = new Thread(input2HeddenThreadClass.InputCombineAndSigmoid);
            heddenThread.Start();
        }

        //開隱藏層之後的thread
        private static void StartHeddenThread(int threadId, Dictionary<string, double> inputValue, string heddenName)
        {
            Hedden2OutputThreadClass hedden2OutputThreadClass = new Hedden2OutputThreadClass();
            hedden2OutputThreadClass.ThreadName = heddenName;
            hedden2OutputThreadClass.InputData = inputValue;
            hedden2OutputThreadClass.ThreadID = threadId;
            Thread heddenThread = new Thread(hedden2OutputThreadClass.CalculateOutputValue);
            heddenThread.Start();
        }

        //等所有的thread結束
        private static Dictionary<string, double> WaitAndGenData()
        {
            bool exit = false;
            while (!exit)
            {
                if (ms_stopFlog == ms_threadCount)
                {
                    exit = true;
                }
            }

            Dictionary<string, double> inputValue = new Dictionary<string, double>();
            foreach (KeyValuePair<string, double> item in ms_tempOutputValues)
            {
                inputValue.Add(item.Key, item.Value);
            }
            return inputValue;
        }

        //計算input層到隱藏層
        class Input2HeddenThreadClass
        {
            public string ThreadName { get; set; }
            public DataRow InputData { get; set; }
            public int ThreadID { get; set; }
            public void InputCombineAndSigmoid()
            {
                Neural neural = ms_neural.Where(item => item.Name.Equals(ThreadName)).FirstOrDefault();
                double sum = 0;
                foreach (NeuralWeight nWeight in neural.Weights)
                {
                    if (nWeight.Name.Equals("W_0"))
                    {
                        sum += nWeight.Weight;
                        nWeight.SourceOutputValue = 1;
                    }
                    else
                    {
                        double value = double.Parse(InputData[nWeight.Name.Replace("W_", string.Empty)].ToString());
                        sum += (nWeight.Weight * value);
                        nWeight.SourceOutputValue = value;
                    }
                }
                double result = 1 / (1 + Math.Exp(-sum));
                neural.OutputValue = result;
                ms_tempOutputValues[ThreadName] = result;
                lock (stopLock)
                {
                    ms_stopFlog = ms_stopFlog.Remove(ThreadID, 1);
                    ms_stopFlog = ms_stopFlog.Insert(ThreadID, "1");
                }
            }
        }

        //計算隱藏層之後的
        class Hedden2OutputThreadClass
        {
            public string ThreadName { get; set; }
            public Dictionary<string, double> InputData { get; set; }
            public int ThreadID { get; set; }
            public void CalculateOutputValue()
            {
                Neural neural = ms_neural.Where(item => item.Name.Equals(ThreadName)).FirstOrDefault();
                double sum = 0;
                foreach (NeuralWeight nWeight in neural.Weights)
                {
                    if (nWeight.Name.Equals("W_0"))
                    {
                        sum += nWeight.Weight;
                        nWeight.SourceOutputValue = 1;
                    }
                    else
                    {
                        double value = double.Parse(InputData[nWeight.Name.Replace("W_", string.Empty)].ToString());
                        sum += (nWeight.Weight * value);
                        nWeight.SourceOutputValue = value;
                    }
                }
                double result = 1 / (1 + Math.Exp(-sum));
                neural.OutputValue = result;
                ms_tempOutputValues[ThreadName] = result;
                lock (stopLock)
                {
                    ms_stopFlog = ms_stopFlog.Remove(ThreadID, 1);
                    ms_stopFlog = ms_stopFlog.Insert(ThreadID, "1");
                }
            }
        }

        #endregion

        /// <summary>
        /// 初始化各神經元的權重
        /// </summary>
        /// <param name="data"></param>
        private void InitialWeight(DataTable data)
        {
            //TODO 要刪
            Dictionary<string, double> weightShow = new Dictionary<string, double>();
            //Input -> Hedden的Weight
            for (int i = 0; i < m_heddenNeuralSize; i++)
            {
                List<NeuralWeight> nWeights = new List<NeuralWeight>();
                for (int j = 0; j <= data.Columns.Count; j++)
                {
                    if (j > 0 && data.Columns[j - 1].ColumnName.IndexOf("Target") > -1)
                    {
                        continue;
                    }
                    double weight = 0;
                    if (j > 0)
                    {
                        weight = GetWeight(data.Columns[j - 1].ColumnName);
                    }
                    nWeights.Add(new NeuralWeight()
                    {
                        //Weight = (j == 0 ? 1 - weight : weight),
                        Weight = weight,
                        Name = (j == 0 ? "W_0" : "W_" + data.Columns[j - 1].ColumnName)
                    });
                    weightShow.Add((j == 0 ? "Neural0_" + i + "W_0" : "Neural0_" + i + "W_" + data.Columns[j - 1].ColumnName), weight);
                }
                ms_neural.Add(new Neural() { Layer = 0, Name = "Neural0_" + i, Weights = nWeights });
            }

            //Hedden 1->n 的Weight(目前用不到)
            for (int s = 1; s < m_heddenLayerSize; s++)
            {
                for (int i = 0; i < m_heddenNeuralSize; i++)
                {
                    List<NeuralWeight> nWeights = new List<NeuralWeight>();
                    for (int j = 0; j <= m_heddenNeuralSize; j++)
                    {
                        Random r = new Random(Guid.NewGuid().GetHashCode());
                        double weight = r.Next(100, 999) / 1000.0;
                        nWeights.Add(new NeuralWeight()
                        {
                            //Weight = (j == 0 ? 1 - weight : weight),
                            Weight = weight,
                            Name = (j == 0 ? "W_0" : "W_Neural" + (s - 1) + "_" + i)
                        });
                        weightShow.Add((j == 0 ? "Neural" + s + "_" + i + "W_0" : "Neural" + s + "_" + i + "W_Neural" + (s - 1) + "_" + i), weight);
                    }
                    ms_neural.Add(new Neural() { Layer = s, Name = "Neural" + s + "_" + i, Weights = nWeights });

                }
            }

            //Hedden -> Output的Weight
            var targetCol = data.Columns.Cast<DataColumn>().Where(item => item.ColumnName.IndexOf("Target") > -1).ToList();
            for (int i = 0; i < targetCol.Count; i++)
            {
                List<NeuralWeight> nWeights = new List<NeuralWeight>();
                for (int j = 0; j <= m_heddenNeuralSize; j++)
                {
                    Random r = new Random(Guid.NewGuid().GetHashCode());
                    double weight = 0;
                    if (j > 0)
                    {
                        weight = r.Next(100, 999) / 1000.0;
                    }

                    nWeights.Add(new NeuralWeight()
                    {
                        //Weight = (j == 0 ? 1 - weight : weight),
                        Weight = weight,
                        Name = (j == 0 ? "W_0" : "W_Neural" + (m_heddenLayerSize - 1) + "_" + (j - 1))
                    });
                    weightShow.Add((j == 0 ? "Neural" + (m_heddenLayerSize) + "_" + targetCol[i].ColumnName + "tW_0" : "Neural" + (m_heddenLayerSize) + "_" + targetCol[i].ColumnName + "tW_Neural" + (m_heddenLayerSize - 1) + "_" + (j - 1)), weight);
                }
                ms_neural.Add(new Neural() { Layer = m_heddenLayerSize, Name = "Neural" + (m_heddenLayerSize) + "_" + targetCol[i].ColumnName, Weights = nWeights });

            }

            //TODO 要刪
            dataGridView2.DataSource = weightShow.ToList();
        }

        private double GetWeight(string colName)
        {
            string weightType = energyColumns.Where(item => colName.IndexOf(item.Name) > -1).OrderBy(item => item.Name).FirstOrDefault().WeightType;
            Random r = new Random(Guid.NewGuid().GetHashCode());
            double weight = -1;

            switch (weightType)
            {
                case "high":
                    weight = r.Next(700, 999) / 1000.0;
                    break;
                case "mid":
                    weight = r.Next(400, 699) / 1000.0;

                    break;
                case "low":
                    weight = r.Next(100, 399) / 1000.0;
                    break;
            }
            return weight;
        }

        /// <summary>
        /// 從csv讀出資料
        /// </summary>
        /// <returns></returns>
        private LinqToExcel.Query.ExcelQueryable<Row> LoadData()
        {
            string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, m_excelFileName);
            var excelFile = new ExcelQueryFactory(fileName);
            excelFile.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace;
            var excelSheet = excelFile.Worksheet(m_excelSheetName);
            return excelSheet;
        }

        private void FindRange(LinqToExcel.Query.ExcelQueryable<Row> excelSheet)
        {
            #region 找出最大最小及類別值
            foreach (var row in excelSheet)
            {
                foreach (EnergyColumn eng in energyColumns)
                {
                    if (eng.DataType == DataType.Continuous)
                    {
                        double value = 0;
                        if (double.TryParse(row[eng.Name], out value))
                        {
                            if (value > eng.MaxValue)
                            {
                                eng.MaxValue = value;
                            }
                            if (value < eng.MinValue)
                            {
                                eng.MinValue = value;
                            }
                        }
                    }
                    else if (eng.DataType == DataType.Category)
                    {
                        string categoryStr = row[eng.Name];
                        if (!eng.CategoryList.Contains(categoryStr))
                        {
                            eng.CategoryList.Add(categoryStr);
                        }
                    }
                }
            }

            //移除沒有類別的欄位
            List<EnergyColumn> removeEng = new List<EnergyColumn>();
            foreach (EnergyColumn eng in energyColumns)
            {
                if (eng.DataType == DataType.Category && eng.CategoryList.Count == 0)
                {
                    removeEng.Add(eng);
                }
            }
            foreach (EnergyColumn rmEng in removeEng)
            {
                energyColumns.Remove(rmEng);
            }
            #endregion
        }

        private void FindRange(IQueryable<Row> FilterData)
        {
            #region 找出最大最小及類別值
            foreach (var row in FilterData)
            {
                foreach (EnergyColumn eng in energyColumns)
                {
                    if (eng.DataType == DataType.Continuous)
                    {
                        double value = 0;
                        if (double.TryParse(row[eng.Name], out value))
                        {
                            if (value > eng.MaxValue)
                            {
                                eng.MaxValue = value;
                            }
                            if (value < eng.MinValue)
                            {
                                eng.MinValue = value;
                            }
                        }
                    }
                    else if (eng.DataType == DataType.Category)
                    {
                        string categoryStr = row[eng.Name];
                        if (!eng.CategoryList.Contains(categoryStr))
                        {
                            eng.CategoryList.Add(categoryStr);
                        }
                    }
                }
            }

            //移除沒有類別的欄位
            List<EnergyColumn> removeEng = new List<EnergyColumn>();
            foreach (EnergyColumn eng in energyColumns)
            {
                if (eng.DataType == DataType.Category && eng.CategoryList.Count == 0)
                {
                    removeEng.Add(eng);
                }
            }
            foreach (EnergyColumn rmEng in removeEng)
            {
                energyColumns.Remove(rmEng);
            }
            #endregion
        }


        /// <summary>
        /// 正規化
        /// </summary>
        /// <param name="excelSheet"></param>
        /// <returns></returns>
        private DataTable Normalization(IQueryable<Row> excelSheet)
        {

            #region Initial Table
            DataTable data = new DataTable();
            foreach (EnergyColumn eng in energyColumns)
            {
                if (eng.DataType == DataType.Continuous)
                {
                    data.Columns.Add(eng.Name);
                }
                else if (eng.DataType == DataType.Category)
                {
                    if (eng.CategoryList.Count <= 2)
                    {
                        data.Columns.Add(eng.Name);
                    }
                    else
                    {
                        foreach (string categoryStr in eng.CategoryList)
                        {
                            data.Columns.Add(eng.Name + "_" + categoryStr);
                        }
                    }
                }
                if (eng.Name.Equals("Target"))
                {
                    if (eng.DataType == DataType.Continuous)
                    {
                        m_outputNeuralSize = 1;
                    }
                    else if (eng.DataType == DataType.Category)
                    {
                        if (eng.CategoryList.Count <= 2)
                        {
                            m_outputNeuralSize = 1;

                        }
                        else
                        {
                            m_outputNeuralSize = eng.CategoryList.Count;
                        }
                    }
                }
            }
            #endregion

            #region Normalization
            foreach (var row in excelSheet)
            {
                DataRow dr = data.NewRow();
                foreach (EnergyColumn eng in energyColumns)
                {
                    if (eng.DataType == DataType.Continuous)
                    {
                        double value = 0;
                        if (double.TryParse(row[eng.Name], out value))
                        {
                            dr[eng.Name] = NormalizationValue(eng.MaxValue, eng.MinValue, value);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (eng.DataType == DataType.Category)
                    {
                        //不可能會有沒類別的情況
                        string categoryValue = row[eng.Name];
                        if (eng.CategoryList.Count <= 2)
                        {
                            if (categoryValue == eng.CategoryList[0])
                            {
                                dr[eng.Name] = 1;
                            }
                            else
                            {
                                dr[eng.Name] = 0;
                            }
                        }
                        else
                        {
                            foreach (string categoryStr in eng.CategoryList)
                            {
                                if (categoryStr.Equals(categoryValue))
                                {
                                    dr[eng.Name + "_" + categoryStr] = 1;
                                }
                                else
                                {
                                    dr[eng.Name + "_" + categoryStr] = 0;
                                }
                            }
                        }
                    }
                }
                data.Rows.Add(dr);
            }
            #endregion
            return data;
        }

        private double NormalizationValue(double max, double min, double value)
        {
            if (value - min == 0)
            {
                return 0;
            }
            else
            {
                return (value - min) / (max - min);
            }
        }

        private void btn_Forecast_Click(object sender, EventArgs e)
        {
            m_learningRate = double.Parse(txt_LearningRate.Text);
            m_momentumFactor = double.Parse(txt_MomentumFactor.Text);
            NeuralNetwork();
        }

    }
}
