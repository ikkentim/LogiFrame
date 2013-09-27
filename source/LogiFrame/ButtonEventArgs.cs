using System;

/// <summary>
/// Provides data for the LogiFrame.Frame.ButtonDown and LogiFrame.Frame.ButtonUp events.
/// </summary>
public class ButtonEventArgs : EventArgs
{
    /// <summary>
    /// Represents the 0-based number of the button being pressed.
    /// </summary>
    public int Button { get; set; }

    /// <summary>
    /// Initializes a new instance of the LogiFrame.ButtonEventArgs class.
    /// </summary>
    /// <param name="button">0-based number of the button being pressed.</param>
    public ButtonEventArgs(int button)
    {
        Button = button;
    }
}