using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using DeftSharp.Windows.Input.Extensions;
using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Keyboard;
using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Mouse.Interceptors;

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

       private ScrollDisable sd = new ScrollDisable();
       private MouseLog mml = new MouseLog();
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void OnClickButton1(object sender, RoutedEventArgs e)
        {

            sd.Hook();
            mml.Hook();
        }

        private void OnClickButton2(object sender, RoutedEventArgs e)
        {
            sd.Unhook();
            mml.Unhook();
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

    public class ScrollDisable : MouseInterceptor
    {
        protected override bool IsInputAllowed(MouseInputArgs args)
        {
            if(args.Event is MouseInputEvent.Scroll)
            {
                return false;
            }

            return true;
        }
    }

    public class MouseLog : MouseInterceptor
    {
        protected override bool IsInputAllowed(MouseInputArgs args)
        {
            return true;
        }

        protected override void OnInputSuccess(MouseInputArgs args)
        {
            if (args.Event is MouseInputEvent.Move)
                return;

            Console.WriteLine(args.Event);

        }

        protected override void OnInputFailure(MouseInputArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
        {
            Console.WriteLine($"{args.Event} failed at {failedInterceptors.ToNames}");
        }


    }
}