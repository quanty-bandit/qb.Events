using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
namespace qb.Events
{
    /// <summary>
    /// Listens to multiple event channels and invokes corresponding UnityEvents when events are received.
    /// </summary>
    [AddComponentMenu("qb/Event/EventChannelsListener")]
    public class EventChannelsListener : MonoBehaviour
    {
        [Serializable]
        public class ListenerSettings
        {
            [SerializeField]
            ECProvider_R channelProvider;
            [SerializeField]
            UnityEvent<object> onEvent = new UnityEvent<object>();
            public void Bind()
            {
                channelProvider.AddListener(InvokeEvent);
            }
            public void Unbind() 
            {
                 channelProvider.RemoveListener(InvokeEvent);
            }
            private void InvokeEvent(object sender)
            {
                onEvent.Invoke(sender);
            }
        }
        [SerializeField]
        List<ListenerSettings> listeners = new List<ListenerSettings>();
        private void OnEnable()
        {
            foreach(var  listener in listeners)
                listener.Bind();
        }
        private void OnDisable()
        {
            foreach (var listener in listeners)
                listener.Unbind();

        }
    }
    /// <summary>
    /// Abstract MonoBehaviour that manages a collection of event channel listeners, binding and unbinding them on
    /// enable and disable.
    /// </summary>
    /// <typeparam name="T">The type of event data received from the event channels.</typeparam>
    public abstract class EventChannelsListener<T> : MonoBehaviour
    {
        [Serializable]
        public class ListenerSettings
        {
            [SerializeField]
            ECProvider_R<T> channelProvider;
            [SerializeField]
            UnityEvent<T,object> onEvent = new UnityEvent<T,object>();
            public void Bind()
            {
                channelProvider.AddListener(InvokeEvent);
            }
            public void Unbind()
            {
                channelProvider.RemoveListener(InvokeEvent);
            }
            private void InvokeEvent(T value,object sender)
            {
                onEvent.Invoke(value,sender);
            }
        }
        [SerializeField]
        List<ListenerSettings> listeners = new List<ListenerSettings>();
        private void OnEnable()
        {
            foreach (var listener in listeners)
                listener.Bind();
        }
        private void OnDisable()
        {
            foreach (var listener in listeners)
                listener.Unbind();

        }
    }
}
