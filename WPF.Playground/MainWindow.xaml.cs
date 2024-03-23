using System.Diagnostics;
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
        private KeyboardListener _keyboardListener1;
        private KeyboardListener _keyboardListener2;
        private KeyboardListener _keyboardListener3;

        private KeyboardManipulator _keyboardManipulator1;

        private KeyboardBinder _keyboardBinder1;
        private KeyboardBinder _keyboardBinder2;

        private MouseListener _mouseListener;

        private MouseManipulator _mouseManipulator;

        // Custom

        private KeyboardLogger _keyboardLogger;
        private MouseLogger _mouseLogger;

        private ScrollDisabler _scrollDisabler;

        public void TestBinder()
        {
            _keyboardBinder1 = new KeyboardBinder();
            _keyboardBinder2 = new KeyboardBinder();


            _keyboardBinder1.Bind(Key.Q, Key.W);
            _keyboardBinder2.Bind(Key.W, Key.Q);

            _keyboardListener1 = new KeyboardListener();

            _keyboardListener1.Subscribe(Key.Q, key => PressedButtons.Text += key.ToString());

            _keyboardListener1.Subscribe(Key.W, key => PressedButtons.Text += key.ToString());

            _keyboardListener1.Subscribe(Key.E, key => { PressedButtons.Text += key.ToString(); });

            _keyboardListener1.Subscribe(Key.C, key => { PressedButtons.Text += key.ToString(); });

            _keyboardListener1.Subscribe(Key.V, key => { PressedButtons.Text += key.ToString(); });
        }

        public void TestListener()
        {
            _keyboardListener1 = new KeyboardListener();
            _keyboardListener2 = new KeyboardListener();
            _keyboardListener3 = new KeyboardListener();

            _keyboardListener1.Subscribe(Key.Q, key => PressedButtons.Text += key.ToString());
            _keyboardListener2.Subscribe(Key.W, key => PressedButtons.Text += key.ToString());
            _keyboardListener3.Subscribe(Key.E, key => PressedButtons.Text += key.ToString());
        }

        public MainWindow()
        {
            _keyboardManipulator1 = new KeyboardManipulator();
            _mouseManipulator = new MouseManipulator();

            _keyboardListener1 = new KeyboardListener();
            _keyboardListener2 = new KeyboardListener();
            _keyboardListener3 = new KeyboardListener();
            _mouseListener = new MouseListener();
            _scrollDisabler = new ScrollDisabler();
            _keyboardLogger = new KeyboardLogger();

            _keyboardBinder1 = new KeyboardBinder();
            _keyboardBinder2 = new KeyboardBinder();

            _mouseLogger = new MouseLogger();

            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _mouseLogger.Hook();
            _scrollDisabler.Hook();

            _keyboardManipulator1.Prevent(Key.C);

            _keyboardListener1.Subscribe(Key.A, key =>
            {
                Trace.WriteLine(key.ToString());
            });

            _mouseListener.Subscribe(MouseEvent.Scroll, () => { Trace.WriteLine(_keyboardListener1.IsWinPressed); });
        }

        private void OnClickButton1(object sender, RoutedEventArgs e)
        {
            _mouseLogger.Unhook();
            _scrollDisabler.Unhook();
            _keyboardLogger.Unhook();
            _keyboardLogger.Unhook();
            _mouseManipulator.ReleaseAll();
            _mouseListener.UnsubscribeAll();
        }

        private void OnClickButton2(object sender, RoutedEventArgs e) { }

        private void OnClickButton3(object sender, RoutedEventArgs e) { }

        private void OnClickButton4(object sender, RoutedEventArgs e) { }

        private void OnClickButton5(object sender, RoutedEventArgs e) { }
    }
}