using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisAlgorithm.Model
{
    public class CheckBoxListItem
    {
        public bool Checked { get; set; }
        public string Text { get; set; }

        public string Value { get; set; }
        public CheckBoxListItem(bool ch, string text,string value)
        {
            Checked = ch;
            Text = text;
            Value = value;
        }
    }
}
