<h1 align="center">DeftSharp.Windows.Input</h1>

<div align="center">
    
[![Nuget version](https://badge.fury.io/nu/DeftSharp.Windows.Input.svg)](https://www.nuget.org/packages/DeftSharp.Windows.Input)
![GitHub License](https://img.shields.io/github/license/Empiree/DeftSharp.Windows.Input?color=rgb(0%2C191%2C255))
![GitHub Stars](https://img.shields.io/github/stars/Empiree/DeftSharp.Windows.Input)

</div>

DeftSharp.Windows.Input is a powerful .NET library designed to handle global keyboard and mouse input events in the Windows OS. It is intended for use in various UI frameworks such as WPF, WinUI, Avalonia, and MAUI, providing a universal solution for all types of Windows applications. 

The library offers a wide range of capabilities, including event subscription, button binding, control over specific input events, and various mouse operations such as tracking clicks and obtaining cursor coordinates. It also provides flexible custom interceptors, allowing users to define their own logic.

The main goal of this library is to provide maximum user-friendliness so that you don't have to write a lot of code. Therefore, it includes many convenient methods that facilitate an intuitive and efficient process of working with input events.

The library is built on P/Invoke native calls.

# Features

* Subscription to keyboard events
* Subscription to mouse events
* Pressing buttons from code
* Changing press frequency
* Changing keys binding
* Prevent input events
* Custom interceptors

# How to Install

The library is published as a [Nuget](https://www.nuget.org/packages/DeftSharp.Windows.Input)

`dotnet add package DeftSharp.Windows.Input`

# Documentation

You can read the complete documentation in the file [DOCUMENTATION](https://github.com/Empiree/DeftSharp.Windows.Input/blob/main/DOCUMENTATION.md). 

> [!NOTE]
> Documentation is in the process of being written.

# Examples

### Simple key subscription

```c#

var keyboardListener = new KeyboardListener();

keyboardListener.Subscribe(Key.Space, key =>
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

### Subscription to mouse move event and get current coordinates

```c#
var mouseListener = new MouseListener();

mouseListener.Subscribe(MouseEvent.Move, () =>
{
  Coordinates coordinates = mouseListener.GetPosition();

  Label.Text = $"X: {coordinates.X} Y: {coordinates.Y}";
});
```
![MouseListenerSample](https://github.com/Empiree/DeftSharp.Windows.Input/assets/60399216/9c9a04f6-cb39-491c-b8de-2cb6b435e112)

### Keyboard/Mouse control from code

```c#
var keyboard = new KeyboardManipulator();
var mouse = new MouseManipulator();

keyboard.Press(Key.Escape);
keyboard.Press(Key.LeftCtrl, Key.V); 

mouse.Click();
mouse.DoubleClick();
mouse.Scroll(150); 
```

### Prevent input events

```c#
var keyboard = new KeyboardManipulator();
var mouse = new MouseManipulator();

// Each press of this button will be ignored
keyboard.Prevent(Key.Delete); 

// Prevent with condition
keyboard.Prevent(Key.Escape, () => 
{
   var currentTime = DateTime.Now;

   return currentTime.Minute > 30;
});

// Prevent mouse scroll            
mouse.Prevent(PreventMouseOption.Scroll);
```

# Requirements

- .NET 7.0 for Windows

# License

This project is licensed under the terms of the MIT license. See the [LICENSE](https://github.com/Empiree/DeftSharp.WPF.Keyboard/blob/main/LICENSE) file for details.

# Contributing

We welcome any [contributions](https://github.com/Empiree/DeftSharp.Windows.Input/blob/main/CONTRIBUTING.md) to the development of this project. Whether you want to report a bug, suggest a new feature, or contribute code improvements, your input is highly valued. Please feel free to submit issues or pull requests through GitHub. Let's make this library even better together!

# Feedback

If you have any ideas or suggestions. You can use this e-mail [deftsharp@gmail.com](mailto:deftsharp@gmail.com) for feedback.
