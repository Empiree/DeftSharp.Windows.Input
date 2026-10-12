# DeftSharp.Windows.Input

A powerful .NET library designed to control and manage the keyboard and mouse in the Windows OS. It is intended for use in various UI frameworks such as WPF, WinUI, Avalonia, and MAUI, providing a universal solution for all types of Windows applications.

The library offers a wide range of features including event subscriptions, bindings, preventing input events, device information and a lot of other things. It also provides flexible custom interceptors, allowing users to define their own logic.

## Main Features

* Subscribe to global keyboard and mouse events
* Simulation of input from the code
* Prevent specific input events
* Change key bindings
* Custom interceptors
* Device information

## Examples

### Simple key subscription

You can subscribe to global keyboard events. Including their sequence and combination.

```c#
var keyboardListener = new KeyboardListener();

// Subscription for each click
keyboardListener.Subscribe(Key.Space, () => Trace.WriteLine($"The Space was pressed"));

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
mouse.Prevent(MousePreventOption.Scroll);
```

### Subscription to mouse move and get current coordinates

You can track mouse coordinates in real-time.

```c#
var mouseListener = new MouseListener();

mouseListener.Subscribe(MouseEvent.Move, () =>
     Label.Text = $"X: {mouseListener.Position.X} Y: {mouseListener.Position.Y}");
```

## Requirements

- .NET 8.0 for Windows
- Any UI framework (WPF, WinUI, MAUI, Avalonia)

## Links

- [Documentation](https://github.com/Empiree/DeftSharp.Windows.Input/blob/main/DOCUMENTATION.md)
- [GitHub repository](https://github.com/Empiree/DeftSharp.Windows.Input)
- [Release notes](https://github.com/Empiree/DeftSharp.Windows.Input/releases)
