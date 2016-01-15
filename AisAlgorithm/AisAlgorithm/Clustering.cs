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
            SetSimilarity(m_setting.SimilarityMethod);
        }

        private void SetSimilarity(SimilarityMethod _similarity)
        {
            //設定分群器
            switch (_similarity)
            {
                case SimilarityMethod.HammingDistence:
                    similarity = new HammingDistence(m_setting.ThresholdList[0], m_setting.ThresholdList[1]);
                    break;
                case SimilarityMethod.EucideanDistence:
                    similarity = new EuclideanDistence(m_setting.ThresholdList[0]);
                    break;
                case SimilarityMethod.PredictionDistence:
                    similarity = new PredictionDistence();
                    break;

                default:
                    break;
            }
        }

        public Clustering(SimilarityMethod _similarity)
        {
            SetSimilarity(_similarity);
        }

        public Dictionary<int,GroupInfo> FilterGroup(DataRow dr,Dictionary<int,GroupInfo> groupData)
        {
            double similarityValue;
            Dictionary<int, GroupInfo> result = new Dictionary<int, GroupInfo>();
            foreach (KeyValuePair<int,GroupInfo> item in groupData)
            {
                //TODO 這邊的threshole不應該跟一開始的一樣
                if (similarity.Caculate(item.Value.ColCaculate, dr, out similarityValue))
                {
                    item.Value.SimilarityValue = similarityValue;
                    result.Add(item.Key, item.Value);
                }
            }
            return result;
        }

        /// <summary>
        /// 執行分群
        /// </summary>
        /// <param name="dt">資料</param>
        /// <param name="groupCount">起始分群數</param>
        /// <returns></returns>
        public Dictionary<int, GroupInfo> DoClustering(DataTable dt, int groupCount)
        {
            double similarityValue;
            //加入一個欄位記錄分群結果
            dt.Columns.Add("GroupId");
            Dictionary<int, GroupInfo> result = new Dictionary<int, GroupInfo>();
            //亂數選出要起始分群的RowIndex
            var roundIndexs = Enumerable.Range(0, dt.Rows.Count).OrderBy(item => item * item * new Random(new Guid().GetHashCode()).Next()).Take(groupCount);
            int groupId = 1;
            //分初始群
            foreach (int index in roundIndexs)
            {
                dt.Rows[index]["GroupId"] = groupId + ",";
                result[groupId] = new GroupInfo(m_setting.GroupCenter);
                result[groupId].GroupId = groupId;
                result[groupId].Add(dt.Rows[index]);
                groupId++;
            }

            //一筆一筆的分群
            foreach (DataRow dr in dt.Rows)
            {
                //尚未分群者
                if (string.IsNullOrEmpty(dr["GroupId"].ToString()))
                {
                    bool isSimilarity = false;
                    //先在已有群中看是否屬於該群
                    foreach (int id in result.Keys)
                    {
                        //相似度符合門檻時就加進去該群(加入多群)
                        if (similarity.Caculate(result[id].ColCaculate, dr, out similarityValue))
                        {
                            isSimilarity = true;
                            dr["GroupId"] += id + ",";
                            result[id].Add(dr);
                            //如果只屬於一個群就需要continue離開
                            //continue;
                        }
                    }

                    //都沒有符合群時，自己開一群
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
