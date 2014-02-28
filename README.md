LogiFrame
=========
LogiFrame is a library to easily make applications for Logitech LCD displays(Mono displays only!)
The following code already show a line of text on the display:

	var frame = new Frame("Test app");
	frame.Components.Add(new Label
	{
		AutoSize=true,
		Text="Hello World!"
	});
	frame.WaitForClose();
	
Supported devices:

	-Logitech G510 keyboard
	-Logitech G13 keyboard
	-Logitech G15 v1 keyboard
	-Logitech G15 v2 keyboard

During developement the Library has only been tested with a G15 v2 keyboard, 
but should also work with the other mentioned devices.
	
Source code contains:

	-the LogiFrame Library
	-a Test application
	-an application for GTA: San Andreas (INCOMPLETE!)
	-an application for Spotify
	-an application for Call of Duty 4: Modern Warfare Server Information
	-an application with the Bitonic Bitcoin exchange rate

License
=======
LogiFrame rendering library.
Copyright (C) 2014 Tim Potze

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>. 
