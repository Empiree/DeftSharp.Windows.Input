using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard;
using DeftSharp.Windows.Input.Mouse;
using MouseButton = DeftSharp.Windows.Input.Mouse.MouseButton;

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

        private KeyboardBinder _keyboardBinder1;
        private KeyboardBinder _keyboardBinder2;

        private MouseListener _mouseListener;
        private MouseManipulator _mouseManipulator;

        private KeyboardManipulator _keyboardManipulator;


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
            _keyboardManipulator = new KeyboardManipulator();
            _mouseManipulator = new MouseManipulator();

            _keyboardListener1 = new KeyboardListener();
            _keyboardListener2 = new KeyboardListener();
            _mouseListener = new MouseListener();

            InitializeComponent();
        }


        private int counter = 0;
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //Key[] keys = new[] { Key.LeftCtrl, Key.V };
          //  Key[] keys = new[] { Key.LWin, Key.Tab };
            // _mouseListener.Subscribe(MouseEvent.RightButtonDown,Key
            //     () => {  });|| InputKeyExtended InputKeyExtendedll
            
            // Key[] keys = new[] { Key.LeftShift, Key.H };
            //
            // _mouseListener.Subscribe(MouseEvent.Scroll, mouseEvent =>
            // {
            //     // counter++;           
            //     // PressedButtons.Text = counter.ToString();
            //     _keyboardManipulator.PressCombination(keys);
            // });

            // _keyboardListener1.SubscribeAll(key =>
            // {
            //     PressedButtons.Text = key.ToString();
            // });

            _mouseListener.Subscribe(MouseEvent.Scroll, () =>
            {
                Key[] pasteCombination = { Key.LeftCtrl, Key.LeftAlt, Key.Delete };
            
                _keyboardManipulator.PressCombination(pasteCombination);
            });

            // var keyboardManipulator = new KeyboardManipulator();
            //
            // Key[] paste = { Key.LWin, Key.V };
            //
            // keyboardManipulator.PressCombination(paste);
            

            _keyboardListener1.Subscribe(Key.LWin, key =>
            {
                PressedButtons.Text += "d";
            });
            
            _keyboardListener1.Subscribe(Key.LWin, key =>
            {
                PressedButtons.Text += "u ";
            }, keyboardEvent: KeyboardEvent.KeyUp);
        }

        private void OnClosing(object? sender, CancelEventArgs e)
        {
            //  _keyboardListener.Unregister();
        }

        private void OnClickButton1(object sender, RoutedEventArgs e)
        {
            _keyboardListener1.UnsubscribeAll();
            _keyboardListener2.UnsubscribeAll();
            //  _sequenceListener1.UnsubscribeAll();
        }

        private void OnClickButton2(object sender, RoutedEventArgs e)
        {
        }

        private void OnClickButton3(object sender, RoutedEventArgs e)
        {
            _keyboardListener2.SubscribeCombination(new[] { Key.R, Key.T }, () => { PressedButtons.Text += $"RT | "; });
            _keyboardListener1.SubscribeCombination(new[] { Key.Q, Key.W }, () => { PressedButtons.Text += $"QW | "; });
        }

        private void OnClickButton4(object sender, RoutedEventArgs e)
        {
            _keyboardListener2.UnsubscribeAll();
        }

        private void OnClickButton5(object sender, RoutedEventArgs e)
        {
            _keyboardListener3.UnsubscribeAll();
        }
    }
}