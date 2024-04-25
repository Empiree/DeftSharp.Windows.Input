# Documentation

> [!NOTE]
> Documentation is in the process of being written.

## Overview

- [Introduction](#introduction)
- [Getting started](#getting-started)
- [Reference](#reference)
  - [Keyboard](#keyboard)
     - [Keyboard Listener](#keyboardlistener)
     - [Keyboard Manipulator](#keyboardmanipulator)
     - [Keyboard Binder](#keyboardbinder)
     - [Keyboard Info](#keyboardinfo)
  - [Mouse](#mouse)
     - [Mouse Listener](#mouselistener)
     - [Mouse Manipulator](#mousemanipulator)
     - [Mouse Info](#mouseinfo)
- [Custom Interceptors](#custom-interceptors)
    - [Creating custom interceptors](#creating-custom-interceptors)
    - [Examples of custom interceptors](#examples-of-custom-interceptors)
- [Extensions](#extensions)
    - [Classes](#classes)
        - [NumpadListener](#numpadlistener)
    - [Methods](#methods)
        - [Key.ToUnicode()](#keytounicode) 


# Introduction 

The DeftSharp library provides flexible and powerful functionality for keyboard and mouse control in the Windows OS. 

It is built using [P/Invoke](https://learn.microsoft.com/en-us/dotnet/standard/native-interop/pinvoke) methods, with the help of libraries such as [User32](https://en.wikipedia.org/wiki/Microsoft_Windows_library_files) and [Kernel32](https://en.wikipedia.org/wiki/Microsoft_Windows_library_files). 

The principle of the library is based on a chain of interceptors that are formed into a pipeline. Before an incoming event can be successfully processed by the system, it must pass through all registered interceptors. If the pipeline is empty, the library will not use system resources and will not affect their operation.

New interceptors are registered by your interaction with classes such as [KeyboardListener](#keyboardlistener). This allows you to observe special events and have control over them. If the provided classes do not fit your needs, you can create your own [custom interceptor](#custom-interceptors).

The functionality of the library is divided into two types of functionality, temporary and permanent changes. Temporary changes are active only while the program is running, such as subscribing to input events. Permanent changes do not depend on your application, such as changes in mouse speed. Permanent changes are marked with the `SystemChanges` attribute.

# Getting started

### 1. Prerequisites

You should make sure that your application fits the requirements of this library.

- Version .NET 7 or higher ([Install](https://dotnet.microsoft.com/en-us/download/dotnet))
- Any Windows UI framework (WPF, WinUI, Avalonia, and MAUI)

### 2. Installation

Next, you need to use the [Nuget](https://www.nuget.org/packages/DeftSharp.Windows.Input) package manager and install this library.

![image](https://github.com/Empiree/DeftSharp.Windows.Input/assets/60399216/dc1dfd62-1aea-4bbd-81fb-e48104d3b46e)

Or you can use the command in the console:

`dotnet add package DeftSharp.Windows.Input`

### 3. Subscribe to the event

Now, using a basic WPF application as an example, we'll subscribe to our first event, the Escape button press. When this button is pressed, our application should close, regardless of whether our window is currently active or not.

To do this, we'll go into `MainWindow.xaml.cs` and add a little logic. We need to create an object of the [KeyboardListener](#keyboardlistener) class, with which we can subscribe to global keyboard input events. 

![image](https://github.com/Empiree/DeftSharp.Windows.Input/assets/60399216/03970fe2-f8ca-4a2b-ace0-86c3d3c4eada)

### 4. Explore the library's features

The DeftSharp library has many different classes for handle user input. Below, you will be able to familiarize yourself with all of them.

# Reference

In this section, you can familiarize yourself with all the existing classes.

## Keyboard

These classes provide global control and observation of the keyboard.

### **KeyboardListener**

The **KeyboardListener** class provides the ability to subscribe to global keyboard input events. This allows you, to get the information you need about the user's presses, sequences and key combinations. The whole operation of this class is based on subscriptions, you can subscribe to different events, customizing the configuration to sui
t your needs.

This class stores active subscriptions in properties: `Keys`, `Sequences` and `Combinations`.

Each object of the KeyboardListener class stores its own subscriptions. Keep this in mind when you use the `Unsubscribe` methods.

> [!NOTE]
> :bulb: **Best Practice:** Before closing the application, unsubscribe from all events. This will allow the application to correctly release all system resources it uses.

#### **Subscribe to the press event**

In order to subscribe to press events, you need to call one of the subscribe method. 

```c#
var keyboardListener = new KeyboardListener();

// Subscription for each click
keyboardListener.Subscribe(Key.Space, () => Trace.WriteLine($"The Space was pressed"));

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

Each subscription method returns a subscription object with all the details. Including the unique identifier and event type.

#### **Unsubscribe from the event**

You can unsubscribe from an event using several options. Unsubscribe by GUID, by key, and unsubscribe from all events at once.

```c#
// Subscribe to the event
var subscription = keyboardListener.Subscribe(Key.A, key => { });
            
// 3 different unsubscribe options
keyboardListener.Unsubscribe(); 
keyboardListener.Unsubscribe(Key.A);
keyboardListener.Unsubscribe(subscription.Id);
```

#### **Get the current state of the keys**

You can get information about the current state of the keys. To do this, you can use the already created properties, or you can call the `IsKeyPressed()` method.

```c#
var isNumLockActive = keyboardListener.IsNumLockActive;
var isCapsLockActive = keyboardListener.IsCapsLockActive;

var isSpacePressed = keyboardListener.IsKeyPressed(Key.Space);
```

--- 

### **KeyboardManipulator**

The **KeyboardManipulator** class provides the ability to control the keyboard.

> [!NOTE]
> :pushpin: This class works with a single context. Therefore, all your objects of this class have the same state.

**Features**

- Simulate keyboard input
- Prevent input events 
- Set the press interval 

#### **Simulate keyboard input**

You can simulate pressing buttons from the keyboard. To do this, you need to use the `Press()` method, which accepts a collection of keys. The simulated keys are fully compatible with your keyboard, so different active modifiers will be applied to them, such as the Shift key.

```c#
var keyboard = new KeyboardManipulator();

// Single button press            
keyboard.Press(Key.Space);

// Combination press
keyboard.Press(Key.LeftCtrl, Key.V);
```

If you need to trigger a specific event, you can use `Simulate()` methods that directly simulate keyboard input events.

```c#
// Hold the button
keyboard.Simulate(Key.LeftShift, KeyboardInputEvent.KeyDown); 

// Release the button            
keyboard.Simulate(Key.LeftShift, KeyboardInputEvent.KeyUp); 
```

#### **Prevent input events**

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
keyboard.Release();
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

#### **Set the press interval**

The interval setting allows you to control the frequency of presses. With `SetInterval()` method, you will set a global interval for pressing a key on the keyboard. As with locked buttons, the interval will remain until you remove it or the application is completed.

```c#
var keyboard = new KeyboardManipulator();
            
// Space will now trigger no more than once per second
keyboard.SetInterval(Key.Space, TimeSpan.FromSeconds(1));
            
// Remove interval
keyboard.ResetInterval(Key.Space); 
            
// Remove interval alternative
keyboard.SetInterval(Key.Space, TimeSpan.Zero);
```

---

### **KeyboardBinder**

The **KeyboardBinder** class provides the ability to modify the bindings of the specified keys. All bindings are stored in the `BoundedKeys` property.

> [!NOTE]
> :pushpin: This class works with a single context. Therefore, all your objects of this class have the same state.

#### **Change the button bind**

To change the button binding, you must call the `Bind()` method. This method always works, even if a bind already exists, it will just change it to a new one.

```c#
var keyboardBinder = new KeyboardBinder();
            
keyboardBinder.Bind(Key.Q, Key.W); 

// Now any time the 'Q' button is triggered, it will behave like the 'W' button
```

#### **Swap button bindings**

In order to swap the button bindings, you can use the `Swap()` method.

```c#
var keyboardBinder = new KeyboardBinder();

keyboardBinder.Swap(Key.Q, Key.W);
            
// Alternative option
            
keyboardBinder.Bind(Key.Q, Key.W);
keyboardBinder.Bind(Key.W, Key.Q);
```

#### **Get current state**

Get the current state of the bindings using the `IsKeyBounded()` method, which returns true/false depending on the existence of the binding. And `GetBoundKey()` method which returns the current key to which the specified key belongs.

```c#
var keyboardBinder = new KeyboardBinder();

keyboardBinder.Bind(Key.Q, Key.W);

keyboardBinder.IsKeyBounded(Key.Q); // true
keyboardBinder.GetBoundKey(Key.Q); // W
keyboardBinder.GetBoundKey(Key.W); // W
```

#### **Unbind the key**

To unbind buttons, you need to call one of the `Unbind()` method overloads.

```c#
var keyboardBinder = new KeyboardBinder();

keyboardBinder.Bind(Key.Q, Key.W);

keyboardBinder.Unbind(Key.Q);

// Unbind all the buttons, alternatively

keyboardBinder.Unbind();
```

---

### **KeyboardInfo**

This class provides various information about the keyboard, both physical and software.

#### **Get the current keyboard layout**

This method helps to find out the active layout of the user.

```c#

var keyboardInfo = new KeyboardInfo();

// Getting the layout
var layout = keyboardInfo.GetLayout();
            
Trace.WriteLine(layout.Id); // 1033
Trace.WriteLine(layout.LocaleId); // 1033
Trace.WriteLine(layout.Name); // en-US 
Trace.WriteLine(layout.DisplayName); // English (United States)
```

#### **Get the current keyboard type**

Up to 7 basic keyboard types are supported. 

```c#
var keyboardInfo = new KeyboardInfo();

// Getting the type
var type = keyboardInfo.GetKeyboardType();
            
Trace.WriteLine(type.Name); // IBM enhanced (101- or 102-key) keyboard
Trace.WriteLine(type.Value); // 4
```
---

## Mouse

These classes provide global control and observation of the mouse.

### **MouseListener**

The **MouseListener** class provides the ability to use global mouse input events. This gives you information about mouse clicks, scrolling, mouse movement, and more. 

You can subscribe to different events from the MouseListener class to enable these features and customize them.

This class stores active subscriptions in the `Subscriptions` property.

Each object of the MouseListener class stores its own subscriptions. Keep this in mind when you use the `Unsubscribe` methods.

> [!NOTE]
> :bulb: **Best Practice:** Before closing the application, unsubscribe from all events. This correctly releases the system resources used by the application.

#### **Subscribing to mouse listener events**

Call one of the following methods to subscribe to a mouse event.

```c#
var mouseListener = new MouseListener();

// Subscription for left button down event
mouseListener.Subscribe(MouseEvent.LeftButtonDown,
    () => Trace.WriteLine($"The left mouse button was pressed"));

// One-time subscription            
mouseListener.SubscribeOnce(MouseEvent.RightButtonDown,
    () => Trace.WriteLine($"The right mouse button was pressed"));

// Subscription to generic mouse down event that will trigger on any mouse button            
mouseListener.Subscribe(MouseEvent.ButtonDown, mouseEvent 
     => Trace.WriteLine($"The {mouseEvent} was pressed"));
```

Each subscription method returns an object with event details, such as a unique identifier and event type.

#### **Unsubscribing from mouse listener events**

You can unsubscribe from an event using several options. Unsubscribe by GUID, by key, and unsubscribe from all events at once.

```c#
// Subscribe to the event
var subscription = mouseListener.Subscribe(MouseEvent.MiddleButtonDown, () => { });

// 3 different unsubscribe options
mouseListener.Unsubscribe(); 
mouseListener.Unsubscribe(subscription.Id);
mouseListener.Unsubscribe(MouseEvent.MiddleButtonDown);
```

#### **Get the current state of the keys**

You can get information about the current state of the keys. To do this you can call the `IsKeyPressed()` method.

```c#
var isLeftButtonPressed = mouseListener.IsKeyPressed(MouseButton.Left);
```

#### **Getting the mouse position**
 
You can get the position of the mouse by using the `Position` property. It will return the Point class with an X and Y value.

```c#
var mouseListener = new MouseListener();

var position = mouseListener.Position;
            
Trace.WriteLine($"X: {position.X} Y: {position.Y}"); // X: 943 Y: 378
```

---

### **MouseManipulator**

The **MouseManipulator** class provides the ability to control the mouse. 

> [!NOTE]
> :pushpin: This class works with a single context. Therefore, all your objects of this class have the same state.

**Features**

- Simulate mouse input
- Prevent input events 
- Global mouse configuration

#### **Simulate mouse input**

You can simulate mouse clicks events.

```c#
var mouse = new MouseManipulator();
      
mouse.Click();
mouse.DoubleClick();
```

You can also set the coordinates of moves and clicks.

```c#
// Sets the mouse cursor to X:840 Y:420 coordinates
mouse.SetPosition(840, 420); 

// Right click by coordinates
mouse.Click(500, 500, MouseButton.Right);
```

You can scroll the mouse wheel in the direction you want.

```c#
// Scroll up
mouse.Scroll(450);
            
// Scroll down
mouse.Scroll(-150);
```

#### **Prevent input events**

You can prevent global input events by default or with some condition. All locked keys are stored in the `LockedKeys` collection. The keys will be locked until you call the `Release()` method or the application is completed.

```c#
var mouse = new MouseManipulator();
        
// Each scroll event will be ignored
mouse.Prevent(PreventMouseEvent.Scroll); 

// Prevent with condition
mouse.Prevent(PreventMouseEvent.RightButton, () => 
{
   var currentTime = DateTime.Now;

   return currentTime.Minute > 30;
});

// Release locked keys
mouse.Release();
```

To check the current state of a key you can use the `IsKeyLocked()` method.

```c#
mouse.Prevent(PreventMouseEvent.Move); 

mouse.IsKeyLocked(PreventMouseEvent.Move); // true
```

Also, the class has a `InputPrevented` event that fires when a input has been prevented by this class.

```c#
mouse.InputPrevented += mEvent => Trace.WriteLine($"The {mEvent} event was prevented");
```

#### **Global mouse configuration**

You can make permanent changes with this class. They will be active even after your application is terminated.

```c#
var mouse = new MouseManipulator();
            
mouse.SetMouseSpeed(5); // changes the mouse speed on your system
```
---

### **MouseInfo**

This class provides various information about the mouse, both physical and software.

#### **Get the current mouse speed**

```c#
var mouseInfo = new MouseInfo();
            
// Getting the speed
var speed = mouseInfo.GetSpeed();
            
Trace.WriteLine(speed); // 10
```

You can change the speed of the mouse using the [MouseManipulator](#mousemanipulator) class.

---

# Custom interceptors

Custom interceptors provide the ability for users to manage incoming events themselves. This is a bit more complex than using already created interceptors such as [KeyboardListener](#keyboardlistener). But in some cases you can't do without it.

The work of interceptors can be visualized as a pipeline. Before an incoming event is processed by the system, it passes through all registered interceptors and if at least one interceptor blocks an incoming event, it will not be processed. Each interceptor has its own name, which corresponds to the name of the class. And also the type of interceptor. 

Interceptors are registered using the `Hook` method. A call to this method adds an interceptor to the pipeline, and a call to the `Unhook` method removes it.

## Creating custom interceptors

To create an interceptor you need to inherit from `MouseInterceptor` or `KeyboardInterceptor` and implement `IsInputAllowed` method.

`IsInputAllowed` method is called when any input event occurs. It decides whether we need to block this event or not.

Also, interceptors have two optional methods, such as `OnInputSuccess` and `OnInputFailure`. The `OnInputSuccess` method is called when the input event is successfully processed by all interceptors. In this method we can get the details of the event and execute the code we need. The `OnInputFailure` method is called if the event was blocked, and besides the event details we can also get the list of interceptors that did not approve this input.

> [!NOTE]
> :bulb: **Best Practice:** Design interceptors so that each interceptor solves only one specific problem.

## Examples of custom interceptors

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

## Classes

### NumpadListener

The **NumpadListener** class allows you to easily subscribe to numpad buttons. It is a decorator over the [KeyboardListener](#keyboardlistener) class. To create an object of this class, it needs to be passed an existing KeyboardListener through which it will create subscriptions. 

Using the `Subscribe()` method, it subscribes to each numeric Numpad key. The method has a required parameter `Action<Key>` which is triggered when one of the buttons is pressed. To unsubscribe from all created subscriptions, you need to call the `Unsubscribe()` method.

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

## Methods

### Key.ToUnicode()

This extension method is applied to [Key](https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.key) enum. It returns the interpretation of the key as a unicode string depending on your current keyboard layout. If the key cannot be represented in text format, `String.Empty` will be returned.

```c#
var key = Key.Z;
            
Trace.WriteLine(key.ToUnicode());

// English layout: z
// German layout: y
// Russian layout: я
```
