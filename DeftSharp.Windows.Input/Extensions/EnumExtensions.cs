using System;
using System.Collections.Generic;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.Extensions;

internal static class EnumExtensions
{
    public static IEnumerable<MouseInputEvent> ToMouseEvents(this PreventMouseOption preventEvent)
    {
        var preventEvents = new List<MouseInputEvent>();

        switch (preventEvent)
        {
            case PreventMouseOption.Move:
                preventEvents.Add(MouseInputEvent.Move);
                break;
            case PreventMouseOption.LeftButton:
                preventEvents.Add(MouseInputEvent.LeftButtonDown);
                preventEvents.Add(MouseInputEvent.LeftButtonUp);
                break;
            case PreventMouseOption.RightButton:
                preventEvents.Add(MouseInputEvent.RightButtonDown);
                preventEvents.Add(MouseInputEvent.RightButtonUp);
                break;
            case PreventMouseOption.MiddleButton:
                preventEvents.Add(MouseInputEvent.MiddleButtonDown);
                preventEvents.Add(MouseInputEvent.MiddleButtonUp);
                break;
            case PreventMouseOption.Scroll:
                preventEvents.Add(MouseInputEvent.Scroll);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(preventEvent), preventEvent,
                    $"Invalid value for argument {nameof(preventEvent)}: {preventEvent}");
        }

        return preventEvents;
    }

    public static IEnumerable<MouseEvent> ToMouseEvents(this MouseInputEvent inputEvent)
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