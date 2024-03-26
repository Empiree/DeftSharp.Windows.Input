# DeftSharp.Windows.Input

[![Nuget version](https://badge.fury.io/nu/DeftSharp.Windows.Input.svg)](https://www.nuget.org/packages/DeftSharp.Windows.Input)
![GitHub License](https://img.shields.io/github/license/Empiree/DeftSharp.Windows.Input?color=rgb(0%2C191%2C255))

DeftSharp.Windows.Input is a powerful C# library designed to handle all keyboard and mouse input events in the Windows OS. It is intended for use in various UI frameworks such as WPF, WinUI, Avalonia, and MAUI, providing a universal solution for all types of Windows applications. 

The library offers a wide range of capabilities, including event subscription, button binding, control over specific input events, and various mouse operations such as tracking clicks and obtaining cursor coordinates. It also provides flexible custom interceptors, allowing users to define their own logic.

The main goal of this library is to provide maximum user-friendliness so that you don't have to write a lot of code. Therefore, it includes many convenient methods that facilitate an intuitive and efficient process of working with input events.

The library is built on P/Invoke native calls.

# How to Install

The library is published as a [Nuget](https://www.nuget.org/packages/DeftSharp.Windows.Input)

`dotnet add package DeftSharp.Windows.Input`

# Features

* Subscription to keyboard events
* Subscription to mouse events
* Pressing buttons from code
* Blocking any input events
* Changing key binding
* Custom interceptors

# Examples

## KeyboardListener

This class allows you to subscribe to keyboard press events, their sequence and combination. Also, provides various information about the current status of certain buttons.

### Simple key subscription

```c#

var keyboardListener = new KeyboardListener();

keyboardListener.Subscribe(Key.A, key =>
{
    Trace.WriteLine($"The {key} was pressed");
});
```

### One-time subscription

```c#
var keyboardListener = new KeyboardListener();

keyboardListener.SubscribeOnce(Key.Escape, key =>
{
    this.Close();
});

```

### Subscription with interval and event type

```c#
var keyboardListener = new KeyboardListener();

keyboardListener.Subscribe(Key.Space, key =>
{
    // This code will be triggered no more than once per second.
},
TimeSpan.FromSeconds(1), // Interval of callback triggering
KeyboardEvent.KeyUp); // Subscribe to KeyUp event
```

### Available subscription methods: 

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

This class provides the ability to control the keyboard. It allows you to prevent pressing a key and press key or their combination from code.

### Pressing a key from the code

```c#
var keyboardManipulator = new KeyboardManipulator();

keyboardManipulator.Press(Key.A); 
```

### Prevent key pressing

```c#
var keyboardManipulator = new KeyboardManipulator();

keyboardManipulator.Prevent(Key.Delete); // Each press of this button will be ignored
```

## KeyboardBinder

This class provides the option to change the bind of the specified button.

### Change the button bind

```c#
var keyboardBinder = new KeyboardBinder();
            
keyboardBinder.Bind(Key.Q, Key.W); 

// Now any time the 'Q' button is triggered, it will behave like the 'W' button
```

> [!NOTE]
> Prevented and bounded buttons are shared among all class objects. You don't have to worry that an object in this class has locked a particular button and you no longer have access to that object.

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

### Click the right mouse button on the specified coordinates

```c#
var mouseManipulator = new MouseManipulator();
            
mouseManipulator.Click(x:100, y:100, MouseButton.Right);
```

### Prohibit mouse button pressing

```c#
mouseManipulator.Prevent(PreventMouseOption.LeftButton);
```
> [!NOTE]
> Be careful when using this method. You may completely block the operation of your mouse.

## Custom Interceptors

Version [0.6](https://github.com/Empiree/DeftSharp.Windows.Input/releases/v0.6) introduced the ability to create your own interceptors. This means that if your use case is unique and requires its own implementation, you can create a new interceptor, similar to KeyboardListener or KeyboardManipulator!

To create an interceptor you need to inherit from `MouseInterceptor` or `KeyboardInterceptor` and implement `IsInputAllowed` method. To activate/deactivate it you need to call the `Hook` or `Unhook` method.

Available methods: 

- `IsInputAllowed` - сalled when a system input event is triggered and responsible for event blocking
 
- `OnInputSuccess` - called if the input was processed successfully and no interceptor blocked it

- `OnInputFailure` - called if the event was blocked by one or more interceptors. In it we will get the list of these interceptors

### Examples

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

> [!NOTE]
> The implementation of these 2 interceptors can be placed in one interceptor, but it is better to separate it. So that each is responsible for its own task.

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

mouseManipulator.ClickPrevented += mouseEvent => 
     Trace.WriteLine($"Failed {mouseEvent} by: MouseManipulator");
```

# Requirements

- .NET 7.0 for Windows

# License

This project is licensed under the terms of the MIT license. See the [LICENSE](https://github.com/Empiree/DeftSharp.WPF.Keyboard/blob/main/LICENSE) file for details.

# Contributing

We welcome any [contributions](https://github.com/Empiree/DeftSharp.Windows.Input/blob/main/CONTRIBUTING.md) to the development of this project. Whether you want to report a bug, suggest a new feature, or contribute code improvements, your input is highly valued. Please feel free to submit issues or pull requests through GitHub. Let's make this library even better together!

# Feedback

If you have any ideas or suggestions. You can use this e-mail [deftsharp@gmail.com](mailto:deftsharp@gmail.com) for feedback.
