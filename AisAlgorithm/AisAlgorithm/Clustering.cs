using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisAlgorithm
{
    internal class Clustering
    {
        private ISimilarity similarity;
        private ClusteringSetting m_setting;
        public Clustering(ClusteringSetting _setting)
        {
            m_setting = _setting;
            switch (m_setting.SimilarityMethod)
            {
                case SimilarityMethod.HammingDistence:
                    similarity = new HammingDistence(m_setting.ThresholdList[0], m_setting.ThresholdList[1]);
                    break;
                case SimilarityMethod.EucideanDistence:
                    similarity = new EuclideanDistence(m_setting.ThresholdList[0]);
                    break;
                default:
                    break;
            }
        }

        public Dictionary<int,GroupInfo> FilterGroup(DataRow dr,Dictionary<int,GroupInfo> groupData)
        {
            Dictionary<int, GroupInfo> result = new Dictionary<int, GroupInfo>();
            foreach (KeyValuePair<int,GroupInfo> item in groupData)
            {
                //TODO 這邊的threshole不應該跟一開始的一樣
                if(similarity.Caculate(item.Value.ColAvg, dr))
                {
                    result.Add(item.Key, item.Value);
                }
            }
            return result;
        }

        public Dictionary<int, GroupInfo> DoClustering(DataTable dt, int groupCount)
        {
            dt.Columns.Add("GroupId");
            Dictionary<int, GroupInfo> result = new Dictionary<int, GroupInfo>();
            var roundIndexs = Enumerable.Range(0, dt.Rows.Count).OrderBy(item => item * item * new Random(new Guid().GetHashCode()).Next()).Take(groupCount);
            int groupId = 1;
            foreach (int index in roundIndexs)
            {
                dt.Rows[index]["GroupId"] = groupId + ",";
                result[groupId] = new GroupInfo(m_setting.GroupCenter);
                result[groupId].GroupId = groupId;
                result[groupId].Add(dt.Rows[index]);
                groupId++;
            }
            foreach (DataRow dr in dt.Rows)
            {
                if (string.IsNullOrEmpty(dr["GroupId"].ToString()))
                {
                    bool isSimilarity = false;
                    foreach (int id in result.Keys)
                    {
                        if (similarity.Caculate(result[id].ColAvg, dr))
                        {
                            isSimilarity = true;
                            dr["GroupId"] += id + ",";
                            result[id].Add(dr);
                        }
                    }
                    if (!isSimilarity)
                    {
                        dr["GroupId"] += groupId + ",";
                        result[groupId] = new GroupInfo(m_setting.GroupCenter);
                        result[groupId].GroupId = groupId;
                        result[groupId].Add(dr);
                        groupId++;
                    }
                }


            }
            return result;
        }
    }
}
