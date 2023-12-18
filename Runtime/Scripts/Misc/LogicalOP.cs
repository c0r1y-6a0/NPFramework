using System;
using System.Collections.Generic;

namespace NP
{
    public class LogicAnd<TEnum> where TEnum:Enum
    {
        public bool Value
        {
            get
            {
                foreach (var kv in m_status)
                {
                    if (kv.Value == false)
                        return false;
                }

                return true;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="onValueChange"> 第一个是之前，第二个是现在 </param>
        public LogicAnd(IEnumerable<TEnum> slots, Action<bool, bool> onValueChange)
        {
            m_status = new();
            m_onValueChange = onValueChange;
            foreach (var slot in slots)
            {
                m_status.Add(slot.ToString(), false);
            }
        }
        
        public void Set(string slot, bool b)
        {
            bool preValue = Value;
            m_status[slot] = b;
            bool newValue = Value;
            if (preValue != newValue)
            {
                m_onValueChange?.Invoke(preValue, newValue);
            }
        }

        private Dictionary<string, bool> m_status;
        private Action<bool, bool> m_onValueChange;
    }
}