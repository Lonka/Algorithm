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
        static List<Neural> ms_neural = new List<Neural>();
        int m_trainPercentage = 70;
        string m_excelFileName = "EnergyComsumptionData.csv";
        string m_excelSheetName = "Energy Comsumption";
        List<EnergyColumn> energyColumns = null;
        //多層暫不打算寫
        int m_heddenLayerSize = 1;
        int m_heddenNeuralSize = 5;
        int m_outputNeuralSize = 1;
        double m_learningRate = 0.2;

        public ElectricityForecast()
        {
            InitializeComponent();

            //double xx = Math.Exp(-0.4);

            //double yy = 1 / (1 + xx);


            energyColumns = new List<EnergyColumn>();
            energyColumns.Add(new EnergyColumn("Avg_Temperature", "平均溫度", DataType.Continuous));
            energyColumns.Add(new EnergyColumn("Total_kWh", "即時總用電", DataType.Continuous));
            energyColumns.Add(new EnergyColumn("Total_Relatively_kWh", "相對總用電", DataType.Continuous));
            energyColumns.Add(new EnergyColumn("Avg_Illumination", "平均日照量", DataType.Continuous));
            energyColumns.Add(new EnergyColumn("Season", "季節", DataType.Category));
            energyColumns.Add(new EnergyColumn("Holiday", "假日", DataType.Category));
            energyColumns.Add(new EnergyColumn("Work", "工作", DataType.Category));
            energyColumns.Add(new EnergyColumn("Target", "預測電力", DataType.Continuous));

            //Load File
            var excelSheet = LoadData();

            //正規化
            DataTable data = Normalization(excelSheet);

            //Initial Weight
            InitialWeight(data);

            #region 設定training及testing data
            int totalCount = data.Rows.Count;
            int trainCount = totalCount * m_trainPercentage / 100;
            int testCount = totalCount - trainCount;
            var trainData = data.AsEnumerable().Take(trainCount).CopyToDataTable();
            var testingData = data.AsEnumerable().Skip(trainCount).CopyToDataTable();
            #endregion

            #region training

            TrainingModel(trainData);



            #endregion
        }
        //暫存每個thread的輸出值
        static Dictionary<string, double> ms_tempOutputValues = null;
        //看thread結束狀態
        static Dictionary<string, bool> ms_stopFlog = null;
        private void TrainingModel(DataTable sourceData)
        {
            //每筆輸入值
            for (int rowIndex = 0; rowIndex < sourceData.Rows.Count; rowIndex++)
            {
                Dictionary<string, double> outputValue = ForwardPropagation(sourceData, rowIndex);
                double effectValue = CalculateEffect(outputValue, sourceData.Rows[rowIndex]);
                //門檻 break

                BackPropagation(sourceData.Rows[rowIndex]);
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
                        nWeight.Weight += (m_learningRate * neural.GapValue * nWeight.SourceOutputValue);
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
            ms_stopFlog = new Dictionary<string, bool>();
            for (int heddenNeuralIndex = 0; heddenNeuralIndex < m_heddenNeuralSize; heddenNeuralIndex++)
            {
                //每個hedden都自己開thread做
                string heddenName = "Neural0_" + heddenNeuralIndex;
                ms_stopFlog.Add(heddenName, false);
                ms_tempOutputValues.Add(heddenName, 0);
                StartInputThread(sourceData.Rows[rowIndex], heddenName);
            }
            //等所有的hedden thread跑完
            Dictionary<string, double> inputValue = WaitAndGenData();


            #endregion

            #region 隱藏層(目前用不到)
            if (m_heddenLayerSize > 1)
            {
                ms_stopFlog = new Dictionary<string, bool>();
                ms_tempOutputValues = new Dictionary<string, double>();
                for (int heddenLayerIndex = 1; heddenLayerIndex < m_heddenLayerSize; heddenLayerIndex++)
                {
                    for (int heddenNeuralIndex = 0; heddenNeuralIndex < m_heddenNeuralSize; heddenNeuralIndex++)
                    {

                        //每個hedden都自己開thread做
                        string heddenName = "Neural" + heddenLayerIndex + "_" + heddenNeuralIndex;
                        ms_stopFlog.Add(heddenName, false);
                        ms_tempOutputValues.Add(heddenName, 0);
                        StartHeddenThread(inputValue, heddenName);
                    }
                    //等所有的hedden thread跑完
                    inputValue = WaitAndGenData();
                }
            }
            #endregion

            #region 輸出層
            ms_stopFlog = new Dictionary<string, bool>();
            ms_tempOutputValues = new Dictionary<string, double>();
            var targetCol = sourceData.Columns.Cast<DataColumn>().Where(item => item.ColumnName.IndexOf("Target") > -1).ToList();
            for (int outputIndex = 0; outputIndex < targetCol.Count; outputIndex++)
            {
                //每個hedden都自己開thread做
                string OutputName = "Neural" + m_heddenLayerSize + "_" + targetCol[outputIndex].ColumnName;
                ms_stopFlog.Add(OutputName, false);
                ms_tempOutputValues.Add(OutputName, 0);
                StartHeddenThread(inputValue, OutputName);
            }
            //等所有的hedden thread跑完
            inputValue = WaitAndGenData();


            #endregion

            return inputValue;
        }

        //開input層到隱藏層的thead
        private static void StartInputThread(DataRow dr, string heddenName)
        {
            Input2HeddenThreadClass input2HeddenThreadClass = new Input2HeddenThreadClass();
            input2HeddenThreadClass.ThreadName = heddenName;
            input2HeddenThreadClass.InputData = dr;
            Thread heddenThread = new Thread(input2HeddenThreadClass.InputCombineAndSigmoid);
            heddenThread.Start();
        }

        //開隱藏層之後的thread
        private static void StartHeddenThread(Dictionary<string, double> inputValue, string heddenName)
        {
            Hedden2OutputThreadClass hedden2OutputThreadClass = new Hedden2OutputThreadClass();
            hedden2OutputThreadClass.ThreadName = heddenName;
            hedden2OutputThreadClass.InputData = inputValue;
            Thread heddenThread = new Thread(hedden2OutputThreadClass.CalculateOutputValue);
            heddenThread.Start();
        }

        //等所有的thread結束
        private static Dictionary<string, double> WaitAndGenData()
        {
            //TODO解決執行緒改變內容的問題
            int i = 0;
            //一個一個過
            while (i < ms_stopFlog.Count)
            {
                i = 0;
                foreach (KeyValuePair<string, bool> item in ms_stopFlog)
                {
                    if (!item.Value)
                        break;
                    i++;

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
                ms_stopFlog[ThreadName] = true;
            }
        }

        //計算隱藏層之後的
        class Hedden2OutputThreadClass
        {
            public string ThreadName { get; set; }
            public Dictionary<string, double> InputData { get; set; }

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
                ms_stopFlog[ThreadName] = true;
            }
        }

        #endregion

        /// <summary>
        /// 初始化各神經元的權重
        /// </summary>
        /// <param name="data"></param>
        private void InitialWeight(DataTable data)
        {
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
                    Random r = new Random(Guid.NewGuid().GetHashCode());
                    nWeights.Add(new NeuralWeight()
                    {
                        Weight = (j == 0 ? 1 - r.NextDouble() : r.NextDouble()),
                        Name = (j == 0 ? "W_0" : "W_" + data.Columns[j - 1].ColumnName)
                    });
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
                        nWeights.Add(new NeuralWeight()
                        {
                            Weight = (j == 0 ? 1 - r.NextDouble() : r.NextDouble()),
                            Name = (j == 0 ? "W_0" : "W_Neural" + (s - 1) + "_" + i)
                        });
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
                    nWeights.Add(new NeuralWeight()
                    {
                        Weight = (j == 0 ? 1 - r.NextDouble() : r.NextDouble()),
                        Name = (j == 0 ? "W_0" : "W_Neural" + (m_heddenLayerSize - 1) + "_" + (j - 1))
                    });

                }
                ms_neural.Add(new Neural() { Layer = m_heddenLayerSize, Name = "Neural" + (m_heddenLayerSize) + "_" + targetCol[i].ColumnName, Weights = nWeights });

            }
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

        /// <summary>
        /// 正規化
        /// </summary>
        /// <param name="excelSheet"></param>
        /// <returns></returns>
        private DataTable Normalization(LinqToExcel.Query.ExcelQueryable<Row> excelSheet)
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

    }
}
