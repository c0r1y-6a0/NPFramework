using System;
using System.Collections.Generic;
using UnityEngine;

namespace NP
{
    public class LogicOr<TEnum> : LogicOP<TEnum> where TEnum : Enum
    {
        public override bool Value
        {
            get
            {
                foreach (var kv in m_status)
                {
                    if (kv.Value)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="onValueChange"> 第一个是之前，第二个是现在 </param>
        public LogicOr(IEnumerable<TEnum> slots, Action<bool, bool> onValueChange, bool alwaysUpdate = false, string debugString = null)
            : base(slots, onValueChange, alwaysUpdate, debugString)
        {
        }
    }

    public class LogicAnd<TEnum> : LogicOP<TEnum> where TEnum : Enum
    {
        public override bool Value
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
        public LogicAnd(IEnumerable<TEnum> slots, Action<bool, bool> onValueChange, bool alwaysUpdate = false, string debugString = null)
            : base(slots, onValueChange, alwaysUpdate, debugString)
        {
        }
    }

    public abstract class LogicOP<TEnum> where TEnum : Enum
    {
        public abstract bool Value { get; }
        public string DebugString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="onValueChange"> 第一个是之前，第二个是现在 </param>
        protected LogicOP(IEnumerable<TEnum> slots, Action<bool, bool> onValueChange, bool alwaysUpdate, string debugString)
        {
            m_status = new();
            m_onValueChange = onValueChange;
            m_alwaysUpdate = alwaysUpdate;
            DebugString = debugString;
            foreach (var slot in slots)
            {
                m_status.Add(slot.ToString(), false);
            }
        }

        public void Set(string slot, bool b)
        {
            string preStatus = GetStatusString();
            bool preValue = Value;
            m_status[slot] = b;
            bool newValue = Value;

            if (DebugString != null)
            {
                Debug.Log($"LogicOP {DebugString} statuc from {preStatus} to {GetStatusString()}");
            }

            if (preValue != newValue || m_alwaysUpdate)
            {
                m_onValueChange?.Invoke(preValue, newValue);
            }
        }

        private string GetStatusString()
        {
            string result = "";
            foreach (var kv in m_status)
            {
                result += $"{kv.Key}:{kv.Value}   ";
            }

            return result;
        }

        protected readonly Dictionary<string, bool> m_status;
        private readonly Action<bool, bool> m_onValueChange;
        private readonly bool m_alwaysUpdate;
    }
}