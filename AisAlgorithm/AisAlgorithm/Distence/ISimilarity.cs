using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AisAlgorithm.Model;

namespace AisAlgorithm
{
    internal interface ISimilarity
    {
        bool Caculate(Dictionary<string, double> colCaculate, DataRow dr,out double similarityValue);

        bool Caculate(GroupInfo groupInfo, DataRow dr, out double similarityValue);
    }
}
