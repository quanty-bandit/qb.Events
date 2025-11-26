using System.Globalization;
using UnityEngine;
namespace qb.Events
{

    [CreateAssetMenu(fileName = "Int_EventChannel", menuName = "qb/Event/Int_EventChannel")]
    public class Int_EventChannel : EventChannel<int>
    {
#if UNITY_EDITOR
        protected override bool ShowDispatchEventButton => true;
        protected override void DispatchEventFromString(string value)
        {
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
            {
                DispatchEvent(result);
            }
            else
                Debug.LogError($"Cant't parse <{value}> as a valid integer, event wasn't dispatched!");

        }
#endif


    }
}
