using AisAlgorithm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisAlgorithm
{
    public class Parameter
    {
        private static readonly object ms_lockObject = new object();
        private volatile static Parameter ms_Instance;
        public static Parameter GetInstance()
        {
            if (ms_Instance == null)
            {
                lock (ms_lockObject)
                {
                    if (ms_Instance == null)
                    {
                        ms_Instance = new Parameter();
                    }
                }
            }
            return ms_Instance;
        }

        public Dictionary<string, List<HelfRegion>> FuzzyCol;
        public List<EnergyColumn> Items;
        public Dictionary<string, MaxMinValue> NorCol;


    }
}
