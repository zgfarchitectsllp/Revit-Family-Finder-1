using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ZGF.Revit
{
    /// <summary>
    /// Saves the history of terms entered into a comboBox, such as a search history
    /// </summary>
    class SearchHistory
    {   
        private List<string> m_history_list = new List<string>();

        public SearchHistory()
        {
            MaxHistoryToSave = 12;            
        }
        public SearchHistory(StringCollection searchHistory)
        {
            MaxHistoryToSave = 12;
            foreach (string s in searchHistory)
                m_history_list.Add(s);
        }

        public int MaxHistoryToSave { get; set; }

        public int Count { get { return m_history_list.Count; } }
        
        public void Add(string item, bool toBeginning)
        {
            // Check for duplicates and remove
            if (m_history_list.Contains(item, new HistoryItemComparer()))
            {
                m_history_list.RemoveAll(s => s.Equals(item, StringComparison.InvariantCultureIgnoreCase));
            }
            // Add to beginning of collection
            if (toBeginning)
                m_history_list.Insert(0, item);
            else
                m_history_list.Add(item);

            while (m_history_list.Count > MaxHistoryToSave)
                m_history_list.RemoveAt(m_history_list.Count - 1);
        }        

        //public void AddItems(string[] items, bool replace)
        //{

        //}

        public void Reverse()
        {
            m_history_list.Reverse();            
        }

        public void Clear()
        {
            m_history_list.Clear();
        }

        public string[] AsStringArray()
        {   
            return m_history_list.ToArray();
        }

        public StringCollection AsStringCollection()
        {
            StringCollection sc = new StringCollection();
            sc.AddRange(m_history_list.ToArray());
            return sc;
        }

        public List<string> AsList()
        {
            return m_history_list;
        }
        
    }

    class HistoryItemComparer : IEqualityComparer<string>
    {

        public bool Equals(string x, string y)
        {
            return x.Equals(y, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
