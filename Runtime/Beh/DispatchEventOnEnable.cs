using UnityEngine;
using UnityEngine.Events;
namespace qb.Events
{
    public class DispatchEventOnEnable : MonoBehaviour
    {
        public UnityEvent onEnable = new UnityEvent();
        private void OnEnable()
        {
            onEnable.Invoke();
        }
    }
}
