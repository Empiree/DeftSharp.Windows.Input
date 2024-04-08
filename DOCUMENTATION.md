# Documentation

> [!NOTE]
> Documentation is in the process of being written.

The library provides flexible and powerful functionality for keyboard and mouse control in the Windows OS.

It is built using [P/Invoke](https://learn.microsoft.com/en-us/dotnet/standard/native-interop/pinvoke) methods, with the help of libraries such as [User32](https://en.wikipedia.org/wiki/Microsoft_Windows_library_files) and [Kernel32](https://en.wikipedia.org/wiki/Microsoft_Windows_library_files). 

# Overview

[Keyboard](#keyboard)

- [Keyboard Listener](#keyboardlistener)
- [Keyboard Manipulator](#keyboardmanipulator)
- [Keyboard Binder](#keyboardbinder)
- [Keyboard Info](#keyboardinfo)

[Mouse](#mouse)

- [Mouse Listener](#mouselistener)
- [Mouse Manipulator](#mousemanipulator)
- [Mouse Info](#mouseinfo)

[Custom Interceptors](#custom-interceptors)

[Extensions](#extensions)

# Keyboard

These classes provide global control and observation of the keyboard.

# KeyboardListener

The KeyboardListener class provides the ability to subscribe to global keyboard input events. This allows you, to get the information you need about the user's presses, sequences and key combinations. The whole operation of this class is based on subscriptions, you can subscribe to different events, customizing the configuration to suit your needs.

This class stores active subscriptions in properties: `Keys`, `Sequences` and `Combinations`.

Each object of the KeyboardListener class stores its own subscriptions. Keep this in mind when you use the `Unsubscribe` methods.

> [!NOTE]
> **Best Practice:** Before closing the application, unsubscribe from all events. This allows the application to release all the system resources it is using.

## Available subscription options

- Subscribe
- SubscribeAll
- SubscribeOnce
- SubscribeSequence
- SubscribeSequenceOnce
- SubscribeCombination
- SubscribeCombinationOnce

## Subscribe to the press event

Different ways to subscribe to a button press.

```c#
var keyboardListener = new KeyboardListener();

// Subscription for each click
keyboardListener.Subscribe(Key.Space, key => Trace.WriteLine($"The {key} was pressed"));

// One-time subscription
keyboardListener.SubscribeOnce(Key.Space, key => Trace.WriteLine($"The {key} was pressed"));

// Subscription with interval and event type
keyboardListener.Subscribe(Key.Space, (key, eventType) =>
{
    Trace.WriteLine($"The {key} was pressed")
},
TimeSpan.FromSeconds(1), // Interval of callback triggering
KeyboardEvent.Up); // Subscribe to up events
```

## Unsubscribe from the event

You can unsubscribe from an event using several options. Unsubscribe by GUID, by key, and unsubscribe from all events at once.

```c#
// Subscribe to the event
var subscription = keyboardListener.Subscribe(Key.A, key => { });
            
// 3 different unsubscribe options
keyboardListener.Unsubscribe(subscription.Id);
keyboardListener.Unsubscribe(Key.A);
keyboardListener.UnsubscribeAll(); 
```

## Get the current state of the keys

You can get information about the current state of the keys.

```c#
var isNumLockActive = keyboardListener.IsNumLockActive;
var isCapsLockActive = keyboardListener.IsCapsLockActive;
            
var IsWinPressed = keyboardListener.IsWinPressed;
var IsAltPressed = keyboardListener.IsAltPressed;
var IsCtrlPressed = keyboardListener.IsCtrlPressed;
var isShiftPressed = keyboardListener.IsShiftPressed;

var isSpacePressed = keyboardListener.IsKeyPressed(Key.Space);
```

--- 

# KeyboardManipulator

This class provides the ability to control the keyboard. 

> [!NOTE]
> This class works with a single context. Therefore, all your objects of this class have the same state.

## Features

- Prevent input events 
- Set the press interval 
- Simulate of key presses

## Prevent input events

You can prevent global input events by default or with some condition. All locked keys are stored in the `LockedKeys` collection. The keys will be locked until you call the `Release()` method or the application is completed.

```c#
var keyboard = new KeyboardManipulator();
        
// Each press of this button will be ignored
keyboard.Prevent(Key.Delete); 

// Prevent with condition
keyboard.Prevent(Key.Escape, () => 
{
   var currentTime = DateTime.Now;

   return currentTime.Minute > 30;
});

// Release locked keys
keyboard.ReleaseAll();
```

To check the current state of a button you can use the `IsKeyLocked()` method.

```c#
keyboard.Prevent(Key.Space); 

keyboard.IsKeyLocked(Key.Space); // true
```

Also, the class has a `KeyPrevented` event that fires when a press has been prevented by this class.

```c#
 keyboard.KeyPrevented += args => Trace.WriteLine($"Pressing the {args.KeyPressed} button has been prevented");
```

## Set the press interval

The interval setting allows you to control the frequency of presses. With `SetInterval()` method, we will set a global interval for pressing a key on the keyboard. As with locked buttons, the interval will remain until you remove it or the application is completed.

```c#
var keyboardManipulator = new KeyboardManipulator();
            
// Space will now trigger no more than once per second
keyboardManipulator.SetInterval(Key.Space, TimeSpan.FromSeconds(1));
            
// Remove interval
keyboardManipulator.ResetInterval(Key.Space); 
            
// Remove interval alternative
keyboardManipulator.SetInterval(Key.Space, TimeSpan.Zero);
```

## Simulate of key presses

You can simulate button presses from the keyboard with this class. The simulated keys are fully compatible with other pressed keys. If you press the Shift key and simulate the call of some key, the Shift modifier will be applied to this input.

```c#
var keyboardManipulator = new KeyboardManipulator();

// Single button press            
keyboardManipulator.Press(Key.Space);

// Combination press
keyboardManipulator.Press(Key.LeftCtrl, Key.V);
```
---

# KeyboardBinder

This class allows you to modify the bindings of the specified keys. All bindings are stored in the `BoundedKeys` property.

> [!NOTE]
> This class works with a single context. Therefore, all your objects of this class have the same state.

## Change the button bind

To change the bind of a button, all you need to do is call the `Bind()` method. This method always works, even if a bind already exists, it will just change it to a new one.

```c#
var keyboardBinder = new KeyboardBinder();
            
keyboardBinder.Bind(Key.Q, Key.W); 

// Now any time the 'Q' button is triggered, it will behave like the 'W' button
```

## Swap button bindings

In order to swap the button bindings, we can use the `Swap()` method.

```c#
keyboardBinder.Swap(Key.Q, Key.W);
            
// Alternative option
            
keyboardBinder.Bind(Key.Q, Key.W);
keyboardBinder.Bind(Key.W, Key.Q);
```

## Get current state

Get the current state of the bindings using the `IsKeyBounded()` method, which returns true/false depending on the existence of the binding. And `GetBoundKey()` method which returns the current key to which the specified key belongs.

```c#
keyboardBinder.Bind(Key.Q, Key.W);

keyboardBinder.IsKeyBounded(Key.Q); // true
keyboardBinder.GetBoundKey(Key.Q); // W
keyboardBinder.GetBoundKey(Key.W); // W
```

## Unbind the key

To unbind buttons, you need to call method one of the `Unbind()` method overloads.

```c#
keyboardBinder.Unbind(Key.Q);
keyboardBinder.UnbindAll();
```

---

# KeyboardInfo

This class provides various information about the keyboard, both physical and software.

## Get the current keyboard layout

This method helps to find out the active layout of the user.

```c#

var keyboardInfo = new KeyboardInfo();

// Getting the layout
var layout = keyboardInfo.Layout;
            
Trace.WriteLine(layout.Id); // 1033
Trace.WriteLine(layout.LocaleId); // 1033
Trace.WriteLine(layout.Name); // en-US 
Trace.WriteLine(layout.DisplayName); // English (United States)
```

## Get the current keyboard type

Up to 7 basic keyboard types are supported. 

```c#
var keyboardInfo = new KeyboardInfo();

// Getting the type
var type = keyboardInfo.Type;
            
Trace.WriteLine(type.Name); // IBM enhanced (101- or 102-key) keyboard
Trace.WriteLine(type.Value); // 4
```
---

# Mouse

These classes provide global control and observation of the mouse.

# MouseListener

This class allows you to subscribe to mouse events, as well as receive various information, such as the current cursor coordinates.

> [!NOTE]
> **Best Practice:** Before closing the application, unsubscribe from all events. This allows the application to release all the system resources it is using.

### Subscribe to mouse move event and get current coordinates

```c#
var mouseListener = new MouseListener();

mouseListener.Subscribe(MouseEvent.Move, () =>
     Label.Text = $"X: {mouseListener.Position.X} Y: {mouseListener.Position.Y}");
```
![MouseListenerSample](https://github.com/Empiree/DeftSharp.Windows.Input/assets/60399216/9c9a04f6-cb39-491c-b8de-2cb6b435e112)

---

# MouseManipulator

This class allows you to control the mouse. It is based on the principle of KeyboardManipulator.

```c#
var mouseManipulator = new MouseManipulator();

mouseManipulator.DoubleClick();
mouseManipulator.Scroll(150);            
mouseManipulator.Click(100, 100, MouseButton.Right);
```

---

# MouseInfo

---

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
        
        Trace.WriteLine($"Failed {args.Event} by {failureReason}");
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

Now let's run our project in Debug mode and test their work:

![image](https://github.com/Empiree/DeftSharp.Windows.Input/assets/60399216/31add1c6-6b2d-4844-8765-979f4dfcfa38)

In the console, we can see that the mouse button events have fired. And mouse wheel scrolling was blocked by `ScrollDisabler` class. If we need to disable this interceptor, it is enough to call the `Unhook` method.

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
            
mouseManipulator.Prevent(PreventMouseEvent.Scroll);

mouseManipulator.InputPrevented += mouseEvent => 
     Trace.WriteLine($"Failed {mouseEvent} by MouseManipulator");
```

---

# Extensions

Additional functionality of the library.

# Classes

## NumpadListener

This class is a decorator over the [KeyboardListener](#keyboardlistener) class. It allows you to easily subscribe to all Numpad numeric keys. 

```c#
var keyboardListener = new KeyboardListener();
var numpadListener = new NumpadListener(keyboardListener);
            
// 0-9 numpad buttons
numpadListener.Subscribe(number =>
{
    Trace.WriteLine($"The {number} was pressed");
});
            
// ...
            
numpadListener.Unsubscribe();
```

# Methods

## ToUnicode()

This extension method is applied to enum `Key`. It returns the interpretation of the key as a unicode string depending on your current keyboard layout.

```c#

var key = Key.Z;
            
Trace.WriteLine(key.ToUnicode());

// English layout: z
// German layout: y
// Russian layout: я
```
