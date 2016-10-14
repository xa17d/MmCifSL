using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MmCifSL
{
    public class MmCifData
    {
        public MmCifData(string name)
        {
            this.Name = name;
        }

        private Dictionary<string, List<MmCifDataCategory>> data = null;

        public string Name { get; set; }

        public List<MmCifDataCategory> this[string categoryName]
        {
            get
            {
                if (data == null)
                {
                    data = new Dictionary<string, List<MmCifDataCategory>>();
                }

                categoryName = categoryName.ToLower();

                List<MmCifDataCategory> list;
                if (data.TryGetValue(categoryName, out list))
                {
                    return list;
                }
                else
                {
                    list = new List<MmCifDataCategory>();
                    data.Add(categoryName, list);
                    return list;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();

            if (data != null)
            {
                foreach (KeyValuePair<string, List<MmCifDataCategory>> item in data)
                {
                    int i = 0;
                    foreach (var cat in item.Value)
                    {
                        s.AppendLine(item.Key + "[" + (i++) + "]");
                        s.AppendLine(cat.ToString());
                    }

                    s.AppendLine("");
                }
            }

            return s.ToString();
        }
    }
}
