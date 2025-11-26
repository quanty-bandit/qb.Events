using System.Globalization;
using UnityEditor;
using UnityEngine;
namespace qb.Events
{
    [CreateAssetMenu(fileName = "Float_EventChannel", menuName = "qb/Event/Float_EventChannel")]
    public class Float_EventChannel : EventChannel<float>
    {
#if UNITY_EDITOR
        protected override bool ShowDispatchEventButton => true;
        protected override void DispatchEventFromString(string value)
        {
            if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
            {
                DispatchEvent(result);
            }
            else
                Debug.LogError($"Cant't parse <{value}> as a valid float, event wasn't dispatched!");
        }
#endif
    }
}
