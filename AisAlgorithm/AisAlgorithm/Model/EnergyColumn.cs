using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisAlgorithm.Model
{
    public class EnergyColumn
    {
        public bool Checked { get; set; }

        public bool IsFuzzy { get; set; }

        public bool Fuzzy
        {
            get
            {
                return Checked && IsFuzzy;
            }
        }

        public int MemberFunctionRegion { get; set; }
        public string Text { get; set; }

        public string Value { get; set; }
        public EnergyColumn(bool ch, string text,string value,int memberFunctionRegion)
        {
            Checked = ch;
            Text = text;
            Value = value;
            IsFuzzy = false;
            MemberFunctionRegion = memberFunctionRegion;
        }
    }
}
