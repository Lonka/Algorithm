using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackPropagationNeuralNetwork
{
    public class Forecast
    {

    }
    public class EnergyColumn
    {
        public EnergyColumn(string _name, string _chiName, DataType _dataType,string _weightType)
        {
            ChiName = _chiName;
            Name = _name;
            DataType = _dataType;
            WeightType = _weightType;
            if (DataType == BackPropagationNeuralNetwork.DataType.Continuous)
            {
                MaxValue = double.MinValue;
                MinValue = double.MaxValue;
            }
            else if (DataType == BackPropagationNeuralNetwork.DataType.Category)
            {
                CategoryList = new List<string>();
            }
        }
        public string WeightType { get; set; }
        public string Name { get; set; }
        public string ChiName { get; set; }
        public DataType DataType { get; set; }

        public List<string> CategoryList { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
    }

    public enum DataType
    {
        Continuous,
        Category
    }

    public class NeuralWeight
    {
        public NeuralWeight()
        {
            PreviousCorrectionValue = 0;
        }
        public double Weight { get; set; }
        public string Name { get; set; }

        public double SourceOutputValue { get; set; }

        public double PreviousCorrectionValue { get; set; }
    }

    public class Neural
    {
        public int Layer { get; set; }
        //Like : Neural5_Target
        public string Name { get; set; }

        public double GapValue { get; set; }
        public double OutputValue { get; set; }
        public List<NeuralWeight> Weights { get; set; }
    }
}
