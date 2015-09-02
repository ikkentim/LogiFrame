LogiFrame
=========
LogiFrame is a framework for efficiently creating apps for G13/G15/G510 devices. You can create controls and add them to the app, much like WinForms.

Example
-------

This example shows "Hello, World" in the top-left corner.

![](http://timpotze.nl/share/lcd/label.png)
``` c#
// Create a control.
var label = new LCDLabel
{
    Font = PixelFonts.Small, // The PixelFonts class contains various good fonts for LCD screens.
    Text = "Hello, World!",
    AutoSize = true,
};

// Create an app instance.
var app = new LCDApp("Sample App", false, false, false);

// Add the label control to the app.
app.Controls.Add(label);

// Make the app the foreground app on the LCD screen.
app.PushToForeground();

// A blocking call. Waits for the LCDApp instance to be disposed. (optional)
app.WaitForClose();
```

Controls
--------

The following controls are available in the library by default.

**LCDLabel** Draws text on the screen.

![](http://timpotze.nl/share/lcd/label.png)
``` c#
var label = new LCDLabel
{
    Font = PixelFonts.Small,
    Text = "Hello, World!",
    AutoSize = true,
};
```

**LCDLine** Draws a line on the screen.

![](http://timpotze.nl/share/lcd/line.png)
``` c#
var line = new LCDLine
{
    Start = new Point(0, 0),
    End = new Point(LCDApp.DefaultSize.Width-1, LCDApp.DefaultSize.Height-1)
};
```

**LCDRectangle** Draws a rectangle on the screen.

![](http://timpotze.nl/share/lcd/square.png)
``` c#
var rectangle = new LCDRectangle
{
    Location = new Point(0, 0),
    Size = new Size(40, 40),
    Style = RectangleStyle.Bordered
};
```

**LCDEllipse** Draws an ellipse on the screen.

![](http://timpotze.nl/share/lcd/ellipse.png)
``` c#
var ellipse = new LCDEllipse
{
    Location = new Point(40, 20),
    Size = new Size(50, 20)
};
```
**LCDProgressBar** Draws a progress bar on the screen.

![](http://timpotze.nl/share/lcd/progressBar.png)
``` c#
var progressBar = new LCDProgressBar
{
    Location = new Point(12, 14),
    Size = new Size(136, 6),
    Style = BorderStyle.Border,
    Direction = ProgressBarDirection.Right,
    Value = 50
};
```

**LCDPicture** Converts a picture to black-and-white and draws it on the screen.

![](http://timpotze.nl/share/lcd/picture_box.png)
``` c#
var picture = new LCDPicture
{
    Location = new Point(100, 10),
    Size = Resources.gtech.Size, // Resources.gtech is the image we want to draw on the screen.
    Image = Resources.gtech,
};
```

**LCDMarquee** A horizontally moving marquee, for displaying long text on the screen.

![](http://timpotze.nl/share/lcd/marquee.png)
``` c#
var marq = new LCDMarquee
{
    Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
    Size = new Size(LCDApp.DefaultSize.Width, 10),
    Location = new Point(0, 10),
};
```

**LCDSimpleGraph** Draws a simple graph on the screen.

![](http://timpotze.nl/share/lcd/simpleGraph.png)
``` c#
var graph = new LCDSimpleGraph
{
    Location = new Point(70, 30),
    Size = new Size(40, 10),
    Style = BorderStyle.Border
};

// We'll add a random value to the graph every half a second.
// 'Timer' is a class provided by LogiFrame which works much like WinForms' Timer.
var timer = new Timer {Interval = 500, Enabled = true};
timer.Tick += (sender, args) =>
{
    graph.PushValue(new Random().Next(0, 100));
};
```

**LCDTabControl/LCDTabPage** A tab control much like WinForms' `TabControl`.

![](http://timpotze.nl/share/lcd/tabControl.png)
``` c#
var tabPage = new LCDTabPage
{
    Icon = new LCDLabel
    {
        AutoSize = true,
        Text = "A",
        Font = PixelFonts.Title
    }
};
tabPage.Controls.Add(label);
tabPage.Controls.Add(line);
tabPage.Controls.Add(marq);

var tabPage2 = new LCDTabPage
{
    Icon = new LCDLabel
    {
        AutoSize = true,
        Text = "B",
        Font = PixelFonts.Title
    }
};
tabPage2.Controls.Add(rectangle);
tabPage2.Controls.Add(ellipse);
tabPage2.Controls.Add(progressBar);
tabPage2.Controls.Add(picture);
tabPage2.Controls.Add(graph);

var tabControl = new LCDTabControl();
tabControl.TabPages.Add(tabPage);
tabControl.TabPages.Add(tabPage2);

tabControl.SelectedTab = tabPage;
```

LCDControl
----------
All controls and `LCDApp` inherit from `LCDControl`. Below are the most important properties, methods and events inside this class.
- `public LCDControl Parent { get; private set; }` Gets the parent container of the control.
- `public IMergeMethod MergeMethod { get; set; }` Gets or sets the merge method used on the control when merging the rendered image in the parent container.
- `public Point Location { get; set; }` Gets or sets the location of the control.
- `public int Left { get; set; }` Gets or sets the distance, in pixels, between the left edge of the control and the left edge of its container.
- `public int Top { get; set; }` Gets or sets the distance, in pixels, between the top edge of the control and the top edge of its container.
- `public Size Size { get; set; }` Gets or sets the size of the control.
- `public int Width { get; set; }` Gets or sets the width of the control.
- `public int Height { get; set; }` Gets or sets the height of the control.
- `public bool Visible { get; set; }` Gets or sets a value indicating whether this LCDControl is visible.
- `public bool IsDisposed { get; private set; }` Gets a value indicating whether the control has been disposed of.
- `public event EventHandler VisibleChanged` Occurs when the Visible property value changes.
- `public event EventHandler<LCDPaintEventArgs> Paint` Occurs when the control is redrawn.
- `public event EventHandler<ButtonEventArgs> ButtonDown` Occurs when a button is pressed.
- `public event EventHandler<ButtonEventArgs> ButtonUp` Occurs when a button is released.
- `public event EventHandler<ButtonEventArgs> ButtonPress` Occurs while a button is pressed.
- `public event EventHandler Disposed` Occurs when the component is disposed by a call to the Dispose method.
- `public void Invalidate()` Invalidates the entire surface of the control and causes the control to be redrawn.
- `public bool IsButtonDown(int button)` Determines whether the specified button is pressed.
- `public void SuspendLayout()` Suspends the usual layout logic. While the layout logic is suspended, the control won't be rendered when a property has been changed.
- `public void ResumeLayout()` Resumes the usual layout logic.
- `public void Show()` Displays the control to the user.
- `public void Hide()` Hides the control from the user.
- `public void Dispose()` Releases all resources used by the LCDControl.







