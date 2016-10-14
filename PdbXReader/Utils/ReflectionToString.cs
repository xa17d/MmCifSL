using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdbXReader.Utils
{
    static class ReflectionToString
    {
        public static string ToString(object obj)
        {
            if (obj == null) { return ConvertToString(null); }
            else
            {
                StringBuilder s = new StringBuilder();
                s.Append("{[");
                s.Append(obj.GetType().Name);
                s.Append("{]");
                foreach (var prop in obj.GetType().GetProperties())
                {
                    s.Append(" ");
                    s.Append(prop.Name);
                    s.Append(":");
                    s.Append(ConvertToString(prop.GetValue(obj)));
                    s.Append(";");
                }

                s.Append(" }");
                return s.ToString();
            }
        }

        private static string ConvertToString(object value)
        {
            if (value == null) { return "null"; }
            else if (value is String) { return "\"" + value.ToString() + "\""; }
            else { return value.ToString(); }
        }
    }
}
