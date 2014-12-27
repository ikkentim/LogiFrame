LogiFrame
=========
LogiFrame is a library to easily make applications for Logitech LCD displays(Mono displays only!)
The following code already show a line of text on the display:
```C#
var frame = new Frame("Test app");
frame.Components.Add(new Label
{
	AutoSize=true,
	Text="Hello World!"
});
frame.WaitForClose();
```
The library contains a couple of pre-defined components, including: Animation(GIF image), Circle, Container, Diagram, Label, Line, Marquee, Picture, ProgressBar, RotatedContainer(Rotator), ScrollBar, Square and a Timer.

Supported devices:

- Logitech G510 keyboard
- Logitech G13 keyboard
- Logitech G15 v1 keyboard
- Logitech G15 v2 keyboard

During developement the Library has only been tested with a G15 v2 keyboard, 
but should also work with the other mentioned devices.
