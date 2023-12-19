using System;
using System.Collections.Generic;

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
                    if (kv.Value == true)
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
        public LogicOr(IEnumerable<TEnum> slots, Action<bool, bool> onValueChange)
            : base(slots, onValueChange)
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
        public LogicAnd(IEnumerable<TEnum> slots, Action<bool, bool> onValueChange)
            : base(slots, onValueChange)
        {
        }
    }

    public abstract class LogicOP<TEnum> where TEnum : Enum
    {
        public abstract bool Value { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="onValueChange"> 第一个是之前，第二个是现在 </param>
        protected LogicOP(IEnumerable<TEnum> slots, Action<bool, bool> onValueChange)
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

        protected readonly Dictionary<string, bool> m_status;
        private readonly Action<bool, bool> m_onValueChange;
    }
}