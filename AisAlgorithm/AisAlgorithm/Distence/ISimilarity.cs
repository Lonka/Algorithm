using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisAlgorithm
{
    internal interface ISimilarity
    {
        bool Caculate(Dictionary<string, double> colAvg, DataRow dr);
    }
}
