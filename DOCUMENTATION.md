# Documentation

> [!NOTE]
> Any help in creating documentation is welcome! 
> - [Create KeyboardListener documentation](https://github.com/Empiree/DeftSharp.Windows.Input/issues/21)
> - [Create MouseListener documentation](https://github.com/Empiree/DeftSharp.Windows.Input/issues/23)
>   
> Please keep your documentation concise and clear. When creating, please pay attention to already existing documentation, e.g. in the [README.md](https://github.com/Empiree/DeftSharp.Windows.Input/blob/main/README.md) file.
> And structure your documentation so that it goes from the most common user cases to the more specific ones.


# Overview

[Keyboard](#keyboard)

- [Keyboard Listener](#keyboardlistener)
- [Keyboard Manipulator](#keyboardmanipulator)
- [Keyboard Binder](#keyboardbinder)
- [Useful classes](#useful-classes)

[Mouse](#mouse)

- [Mouse Listener](#mouselistener)
- [Mouse Manipulator](#mousemanipulator)

[Custom Interceptors](#custom-interceptors)

# Keyboard



## KeyboardListener

This class is intended for processing input events from the keyboard, as well as obtaining various information about its current state. You can use it to subscribe to a key press, a key combination or a sequence of keys.

### Simple key subscription

```c#

var keyboardListener = new KeyboardListener();

keyboardListener.Subscribe(Key.A, key =>
{
    Trace.WriteLine($"The {key} was pressed");
});
```

### Subscription with interval and event type

```c#
var keyboardListener = new KeyboardListener();

keyboardListener.Subscribe(Key.Space, (key, eventType) =>
{
    // This code will be triggered no more than once per second
},
TimeSpan.FromSeconds(1), // Interval of callback triggering
KeyboardEvent.All); // Subscribe to all events (down and up)
```

### Available subscription options

- Subscribe
- SubscribeAll
- SubscribeOnce
- SubscribeSequence
- SubscribeSequenceOnce
- SubscribeCombination
- SubscribeCombinationOnce

## KeyboardManipulator

## KeyboardBinder

## Useful classes

### NumpadListener

# Mouse

## MouseListener

## MouseManipulator

# Custom interceptors
