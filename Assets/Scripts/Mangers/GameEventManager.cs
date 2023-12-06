using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystems.Core
{
    [CreateAssetMenu(fileName = "GameEventManager", menuName = "GameSystems/GameEventManager", order = 0)]
    public class GameEventManager : ScriptableObject
    {
        private Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();
        private Dictionary<string, Delegate> argEventDictionary = new Dictionary<string, Delegate>();

        public void AddEvent(string key, Action action)
        {
            if (eventDictionary.ContainsKey(key))
            {
                Util.ShowMessage($"The event by {key} already exists.");
                return;
            }
            eventDictionary[key] = action;
        }

        public void AddEvent<T>(string key, Action<T> action)
        {
            if (argEventDictionary.ContainsKey(key))
            {
                Util.ShowMessage($"The event by {key} with argument already exists.");
                return;
            }
            argEventDictionary[key] = action;
        }

        public void Subscribe(string eventName, Action listener)
        {
            if (!eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] = listener;
            }
            else
            {
                eventDictionary[eventName] += listener;
            }
        }

        public void Unsubscribe(string eventName, Action listener)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] -= listener;
            }
        }

        public void Subscribe<T>(string eventName, Action<T> listener)
        {
            if (argEventDictionary.TryGetValue(eventName, out var existingDelegate))
            {
                argEventDictionary[eventName] = Delegate.Combine(existingDelegate, listener);
            }
            else
            {
                argEventDictionary[eventName] = listener;
            }
        }

        public void Unsubscribe<T>(string eventName, Action<T> listener)
        {
            if (argEventDictionary.TryGetValue(eventName, out var existingDelegate))
            {
                argEventDictionary[eventName] = Delegate.Remove(existingDelegate, listener);
            }
        }

        public void RaiseEvent(string eventName)
        {
            if (eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent?.Invoke();
            }
        }

        public void RaiseEvent<T>(string eventName, T arg)
        {
            if (argEventDictionary.TryGetValue(eventName, out var existingDelegate))
            {
                if (existingDelegate is Action<T> action)
                {
                    action.Invoke(arg);
                }
            }
        }
    }
}
