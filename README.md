<h1 align="center">DeftSharp.Windows.Input</h1>

<div align="center">
    
[![Nuget version](https://badge.fury.io/nu/DeftSharp.Windows.Input.svg)](https://www.nuget.org/packages/DeftSharp.Windows.Input)
![GitHub License](https://img.shields.io/github/license/Empiree/DeftSharp.Windows.Input?color=rgb(0%2C191%2C255))
[![Join the chat at https://gitter.im/DeftSharp/DeftSharp](https://badges.gitter.im/Join%20Chat.svg)](https://app.gitter.im/#/room/#deftsharp-windows-input:gitter.im)
![GitHub Stars](https://img.shields.io/github/stars/Empiree/DeftSharp.Windows.Input)

</div>

A powerful .NET library designed to control and manage the keyboard and mouse in the Windows OS. It is intended for use in various UI frameworks such as WPF, WinUI, Avalonia, and MAUI, providing a universal solution for all types of Windows applications. 

The library offers a wide range of features including event subscriptions, bindings, preventing input events, device information and a lot of other things. It also provides flexible custom interceptors, allowing users to define their own logic.

The main goal of this library is to provide maximum user-friendliness so that you don't have to write a lot of code. Therefore, it includes many convenient methods that facilitate an intuitive and efficient process of working with input events.

**You can read the full documentation [here](https://github.com/Empiree/DeftSharp.Windows.Input/blob/main/DOCUMENTATION.md)**

# Main Features

* Subscribe to global keyboard and mouse events
* Simulation of input from the code
* Prevent specific input events
* Change key bindings
* Custom interceptors
* Device information

# How to Install

The library is published as a [Nuget](https://www.nuget.org/packages/DeftSharp.Windows.Input)

`dotnet add package DeftSharp.Windows.Input`

# Examples

### Simple key subscription

You can subscribe to global keyboard events. Including their sequence and combination.

```c#

var keyboardListener = new KeyboardListener();

// Subscription for each click
keyboardListener.Subscribe(Key.Space, key => Trace.WriteLine($"The {key} was pressed"));

// One-time subscription
keyboardListener.SubscribeOnce(Key.Space, key => Trace.WriteLine($"The {key} was pressed"));

// Subscription to the combination
keyboardListener.SubscribeCombination([Key.LeftShift, Key.W], () =>
    Trace.WriteLine($"The Shift + W was pressed"));

```

### Input control from the code

You can simulate the operation of your keyboard and mouse by calling different input actions.

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

You can prevent input events by default or with some condition.

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
mouse.Prevent(PreventMouseEvent.Scroll);
```

### Subscription to mouse move and get current coordinates

You can track mouse coordinates in real-time.

```c#
var mouseListener = new MouseListener();

mouseListener.Subscribe(MouseEvent.Move, () =>
     Label.Text = $"X: {mouseListener.Position.X} Y: {mouseListener.Position.Y}");
```
![MouseListenerSample](https://github.com/Empiree/DeftSharp.Windows.Input/assets/60399216/9c9a04f6-cb39-491c-b8de-2cb6b435e112)

# Requirements

- .NET 7.0 for Windows
- Any UI framework (WPF, WinUI, MAUI, Avalonia)

# License

This project is licensed under the terms of the MIT license. See the [LICENSE](https://github.com/Empiree/DeftSharp.WPF.Keyboard/blob/main/LICENSE) file for details.

# Contributing

We welcome any [contributions](https://github.com/Empiree/DeftSharp.Windows.Input/blob/main/CONTRIBUTING.md) to the development of this project. Whether you want to report a bug, suggest a new feature, or contribute code improvements, your input is highly valued. Please feel free to submit issues or pull requests through GitHub. Let's make this library even better together!

You can also use this e-mail [deftsharp@gmail.com](mailto:deftsharp@gmail.com), if you have any ideas or suggestions.

**If you want to support this library, you can put a star on this repository!**

