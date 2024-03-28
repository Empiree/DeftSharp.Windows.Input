using System.Windows;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard;
using DeftSharp.Windows.Input.Mouse;

namespace WPF.Playground
{
    /// <summary>
    /// You can use this project to test any functionality you want.
    /// </summary>
    public partial class MainWindow
    {
        private readonly Key[] _keys = { Key.Q, Key.W, Key.E };

        private readonly KeyboardListener _keyboardListener1 = new();
        private readonly KeyboardListener _keyboardListener2 = new();
        private readonly KeyboardListener _keyboardListener3 = new();

        private readonly KeyboardManipulator _keyboardManipulator1 = new();
        private readonly KeyboardManipulator _keyboardManipulator2 = new();

        private readonly KeyboardBinder _keyboardBinder1 = new();
        private readonly KeyboardBinder _keyboardBinder2 = new();

        private readonly MouseListener _mouseListener = new();

        private readonly MouseManipulator _mouseManipulator = new();

        private readonly KeyboardLogger _keyboardLogger = new();
        private readonly MouseLogger _mouseLogger = new();
        private readonly ScrollDisabler _scrollDisabler = new();

        public MainWindow() => InitializeComponent();


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void OnClickButton1(object sender, RoutedEventArgs e)
        {
        }

        private void OnClickButton2(object sender, RoutedEventArgs e)
        {
        }

        private void OnClickButton3(object sender, RoutedEventArgs e)
        {
        }

        private void OnClickButton4(object sender, RoutedEventArgs e)
        {
        }

        private void OnClickButton5(object sender, RoutedEventArgs e)
        {
        }
    }
}