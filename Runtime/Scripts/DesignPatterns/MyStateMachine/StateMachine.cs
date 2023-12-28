using System;
using System.Collections.Generic;
using UnityEngine;

namespace NP
{
    public class StateMachine<T> where T : Enum
    {
        public T CurrentState { get; private set; }
        
        private struct Transition
        {
            public T Destination;
            public Func<bool> Guard;
            public Action TransitionAction;
        }

        public StateMachine(T defaultState, IState stateObj)
        {
            m_states = new Dictionary<T, IState>();
            m_transitions = new Dictionary<T, List<Transition>>();
            
            AddState(defaultState, stateObj);

            CurrentState = defaultState;
            m_states[CurrentState].OnEnter();
        }

        public void AddState(T state, IState stateObject)
        {
            m_states.Add(state, stateObject);
        }

        public void AddTransition(T from, T to, Action transitionAction = null, Func<bool> guard = null)
        {
            if (!m_transitions.ContainsKey(from))
            {
                m_transitions.Add(from, new List<Transition>());
            }

            m_transitions[from].Add(new Transition()
            {
                Destination = to,
                TransitionAction = transitionAction,
                Guard = guard
            });
        }

        public void AddBidirectionalTransition(T from, T to, Action transitionAction = null, Func<bool> transitionGuard = null,
            Action backTransitionAction = null, Func<bool> backTransitionGuard = null)
        {
            AddTransition(from, to, transitionAction, transitionGuard);
            AddTransition(to, from, backTransitionAction, backTransitionGuard);
        }

        public void SetState(T state)
        {
            if (Convert.ToInt32(state) == Convert.ToInt32(CurrentState))
            {
                return;
            }
            
            if (!TryTransition(CurrentState, state, out var transitionAction))
            {
                return;
            }

            m_states[CurrentState]?.OnLeave();
            transitionAction?.Invoke();
            CurrentState = state;
            m_states[CurrentState]?.OnEnter();
        }

        private bool TryTransition(T from, T to, out Action transitionAction)
        {
            transitionAction = null;
            if (!m_transitions.ContainsKey(from))
            {
                Debug.LogError($"invalid transition from {CurrentState} to {to}");
                return false;
            }

            var transitions = m_transitions[from];
            foreach (var transition in transitions)
            {
                if (transition.Destination.Equals(to))
                {
                    if (transition.Guard != null && transition.Guard())
                    {
                        transitionAction = transition.TransitionAction;
                        return true;
                    }

                    Debug.LogError($"transition from {CurrentState} to {to} failed");
                    return false;
                }
            }

            return false;
        }

        private Dictionary<T, IState> m_states;
        private Dictionary<T, List<Transition>> m_transitions;
    }
}