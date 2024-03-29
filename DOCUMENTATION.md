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

> [!NOTE]
> Each object of the KeyboardListener class stores its own subscriptions. Keep this in mind when you use the `UnsubscribeAll()` method.

## KeyboardManipulator

This class provides the ability to control the keyboard. This allows you to disable key presses, set their press interval, and simulate any key presses directly from the code.

> [!NOTE]
> Prevented keys are shared among all class objects. You don't have to worry that an object in this class has locked a particular button and you no longer have access to that object.

## KeyboardBinder

This class provides the option to change the bind of the specified button.

### Change the button bind

```c#
var keyboardBinder = new KeyboardBinder();
            
keyboardBinder.Bind(Key.Q, Key.W); 

// Now any time the 'Q' button is triggered, it will behave like the 'W' button
```

## Useful classes

### NumpadListener

# Mouse

## MouseListener

This class allows you to subscribe to mouse events, as well as receive various information, such as the current cursor coordinates.

### Subscribe to mouse move event and get current coordinates

```c#
var mouseListener = new MouseListener();

mouseListener.Subscribe(MouseEvent.Move, () =>
{
  Coordinates coordinates = mouseListener.GetPosition();

  Label.Text = $"X: {coordinates.X} Y: {coordinates.Y}";
});
```
![MouseListenerSample](https://github.com/Empiree/DeftSharp.Windows.Input/assets/60399216/9c9a04f6-cb39-491c-b8de-2cb6b435e112)

## MouseManipulator

This class allows you to control the mouse. It is based on the principle of KeyboardManipulator.

```c#
var mouseManipulator = new MouseManipulator();

mouseManipulator.DoubleClick();
mouseManipulator.Scroll(150);            
mouseManipulator.Click(100, 100, MouseButton.Right);
```

# Custom interceptors

Custom interceptors provide the ability for users to manage incoming events themselves. This is a bit more complex than using already created interceptors such as [KeyboardListener](#keyboardlistener). But in some cases you can't do without it.

The work of interceptors can be visualized as a pipeline. Before an incoming event is processed by the system, it passes through all registered interceptors and if at least one interceptor blocks an incoming event, it will not be processed. Each interceptor has its own name, which corresponds to the name of the class. And also the type of interceptor. 

Interceptors are registered using the `Hook` method. A call to this method adds an interceptor to the pipeline, and a call to the `Unhook` method removes it.

## Creating 

To create an interceptor you need to inherit from `MouseInterceptor` or `KeyboardInterceptor` and implement `IsInputAllowed` method.

`IsInputAllowed` method is called when any input event occurs. It decides whether we need to block this event or not.

Also, interceptors have two optional methods, such as `OnInputSuccess` and `OnInputFailure`. The `OnInputSuccess` method is called when the input event is successfully processed by all interceptors. In this method we can get the details of the event and execute the code we need. The `OnInputFailure` method is called if the event was blocked, and besides the event details we can also get the list of interceptors that did not approve this input.

> [!NOTE]
> **Best Practice:** Design interceptors so that each interceptor solves only one specific problem.

## Examples

As an example we will implement 2 interceptors. One of them will block mouse scroll event. The second one will output mouse input events.

Interceptor for blocking mouse scroll events:

```c#
public class ScrollDisabler : MouseInterceptor
{
    protected override bool IsInputAllowed(MouseInputArgs args)
    {
        if (args.Event is MouseInputEvent.Scroll)
            return false; // disallow mouse scroll input
        
        return true; // all other input events can be processed
    }
}
```

Interceptor for logging mouse events:

```c#
public class MouseLogger : MouseInterceptor
{
    // Always allow input because it's a logger
    protected override bool IsInputAllowed(MouseInputArgs args) => true;

    // If the input event was successfully processed
    protected override void OnInputSuccess(MouseInputArgs args)
    {
        if (args.Event is MouseInputEvent.Move) // Don't log a move event
            return;
        
        Trace.WriteLine($"Processed {args.Event}");
    }

    // If the input event has been blocked
    protected override void OnInputFailure(MouseInputArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {
        var failureReason = failedInterceptors.ToNames();
        
        Trace.WriteLine($"Failed {args.Event} by: {failureReason}");
    }
}
```

In order to use them, we need to call the `Hook` method.

```c#
var scrollDisabler = new ScrollDisabler();
var mouseLogger = new MouseLogger();
            
scrollDisabler.Hook();
mouseLogger.Hook();
```

Now let's run our project and test their work:

![image](https://github.com/Empiree/DeftSharp.Windows.Input/assets/60399216/5126e37f-e928-4a18-aa32-c8ef7141e538)

In the Debug console, we can see that the mouse button events have fired. And mouse wheel scrolling was blocked by `ScrollDisabler` class. If we need to disable this interceptor, it is enough to call the `Unhook` method.

It was a simple implementation of a custom interceptor. In your scenarios they can be much larger and with stronger logic.

Identical functionality using existing interceptors:

```c#
var mouseListener = new MouseListener();
var mouseManipulator = new MouseManipulator();

mouseListener.SubscribeAll(mouseEvent =>
{
     if (mouseEvent is MouseInputEvent.Move)
         return;
                
     Trace.WriteLine($"Processed {mouseEvent}");
});
            
mouseManipulator.Prevent(PreventMouseOption.Scroll);

mouseManipulator.InputPrevented += mouseEvent => 
     Trace.WriteLine($"Failed {mouseEvent} by: MouseManipulator");
```
