# DeftSharp.Windows.Input

A lightweight library designed to handle and manage keyboard and mouse button events in Windows UI applications (WPF, MAUI, Avalonia). Using P/Invoke methods, this library provides an easy-to-use interface for event handling.

# How to Install

The library is published as a [Nuget](https://www.nuget.org/packages/DeftSharp.Windows.Input)

`dotnet add package DeftSharp.Windows.Input`

# Features

* Subscription to key presses, combinations and sequences on the keyboard
* Subscription to mouse events and obtaining information on its coordinates
* Prohibit pressing any button
* Pressing buttons from code
* Changing buttons binding
* Various useful classes such as NumpadListener


# Examples

## KeyboardListener

### Key subscription

```c#

var keyboardListener = new KeyboardListener();

keyboardListener.Subscribe(Key.A, key =>
{
    // This code will be triggered with each press of the 'A' button
});
```

### One-time key subscription

```c#
keyboardListener.SubscribeOnce(Key.A, key =>
{
    // This code will be triggered only once, after the first press of the 'A' button
});

```

### Sequence subscription

```c#
Key[] sequence = { Key.A, Key.B };
            
keyboardListener.SubscribeSequence(sequence, () =>
{
    // This code will trigger after successive presses of 'A B' buttons
});
```

### Combination subscription

```c#
Key[] combination = { Key.Ctrl, Key.C };
            
keyboardListener.SubscribeCombination(combination, () =>
{
    // This code will be triggered by pressing the 'Ctrl+C' button combination
});
```

### Subscription with interval and event type

```c#
keyboardListener.Subscribe(Key.A, key =>
{
    // Your code is here
},
TimeSpan.FromSeconds(5), // Interval of callback triggering
KeyboardEvent.KeyUp); // Subscribe to KeyUp event
```
> [!NOTE]
> Each object of the KeyboardListener class stores its own subscriptions. Keep this in mind when you use the `UnsubscribeAll()` method.

## KeyboardManipulator

### Press button

```c#
var keyboardManipulator = new KeyboardManipulator();

keyboardManipulator.Press(Key.A); // The 'A' button will be pressed
```

### Prohibit button pressing

```c#
keyboardManipulator.Prevent(Key.A); // Each press of this button will be ignored
```

## KeyboardBinder

### Change the button bind

```c#
var keyboardBinder = new KeyboardBinder();
            
keyboardBinder.Bind(Key.Q, Key.W); 

// Now any time the 'Q' button is triggered, it will behave like the 'W' button
```

> [!NOTE]
> Prevented and bounded buttons are shared among all class objects. You don't have to worry that an object in this class has locked a particular button and you no longer have access to that object.

## MouseListener

### Subscribe to mouse move event and get current coordinates

```c#
var mouseListener = new MouseListener();

mouseListener.Subscribe(MouseEvent.Move, () =>
{
  Coordinates coordinates = mouseListener.GetPosition();
  Console.WriteLine($"Current mouse position: X: {coordinates.X} Y: {coordinates.Y}");
});
```

## MouseManipulator

### Set mouse position

```c#
var mouseManipulator = new MouseManipulator();
            
mouseManipulator.SetPosition(x:100,y:100);
```

### Click the right mouse button on the specified coordinates

```c#
mouseManipulator.Click(x:100, y:100, MouseButton.Right);
```

### Prohibit mouse button pressing

```c#
mouseManipulator.Prevent(PreventMouseOption.LeftButton);
```
> [!NOTE]
> Be careful when using this method. You may completely block the operation of your mouse.


## Requirements

- .NET 7.0 for Windows

## License

This project is licensed under the terms of the MIT license. See the [LICENSE](https://github.com/Empiree/DeftSharp.WPF.Keyboard/blob/main/LICENSE) file for details.

## Contributing

We welcome any [contributions](https://github.com/Empiree/DeftSharp.Windows.Input/blob/main/CONTRIBUTING.md) to the development of this project. Whether you want to report a bug, suggest a new feature, or contribute code improvements, your input is highly valued. Please feel free to submit issues or pull requests through GitHub. Let's make this library even better together!

## Feedback

If you have any ideas or suggestions. You can use this e-mail [deftsharp@gmail.com](mailto:deftsharp@gmail.com) for feedback.
