using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anneal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Dictionary<string, int> disData;
        private List<string> worstPath = new List<string>();
        private void btn_Execute_Click(object sender, EventArgs e)
        {
            lsb_Data.Items.Clear();
            Dictionary<string, string> disList = new Dictionary<string, string>();
            disList["A"] = "0,25,15,33,18";
            disList["B"] = "25,0,9,16,22";
            disList["C"] = "15,9,0,30,25";
            disList["D"] = "33,16,30,0,19";
            disList["E"] = "18,22,25,19,0";
            

            int exeCount = 0;
            int exeMax = 100;
            while (exeCount < exeMax)
            {
                List<string> path = new List<string>();
                worstPath = new List<string>();
                path.Add("A");
                path.Add("B");
                path.Add("C");
                path.Add("D");
                path.Add("E");

                int temp = 2000;
                int deltaTemp = 100;
                double fitnessProbability = 0.9;
                double deltaFitnessProbability = 0.9;
                int errorCount = 0;
                int fitnessCount = 0;

                disData = InitialDistance(disList); ;
                int bastDistance = CalculateDistance(path);

                while (temp > 0)
                {
                    lsb_Data.Items.Add("temp:" + temp);
                    while (errorCount < 3 && fitnessCount < 100)
                    {
                        fitnessCount++;
                        lsb_Data.Items.Add("errorCount:" + errorCount);
                        List<string> nextPath = randomChange(path);
                        int nextDistance = CalculateDistance(nextPath);
                        if (nextDistance > bastDistance)
                        {
                            double randomFitnessProbability = new Random().Next(0, 999) * 0.001;
                            if (randomFitnessProbability > fitnessProbability)
                            {
                                errorCount++;
                                worstPath.Add(string.Join("-", nextPath.ToArray()));
                                continue;
                            }
                        }
                        path = nextPath;
                        bastDistance = nextDistance;
                        lsb_Data.Items.Add(string.Join("-", path.ToArray()) + "," + bastDistance);
                        Application.DoEvents();
                    }
                    temp -= deltaTemp;
                    fitnessProbability *= deltaFitnessProbability;
                    errorCount = 0;
                    fitnessCount = 0;
                    Application.DoEvents();
                }
                if (!lsb_Ans.Items.Contains(string.Join("-", path.ToArray()) + "," + bastDistance))
                    lsb_Ans.Items.Add(string.Join("-", path.ToArray()) + "," + bastDistance);
                exeCount++;
            }
        }

        private Dictionary<string, int> InitialDistance(Dictionary<string, string> disList)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (KeyValuePair<string, string> item in disList)
            {
                string[] values = item.Value.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    if (!values[i].Trim().Equals("0"))
                    {
                        int disPoint = -1;
                        int.TryParse(values[i], out disPoint);
                        result.Add(item.Key + "-" + Convert.ToChar(65 + i), disPoint);
                    }
                }
            }
            return result;
        }

        private int CalculateDistance(List<string> path)
        {
            int disCount = 0;
            for (int i = 0; i < path.Count; i++)
            {
                int nextPath = i + 1;
                disCount += disData[path[i] + "-" + path[(nextPath > path.Count - 1 ? 0 : nextPath)]];
            }
            return disCount;
        }

        private List<string> randomChange(List<string> path)
        {
            List<string> result = new List<string>();
            do
            {
                result = new List<string>(path);
                int pointA = new Random(Guid.NewGuid().GetHashCode()).Next(0, 4);
                int pointB = -1;
                do
                {
                    pointB = new Random(Guid.NewGuid().GetHashCode()).Next(0, 4);
                } while (pointB == -1 && pointB == pointA);
                string tempPoint = result[pointA];
                result[pointA] = result[pointB];
                result[pointB] = tempPoint;
            } while (worstPath.Contains(string.Join("-", result.ToArray())));
            return result;
        }
    }
}
