using System;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using qb.Pattern;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace qb.Events
{
    [CreateAssetMenu(fileName = "EventChannel", menuName = "qb/Event/EventChannel")]
    public class EventChannel : SOWithGUID
    {
        [SerializeField]
        protected bool verbose = true;

#pragma warning disable 67
        protected event UnityAction<object> _event;
#pragma warning restore 67
        /// <summary>
        /// The event that can be subscribed to and invoked
        /// The add and remove processes avoid duplicated subscriptions and null object errors
        /// </summary>
        public event UnityAction<object> Event
        {
            add
            {
#if !NO_DEBUG_LOG
                if (verbose)
                    Debug.Log($"<color=#00FF10>Add event listener on {this.name}</color>");
#endif
                if (_event != null)
                {
                    var invocationList = _event.GetInvocationList();
                    foreach (var invocation in invocationList)
                    {
                        if (invocation == value as Delegate)
                        {
#if !NO_DEBUG_LOG_WARNING
                            Debug.LogWarning($"Duplicate subscription to event provider: {this.name}");
#endif
                            return;
                        }
                    }
                }
                _event += value;
            }
            remove
            {

                if (value != null && !(value.Target.Equals(null)))
                {
                    if (_event != null)
                    {
                        var invocationList = _event.GetInvocationList();
                        _event = null;
                        foreach (var invocation in invocationList)
                        {
                            if (invocation != value as Delegate)
                                _event += invocation as UnityAction<object>;
                        }
                    }
                    else
                    {
#if !NO_DEBUG_LOG_WARNING
                        Debug.LogWarning($"Try to unsubcribe from the empty event provider: {this.name}");
#endif
                    }
                }
                else
                {
#if !NO_DEBUG_LOG_WARNING
                    Debug.LogWarning($"Try to unsubcribe null action or null object action from event provider: {this.name}");
#endif
                    ClearInvalidSubscriptions();
                }
            }
        }

        [System.NonSerialized]
        object cleanUpLocker = new object();
        [System.NonSerialized]
        object locker = new object();

        /// <summary>
        /// Remove all invalid subscriptions in case of behaviours deletion
        /// </summary>
        public void ClearInvalidSubscriptions()
        {
            if (_event != null)
            {
                lock (cleanUpLocker)
                {
                    var invocationList = _event.GetInvocationList();
                    int validInvocationCount = 0;
                    foreach (var invocation in invocationList)
                    {
                        if (invocation != null && !(invocation.Target.Equals(null)))
                        {
                            validInvocationCount++;
                        }
                    }
                    if (validInvocationCount != invocationList.Length)
                    {
                        _event = null;
                        foreach (var invocation in invocationList)
                        {
                            if (invocation != null && !(invocation.Target.Equals(null)))
                            {
                                _event += invocation as UnityAction<object>;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Dispatch event  
        /// </summary>
        /// <param name="checkInvalidSubscriptors">
        /// Flag set by default on true for checking invalid subscriptors resulting of object destruction
        /// and avoid errors.
        /// If the falue is set to true, it is recomended to call the method ClearInvalidSubscriptions()
        /// when global context changed like new scene loading...
        /// </param>
        /// <param name="sender">
        /// The sender which dispatch the event, set to null by default for anonymous sender
        /// </param>
        [Button(ButtonSizes.Large)]
        public void DispatchEvent(object sender, bool checkInvalidSubscriptors = true)
        {
            if (_event != null)
            {
                if(checkInvalidSubscriptors){
                    lock (locker)
                    {
                        //reference.onValueChange.UpdateAndDispatchChangeEvent();
                        var invocationList = _event.GetInvocationList();
                        int validInvocationCount = 0;
                        foreach (var invocation in invocationList)
                        {
                            if (invocation != null && !(invocation.Target.Equals(null)))
                            {
                                validInvocationCount++;
                                (invocation as UnityAction<object>).Invoke(sender);
                            }
                        }
                        if (validInvocationCount != invocationList.Length)
                        {
                            _event = null;
                            foreach (var invocation in invocationList)
                            {
                                if (invocation != null && !(invocation.Target.Equals(null)))
                                    _event += invocation as UnityAction<object>;
#if !NO_DEBUG_LOG_WARNING
                                else
                                {
                                    Debug.LogWarning($"The OnChange provider [{this.name}] try to invoke an action of a null object!");
                                }
#endif
                            }
                        }
                    }
                }
                else
                    _event.Invoke(sender);
            }
        }
        /// <summary>
        /// Dispatch event  
        /// </summary>
        /// <param name="checkInvalidSubscriptors">
        /// Flag set by default on true for checking invalid subscriptors resulting of object destruction
        /// and avoid errors.
        /// If the falue is set to true, it is recomended to call the method ClearInvalidSubscriptions()
        /// when global context changed like new scene loading...
        /// </param>
        public void DispatchEvent(bool checkInvalidSubscriptors = true) => DispatchEvent(null,checkInvalidSubscriptors);


        [System.NonSerialized]
        bool isBindedOnSceneUnloadEvent;
        protected override void OnEnable()
        {
            base.OnEnable();
            if (!isBindedOnSceneUnloadEvent)
            {
                SceneManager.sceneUnloaded += OnSceneUnloaded;
                isBindedOnSceneUnloadEvent = true;
            }

        }
        protected override void OnDisable()
        {
            if (isBindedOnSceneUnloadEvent)
            {
                SceneManager.sceneUnloaded -= OnSceneUnloaded;
                isBindedOnSceneUnloadEvent = false;
            }
        }
        private void OnSceneUnloaded(Scene scene)
        {
            ClearInvalidSubscriptions();
        }
        
        protected override void Awake()
        {
            base.Awake();
#if UNITY_EDITOR
            var currentPath = AssetDatabase.GetAssetPath(this);
            if (currentPath != null)
            {
                var obj = AssetDatabase.LoadAssetAtPath<EventChannel>(currentPath);
                if (obj != null)
                {
                    var iconGuids = AssetDatabase.FindAssets($"{GetType().Name} t:texture2D", null);
                    if (iconGuids != null && iconGuids.Length > 0)
                    {
                        var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(iconGuids[0]));
                        EditorGUIUtility.SetIconForObject(obj, icon);
                    }
                }
            }
#endif 
        }
        
    }

    public abstract class EventChannel<T> : SOWithGUID
    {
        [SerializeField]
        protected bool verbose = true;

#pragma warning disable 67
        protected event UnityAction<T,object> _event;
#pragma warning restore 67
        /// <summary>
        /// The event that can be subscribed to and invoked.
        /// The add and remove processes avoid duplicated subscriptions and null object errors
        /// </summary>
        public event UnityAction<T,object> Event
        {
            
            add
            {
#if !NO_DEBUG_LOG
                if (verbose)
                    Debug.Log($"<color=#00FF10>Add event listener on {this.name}</color>");
#endif
                if (_event != null)
                {
                    var invocationList = _event.GetInvocationList();
                    foreach (var invocation in invocationList)
                    {
                        if (invocation == value as Delegate)
                        {
#if !NO_DEBUG_LOG_WARNING
                            Debug.LogWarning($"Duplicate subscription to event provider: {this.name}");
#endif
                            return;
                        }
                    }
                }
                _event += value;
            }
            remove
            {

                if (value != null && !(value.Target.Equals(null)))
                {
                    if (_event != null)
                    {
                        var invocationList = _event.GetInvocationList();
                        _event = null;
                        foreach (var invocation in invocationList)
                        {
                            if (invocation != value as Delegate)
                                _event += invocation as UnityAction<T,object>;
                        }
                    }
                    else
                    {
#if !NO_DEBUG_LOG_WARNING
                        Debug.LogWarning($"Try to unsubcribe from the empty event provider: {this.name}");
#endif
                    }
                }
                else
                {
#if !NO_DEBUG_LOG_WARNING
                    Debug.LogWarning($"Try to unsubcribe null action or null object action from event provider: {this.name}");
#endif
                    ClearInvalidSubscriptions();
                }
            }
        }

        [System.NonSerialized]
        object cleanUpLocker = new object();
        [System.NonSerialized]
        object locker = new object();

        /// <summary>
        /// Remove all invalid subscriptions in case of behaviours deletion
        /// </summary>
        public void ClearInvalidSubscriptions()
        {
            if (_event != null)
            {
                lock (cleanUpLocker)
                {
                    var invocationList = _event.GetInvocationList();
                    int validInvocationCount = 0;
                    foreach (var invocation in invocationList)
                    {
                        if (invocation != null && !(invocation.Target.Equals(null)))
                        {
                            validInvocationCount++;
                        }
                    }
                    if (validInvocationCount != invocationList.Length)
                    {
                        _event = null;
                        foreach (var invocation in invocationList)
                        {
                            if (invocation != null && !(invocation.Target.Equals(null)))
                            {
                                _event += invocation as UnityAction<T,object>;
                            }
                        }
                    }
                }
            }
        }
#if UNITY_EDITOR
        protected virtual string DispatchEventTestDescription => "Enter a value to dispatch";
        protected virtual string dispatchEventTestDefaultValue=>"";
        protected virtual bool ShowDispatchEventButton => false;
        [Button(ButtonSizes.Large),ShowIf(nameof(ShowDispatchEventButton))]
        void _DispatchEvent()
        {
            var v = qb.Utility.Editor.InputDialog.Show("Input field", DispatchEventTestDescription, dispatchEventTestDefaultValue);
            if (!string.IsNullOrEmpty(v)) 
                DispatchEventFromString(v);
        }
        protected virtual void DispatchEventFromString(string value) 
        { }
#endif

        /// <summary>
        /// Dispatch value throw event channel
        /// </summary>
        /// <param name="value">
        /// The value to dispatch throw the channel
        /// </param>
        /// <param name="checkInvalidSubscriptors">
        /// Flag set by default on true for checking invalid subscriptors resulting of object destruction
        /// and avoid errors.
        /// If the falue is set to true, it is recomended to call the method ClearInvalidSubscriptions()
        /// when global context changed like new scene loading...
        /// </param>
        /// <param name="sender">
        /// The sender which dispatch the event, set to null by default for anonymous sender
        /// </param>

        public void DispatchEvent(T value, object sender,bool checkInvalidSubscriptors=true)
        {
            if (_event != null)
            {
                if(checkInvalidSubscriptors)
                {
                    lock (locker)
                    {
                        //reference.onValueChange.UpdateAndDispatchChangeEvent();
                        var invocationList = _event.GetInvocationList();
                        int validInvocationCount = 0;
                        foreach (var invocation in invocationList)
                        {
                            if (invocation != null && !(invocation.Target.Equals(null)))
                            {
                                validInvocationCount++;
                                (invocation as UnityAction<T,object>).Invoke(value,sender);
                            }
                        }
                        if (validInvocationCount != invocationList.Length)
                        {
                            _event = null;
                            foreach (var invocation in invocationList)
                            {
                                if (invocation != null && !(invocation.Target.Equals(null)))
                                    _event += invocation as UnityAction<T, object>;
#if !NO_DEBUG_LOG_WARNING
                                else
                                {
                                    Debug.LogWarning($"The OnChange provider [{this.name}] try to invoke an action of a null object!");
                                }
#endif
                            }
                        }
                    }
                }
                else
                    _event.Invoke(value,sender);
            }
        }
        /// <summary>
        /// Dispatch value throw event channel
        /// </summary>
        /// <param name="value">
        /// The value to dispatch throw the channel
        /// </param>
        /// <param name="checkInvalidSubscriptors">
        /// Flag set by default on true for checking invalid subscriptors resulting of object destruction
        /// and avoid errors.
        /// If the falue is set to true, it is recomended to call the method ClearInvalidSubscriptions()
        /// when global context changed like new scene loading...
        /// </param>
        public void DispatchEvent(T value, bool checkInvalidSubscriptors = true) => DispatchEvent(value, null, checkInvalidSubscriptors);

        [System.NonSerialized]
        bool isBindedOnSceneUnloadEvent;
        protected override void OnEnable()
        {
            base.OnEnable();
            if (!isBindedOnSceneUnloadEvent)
            {
                SceneManager.sceneUnloaded += OnSceneUnloaded;
                isBindedOnSceneUnloadEvent = true;
            }

        }
        protected override void OnDisable()
        {
            base.OnDisable();
            if (isBindedOnSceneUnloadEvent)
            {
                SceneManager.sceneUnloaded -= OnSceneUnloaded;
                isBindedOnSceneUnloadEvent = false;
            }
        }
        private void OnSceneUnloaded(Scene scene)
        {
            ClearInvalidSubscriptions();
        }

        protected override void Awake()
        {
            base.Awake();
#if UNITY_EDITOR
            var currentPath = AssetDatabase.GetAssetPath(this);
            if (currentPath != null)
            {
                var obj = AssetDatabase.LoadAssetAtPath<EventChannel<T>>(currentPath);
                if (obj != null)
                {
                    var iconGuids = AssetDatabase.FindAssets($"{GetType().Name} t:texture2D", null);
                    if (iconGuids != null && iconGuids.Length > 0)
                    {
                        var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(iconGuids[0]));
                        EditorGUIUtility.SetIconForObject(obj, icon);
                    }
                }
            }
#endif 
        }
    }
}
