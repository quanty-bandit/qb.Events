using UnityEngine;
using UnityEngine.Serialization;
namespace qb.Events
{
    /// <summary>
    /// Component that dispatches events using a specified event channel provider.
    /// </summary>
    [AddComponentMenu("qb/Event/EventChannelDispatcher")]
    public class EventChannelDispatcher : MonoBehaviour
    {
        [SerializeField]
        [FormerlySerializedAs("provider")]
        ECProvider_W channelProvider;
        public void DispatchEvent()=>channelProvider.DispatchEvent();
    }
    /// <summary>
    /// Abstract base class for dispatching events of type T through a channel provider in Unity.
    /// </summary>
    /// <typeparam name="T">The type of event data to be dispatched.</typeparam>
    public abstract class EventChannelDispatcher<T> : MonoBehaviour
    {
        [SerializeField]
        [FormerlySerializedAs("provider")]
        ECProvider_W<T> channelProvider;
        public void DispatchEvent(T value) => channelProvider.DispatchEvent(value);
    }

}
