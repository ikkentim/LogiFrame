LogiFrame
=========
LogiFrame is a framework for efficiently creating apps for G13/G15/G510 devices. You can create controls and add them to the app, much like WinForms.

Example
-------

This example shows "Hello, World" in the top-left corner.

![](http://timpotze.nl/share/lcd/helloworld.png)
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
``` c#
var label = new LCDLabel
{
    Font = PixelFonts.Small,
    Text = "Hello, World!",
    AutoSize = true,
};
```

**LCDLine** Draws a line on the screen.
``` c#
var line = new LCDLine
{
    Start = new Point(0, 0),
    End = new Point(LCDApp.DefaultSize.Width-1, LCDApp.DefaultSize.Height-1)
};
```

**LCDRectangle** Draws a rectangle on the screen.
``` c#
var rectangle = new LCDRectangle
{
    Location = new Point(0, 0),
    Size = new Size(40, 40),
    Style = RectangleStyle.Bordered
};
```

**LCDEllipse** Draws an ellipse on the screen.
``` c#
var ellipse = new LCDEllipse
{
    Location = new Point(40, 20),
    Size = new Size(50, 20)
};
```
**LCDProgressBar** Draws a progress bar on the screen.
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
``` c#
var picture = new LCDPicture
{
    Location = new Point(100, 10),
    Size = Resources.gtech.Size, // Resources.gtech is the image we want to draw on the screen.
    Image = Resources.gtech,
};
```

**LCDMarquee** A horizontally moving marquee, for displaying long text on the screen.
``` c#
var marq = new LCDMarquee
{
    Text = "Lorem",
    Size = new Size(LCDApp.DefaultSize.Width, 10),
    Location = new Point(0, 10),
};
```

**LCDSimpleGraph** Draws a simple graph on the screen.
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
