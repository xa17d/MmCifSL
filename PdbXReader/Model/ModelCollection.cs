using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdbXReader.Model
{
    public class ModelCollection<T> : IEnumerable<T>
        where T : IModelId
    {
        private Dictionary<string, T> items = new Dictionary<string, T>();

        public void Add(T item)
        {
            items.Add(item.Id, item);
        }

        public T GetById(string id)
        {
            return items[id];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }
    }
}
