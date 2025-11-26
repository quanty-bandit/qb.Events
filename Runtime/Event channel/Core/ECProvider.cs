
using qb.Pattern;
using System;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

namespace qb.Events
{

    [Serializable]
    public abstract class ECProvider
    {
        [Flags] public enum EventChannelAccess { None = 0, Read = 1, Write = 2, ReadWrite = 3 };

        [SerializeField, PropertyOrder(-2)]
        protected EventChannel channel;

        protected virtual EventChannelAccess channelAcessMode => EventChannelAccess.None;

        public bool IsFilled => channel != null;

        bool sourceIsChecked;
        protected void CheckSource()
        {
            if (sourceIsChecked) return;
            var c = SOWithGUID.GetSourceFromGUID(channel) as EventChannel;
            if (c != null && c != channel)
                channel = c;
            sourceIsChecked = true;
        }

        //protected string CHANNEL_NOT_FILLED_ERROR => $"channel field of ECProvider with {channelAcessMode} access is not filled!";
    }
    /// <summary>
    /// Event channel provider with read channel acces
    /// Manage event channel listener subscription and scriptable object source serialization issue from addressable object.
    /// </summary>

    [Serializable]
    public class ECProvider_R: ECProvider
    {
        [GUIColor("00FFFF")]
        [ReadOnly, ShowInInspector, PropertyOrder(-1), HideLabel]
        protected override EventChannelAccess channelAcessMode => EventChannelAccess.Read;
        public void AddListener(UnityAction<object> action)
        {
            if(channel)
            {
                CheckSource();
                channel.Event += action;
            }
            /*else
                Debug.LogError($"Can't add event listener because: {CHANNEL_NOT_FILLED_ERROR}");
            */
        }
        public void RemoveListener(UnityAction<object> action)
        {
            if(channel)
                channel.Event -= action;
            /*else
                Debug.LogError($"Can't remove event listener because: {CHANNEL_NOT_FILLED_ERROR}");
            */
        }
    }
    /// <summary>
    /// Event channel provider with write channel acces
    /// Manage channel event dispatch subscription and scriptable object source serialization issue from addressable object.
    /// </summary>
    [Serializable]
    public class ECProvider_W : ECProvider
    {
        [GUIColor("FFFF00")]
        [ReadOnly, ShowInInspector, PropertyOrder(-1), HideLabel]
        protected override EventChannelAccess channelAcessMode => EventChannelAccess.Write;

        /// <summary>
        /// Dispatch event  
        /// </summary>
        /// <param name="sender">
        /// The sender which dispatch the event, set to null by default for anonymous sender
        /// </param>
        /// <param name="checkInvalidSubscriptors">
        /// Flag set by default on true for checking invalid subscriptors resulting of object destruction
        /// and avoid errors.
        /// If the falue is set to true, it is recomended to call the method ClearInvalidSubscriptions()
        /// when global context changed like new scene loading...
        /// </param>
        public void DispatchEvent(object sender = null, bool checkInvalidSubscriptors = true)
        {
            if(channel)
            {
                CheckSource();
                channel.DispatchEvent(sender,checkInvalidSubscriptors);
            }
            /*else
                Debug.LogError($"Can't dispatch event because: {CHANNEL_NOT_FILLED_ERROR}");
            */
        }
    }


    [Serializable]
    public abstract class ECProvider<T>
    {
        [Flags] public enum EventChannelAccess { None = 0, Read = 1, Write = 2, ReadWrite = 3 };

        [SerializeField, PropertyOrder(-2)]
        protected EventChannel<T> channel;
       
        protected virtual EventChannelAccess channelAcessMode => EventChannelAccess.None;

        bool sourceIsChecked;
        protected void CheckSource()
        {
            if (sourceIsChecked) return;
            var c = SOWithGUID.GetSourceFromGUID(channel) as EventChannel<T>;
            if (c != null && c != channel)
                channel = c;
            sourceIsChecked = true;
        }
        //protected string CHANNEL_NOT_FILLED_ERROR => $"channel field of ECProvider from Type<{typeof(T)}> with {channelAcessMode} mode is not filled!";

    }

    /// <summary>
    /// Event channel provider with read channel acces
    /// Manage event channel listener subscription and scriptable object source serialization issue from addressable object.
    /// </summary>
    [Serializable]
    public class ECProvider_R<T> : ECProvider<T>
    {
        [GUIColor("00FFFF")]
        [ReadOnly, ShowInInspector, PropertyOrder(-1), HideLabel]
        protected override EventChannelAccess channelAcessMode => EventChannelAccess.Read;

        public void AddListener(UnityAction<T,object> action)
        {
            if(channel)
            {
                CheckSource();
                channel.Event += action;
            }
            /*
            else
                Debug.LogError($"Can't add listener because: {CHANNEL_NOT_FILLED_ERROR}");
            */

        }
        public void RemoveListener(UnityAction<T,object> action)
        {
            if(channel)
                channel.Event -= action;
            /*else
                Debug.LogError($"Can't add remove listener because:  {CHANNEL_NOT_FILLED_ERROR}");
            */
        }
}

    /// <summary>
    /// Event channel provider with write channel acces
    /// Manage channel event dispatch subscription and scriptable object source serialization issue from addressable object.
    /// </summary>
    [Serializable]
    public class ECProvider_W<T> : ECProvider<T>
    {
        [GUIColor("FFFF00")]
        [ReadOnly, ShowInInspector, PropertyOrder(-1), HideLabel]
        protected override EventChannelAccess channelAcessMode => EventChannelAccess.Write;

        /// <summary>
        /// Dispatch value throw event channel
        /// </summary>
        /// <param name="value">
        /// The value to dispatch throw the channel
        /// </param>
        /// <param name="sender">
        /// The sender which dispatch the event, set to null by default for anonymous sender
        /// </param>
        /// <param name="checkInvalidSubscriptors">
        /// Flag set by default on true for checking invalid subscriptors resulting of object destruction
        /// and avoid errors.
        /// If the falue is set to true, it is recomended to call the method ClearInvalidSubscriptions()
        /// when global context changed like new scene loading...
        /// </param>

        public void DispatchEvent(T value, object sender = null,bool checkInvalidSubscriptions=true)
        {
            if(channel)
            {
                CheckSource();
                channel.DispatchEvent(value,sender, checkInvalidSubscriptions);
            }
            /*
            else
                Debug.LogError($"Can't dispatch event because: {CHANNEL_NOT_FILLED_ERROR}");
            */
        }
    }
}
