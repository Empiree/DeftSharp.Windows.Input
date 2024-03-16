using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard;

namespace WPF.Playground
{
    /// <summary>
    /// You can use this project to test any functionality you want.
    /// Important! Please do not commit to this project
    /// </summary>
    public partial class MainWindow
    {
        private KeyboardListener _keyboardListener1;
        private KeyboardListener _keyboardListener2;
        private KeyboardListener _keyboardListener3;

        private KeyboardBinder _keyboardBinder1;
        private KeyboardBinder _keyboardBinder2;

        private KeyboardSequenceListener _sequenceListener1;
        private KeyboardSequenceListener _sequenceListener2;


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
            _sequenceListener1 = new KeyboardSequenceListener();
            _sequenceListener2 = new KeyboardSequenceListener();
            // KeyboardBinder = new KeyboardBinder();
            // _keyboardManipulator1 = new KeyboardManipulator();
            // _keyboardListener1 = new KeyboardListener();

            // _sequenceListener.Subscribe(new[] { Key.Q, Key.W, Key.E }, () =>
            // {
            //     PressedButtons.Text += $"QWE |";
            // });
            //
            // _sequenceListener.Subscribe(new[] { Key.Q, Key.W}, () =>
            // {
            //     PressedButtons.Text += $"QW |";
            // });
            //
            // _sequenceListener.Subscribe(new[] { Key.W, Key.E}, () =>
            // {
            //     PressedButtons.Text += $"WE |";
            // });

            // _keyboardManipulator1.Prevent(Key.Z);
            // _keyboardManipulator1.Prevent(Key.X);
            //
            // KeyboardBinder.Bind(Key.Q, Key.F);
            // KeyboardBinder.Bind(Key.W, Key.F);
            //
            // KeyboardBinder.Bind(Key.Z, Key.Q);
            // KeyboardBinder.Bind(Key.X, Key.W);


            _sequenceListener2.Subscribe(new[] { Key.R, Key.T }, () => { PressedButtons.Text += $"RT | "; });
            _sequenceListener1.Subscribe(new[] { Key.Q, Key.W }, () => { PressedButtons.Text += $"QW | "; });

            InitializeComponent();
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // var temp = new KeyboardListener();
            //
            // temp.Subscribe(Key.B, key =>
            // {
            //     PressedButtons.Text = key.ToString();
            // });
        }

        private void OnClosing(object? sender, CancelEventArgs e)
        {
            //  _keyboardListener.Unregister();
        }

        private void OnClickButton1(object sender, RoutedEventArgs e)
        {
            _sequenceListener1.UnsubscribeAll();
        }

        private void OnClickButton2(object sender, RoutedEventArgs e)
        {
            var sub = _sequenceListener2.Subscriptions.FirstOrDefault();
            if (sub is not null) 
                _sequenceListener2.Unsubscribe(sub.Id);
        }

        private void OnClickButton3(object sender, RoutedEventArgs e)
        {
            _sequenceListener2.Subscribe(new[] { Key.R, Key.T }, () => { PressedButtons.Text += $"RT | "; });
            _sequenceListener1.Subscribe(new[] { Key.Q, Key.W }, () => { PressedButtons.Text += $"QW | "; });
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