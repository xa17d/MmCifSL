using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MmCifSL
{
    public class MmCifDataCategory
    {
        Dictionary<string, string> data = null;
        
        public MmCifDataCategory(string categoryName)
        {
            this.CategoryName = categoryName;
        }

        public string CategoryName { get; set; }

        private void checkData()
        {
            if (data == null)
            {
                
            }
        }

        public string this[string attributeName]
        {
            get
            {
                if (data == null)
                { return null; }
                else
                { return data[attributeName.ToLower()]; }
            }

            set
            {
                if (data == null)
                {
                    data = new Dictionary<string, string>();
                }

                data[attributeName.ToLower()] = value;
            }
        }

        public override string ToString()
        {

            StringBuilder s = new StringBuilder();
            s.AppendLine(CategoryName + ".");
            if (data != null)
            {
                foreach (KeyValuePair<string, string> item in data)
                {
                    s.AppendLine("  " + item.Key + ": " + item.Value);
                }
            }

            return s.ToString();
        }
    }
}
