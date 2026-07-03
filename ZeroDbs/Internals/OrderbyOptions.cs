using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroDbs
{
    /// <summary>
    /// 排序项
    /// </summary>
    internal class OrderbyOptions : IOrderbyOptions
    {
        readonly List<IOrderby> orderbies = new List<IOrderby>();
        public int Count { get {  return orderbies.Count; } }
        public IOrderby this[int index] { get { return orderbies[index]; } }
        public OrderbyOptions()
        {
            
        }
        public OrderbyOptions(string field, bool isAscending = true)
        {
            orderbies.Add(new Orderby { Field=field, IsAscending = isAscending });
        }
        public IOrderbyOptions Add(string field, bool isAscending=true)
        {
            orderbies.Add(new Orderby { Field=field, IsAscending= isAscending });
            return this;
        }
        public void Clear()
        {
            orderbies.Clear();
        }
        public List<IOrderby> GetOrderbies() { return orderbies; }
        public IEnumerator<IOrderby> GetEnumerator()
        {
            return orderbies.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
