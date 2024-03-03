# DeftSharp.WPF.Keyboard


A lightweight library designed for obtaining information about pressed keys in WPF applications on the Windows platform. Leveraging P/Invoke methods, this library provides an easy-to-use interface for keyboard event handling.

## How to use

```c#

static int Main(string[] args)
{
    var keyboardListener = new KeyboardListener();

    keyboardListener.SubscribeOnce(Key.A, key =>
    {
        // This code will trigger after the first presses the 'A' button.
    });
}

```

You can easily handle the press of any button. Additionally, you can configure them to suit your needs:

```c#
 Key[] keys = { Key.W, Key.A, Key.S, Key.D };
            
 keyboardListener.Subscribe(keys, key =>
 {
    // WASD clicks
 }, TimeSpan.FromSeconds(1)); // Interval of click
```
Furthermore, you can take advantage of specialized classes for different usage scenarios. For example, for the NumPad:

```c#
var numpadListener = new NumpadListener(keyboardListener);
            
// 0-9 numpad buttons
numpadListener.Subscribe(number =>
{
    // Your code here
});
```

## Requirements

- .NET 7.0 for Windows
- C# 11 language version

## Coming Soon to NuGet

We're currently in the process of preparing the library for publication on NuGet. Stay tuned for updates!

## License

This project is licensed under the terms of the MIT license. See the [LICENSE](https://github.com/Empiree/DeftSharp.WPF.Keyboard/blob/main/LICENSE) file for details.

## Contributing

We welcome any contributions to the development of this project. Whether you want to report a bug, suggest a new feature, or contribute code improvements, your input is highly valued. Please feel free to submit issues or pull requests through GitHub. Let's make this library even better together!
