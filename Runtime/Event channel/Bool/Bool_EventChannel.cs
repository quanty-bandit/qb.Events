#if UNITY_EDITOR
using System.Globalization;
#endif
using UnityEngine;
namespace qb.Events
{

    [CreateAssetMenu(fileName = "Bool_EventChannel", menuName = "qb/Event/Bool_EventChannel")]
    public class Bool_EventChannel:EventChannel<bool>
    {

    }
}
