# qb.Events
Event channel management with scriptable objects and other behavhiour arround unity event 

## CONTENT

**EventChannel**

General event channel based on a ScriptableObject with GUID.
Allows you to subscribe/unsubscribe listeners and dispatch events without direct dependency between senders and receivers.
Supporting safe subscription management and invalid subscription cleanup.

**ECProvider_R**

Provides read-only access to an event channel and allows adding or removing listeners for event notifications.
Manage scriptable object source refernce serialization issue from addressable assets.
>**Remarks**
>
>**An event channel can be used as a serialized field in a Monobehavior attached to a non-addressable asset, otherwise you must use an ECProvider_x class!**

**ECProvider_W**

Provides write-only access to an event channel and allows dispatching events with optional invalid subscriptor checking.
Manage scriptable object source refernce serialization issue from addressable assets.
>**Remarks**
>
>**An event channel can be used as a serialized field in a Monobehavior attached to a non-addressable asset, otherwise you must use an ECProvider_x class!**

**EventChannelDispatcher**
 
Monobehaviour component that implement a public method to dispatch events using a specified event channel provider.

**EventChannelsListener**

Monobehaviour using for listening to multiple event channels and invokes corresponding UnityEvents when events are received.


**EventChannel\<T>**

Abstract base class for event channels that allow dispatching and subscribing to events with a value and sender.
Supporting safe subscription management and invalid subscription cleanup.

**ECProvider_R\<T>**

Same as ECProvider_R applyed on an EventChannel\<T> with a typed value

**ECProvider_W\<T>**

Same as ECProvider_W applyed on an EventChannel\<T> with a typed value

**EventChannelDispatcher\<T>**
 
Monobehaviour component that implement a public method to dispatch events value using a specified event channel provider.

**EventChannelsListener\<T>**
Same as EventChannelsListener but managing event channels with a typed value

#
DispatchEventOnEnable

Monobehaviour component dispatch an Unity event when the behaviour is enabled.

## HOW TO INSTALL

Use the Unity package manager and the Install package from git url option.

- Install at first time,if you haven't already done so previously, the package <mark>[unity-package-manager-utilities](https://github.com/sandolkakos/unity-package-manager-utilities.git)</mark> from the following url: 
  [GitHub - sandolkakos/unity-package-manager-utilities: That package contains a utility that makes it possible to resolve Git Dependencies inside custom packages installed in your Unity project via UPM - Unity Package Manager.](https://github.com/sandolkakos/unity-package-manager-utilities.git)

- Next, install the package from the current package git URL. 
  
  All other dependencies of the package should be installed automatically.

## Dependencies

- https://github.com/quanty-bandit/qb.Pattern.git
- https://github.com/quanty-bandit/qb.Editor-Extensions.git
- [GitHub - codewriter-packages/Tri-Inspector: Free inspector attributes for Unity [Custom Editor, Custom Inspector, Inspector Attributes, Attribute Extensions]](https://github.com/codewriter-packages/Tri-Inspector.git)
