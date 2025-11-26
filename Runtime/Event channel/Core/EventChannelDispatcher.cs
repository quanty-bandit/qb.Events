using UnityEngine;
using UnityEngine.Serialization;
namespace qb.Events
{
    [AddComponentMenu("qb/Event/EventChannelDispatcher")]
    public class EventChannelDispatcher : MonoBehaviour
    {
        [SerializeField]
        [FormerlySerializedAs("provider")]
        ECProvider_W channelProvider;
        public void DispatchEvent()=>channelProvider.DispatchEvent();
    }

    public abstract class EventChannelDispatcher<T> : MonoBehaviour
    {
        [SerializeField]
        [FormerlySerializedAs("provider")]
        ECProvider_W<T> channelProvider;
        public void DispatchEvent(T value) => channelProvider.DispatchEvent(value);
    }

}
