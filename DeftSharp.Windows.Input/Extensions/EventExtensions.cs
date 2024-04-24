using System;
using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.Keyboard;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.Extensions;

internal static class EventExtensions
{
    internal static IEnumerable<MouseInputEvent> ToMouseEvents(this PreventMouseOption preventOption)
    {
        var events = new List<MouseInputEvent>();

        switch (preventOption)
        {
            case PreventMouseOption.Move:
                events.Add(MouseInputEvent.Move);
                break;
            case PreventMouseOption.LeftButton:
                events.Add(MouseInputEvent.LeftButtonDown);
                events.Add(MouseInputEvent.LeftButtonUp);
                break;
            case PreventMouseOption.RightButton:
                events.Add(MouseInputEvent.RightButtonDown);
                events.Add(MouseInputEvent.RightButtonUp);
                break;
            case PreventMouseOption.MiddleButton:
                events.Add(MouseInputEvent.MiddleButtonDown);
                events.Add(MouseInputEvent.MiddleButtonUp);
                break;
            case PreventMouseOption.Scroll:
                events.Add(MouseInputEvent.Scroll);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(preventOption), preventOption,
                    $"Invalid value for argument {nameof(preventOption)}: {preventOption}");
        }

        return events;
    }

    internal static IEnumerable<KeyboardEvent> ToKeyboardEvents(this KeyboardInputEvent inputEvent)
    {
        var events = new List<KeyboardEvent>();

        switch (inputEvent)
        {
            case KeyboardInputEvent.KeyDown:
                events.Add(KeyboardEvent.KeyDown);
                break;
            case KeyboardInputEvent.KeyUp:
                events.Add(KeyboardEvent.KeyUp);
                break;
            default:
                return Enumerable.Empty<KeyboardEvent>();
        }
        
        events.Add(KeyboardEvent.All);
        return events;
    }

    internal static IEnumerable<MouseEvent> ToMouseEvents(this MouseInputEvent inputEvent)
    {
        var events = new List<MouseEvent>();

        switch (inputEvent)
        {
            case MouseInputEvent.Move:
                events.Add(MouseEvent.Move);
                break;
            case MouseInputEvent.LeftButtonDown:
                events.Add(MouseEvent.LeftButtonDown);
                events.Add(MouseEvent.ButtonDown);
                break;
            case MouseInputEvent.LeftButtonUp:
                events.Add(MouseEvent.LeftButtonUp);
                events.Add(MouseEvent.ButtonUp);
                break;
            case MouseInputEvent.RightButtonDown:
                events.Add(MouseEvent.RightButtonDown);
                events.Add(MouseEvent.ButtonDown);
                break;
            case MouseInputEvent.RightButtonUp:
                events.Add(MouseEvent.RightButtonUp);
                events.Add(MouseEvent.ButtonUp);
                break;
            case MouseInputEvent.MiddleButtonDown:
                events.Add(MouseEvent.MiddleButtonDown);
                events.Add(MouseEvent.ButtonDown);
                break;
            case MouseInputEvent.MiddleButtonUp:
                events.Add(MouseEvent.MiddleButtonUp);
                events.Add(MouseEvent.ButtonUp);
                break;
            case MouseInputEvent.Scroll:
                events.Add(MouseEvent.Scroll);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(inputEvent), inputEvent,
                    $"Invalid value for argument {nameof(inputEvent)}: {inputEvent}");
        }

        return events;
    }
}