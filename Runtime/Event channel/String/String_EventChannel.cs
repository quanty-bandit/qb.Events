using UnityEngine;
namespace qb.Events
{
    [CreateAssetMenu(fileName = "String_EventChannel", menuName = "qb/Event/String_EventChannel")]
    public class String_EventChannel : EventChannel<string> 
    {
#if UNITY_EDITOR
        protected override bool ShowDispatchEventButton => true;
        protected override void DispatchEventFromString(string value) => DispatchEvent(value);
#endif

    }
}
