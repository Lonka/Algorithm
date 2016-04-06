using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AisAlgorithm.Category;

namespace AisAlgorithm.Model
{
    internal class ClusteringSetting
    {
        public ClusteringSetting()
        {
            ThresholdList = new List<double>();
        }
        public SimilarityMethod SimilarityMethod { get; set; }

        public bool Fuzzy { get; set; }

        public List<double> ThresholdList { get; set; }

        public GroupCenter GroupCenter { get; set; }
    }
}
