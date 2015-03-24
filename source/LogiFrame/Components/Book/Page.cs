// LogiFrame
// Copyright 2015 Tim Potze
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

namespace LogiFrame.Components.Book
{
    /// <summary>
    ///     Represents a page which can be used with a <see cref="Book" />.
    /// </summary>
    public abstract class Page : Container
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Page" /> class.
        /// </summary>
        protected Page()
        {
            base.Size = new Size((int) LgLcd.LgLcdBitmapWidth, (int) LgLcd.LgLcdBitmapHeight);
        }

        /// <summary>
        ///     Gets or sets the <see cref="Location" /> this <see cref="Component" /> should be rendered at within the parent
        ///     <see cref="Container" />.
        /// </summary>
        /// <exception cref="System.ArgumentException">The Location of a LogiFrame.Components.Book.Page cannot be changed.</exception>
        public override Location Location
        {
            get { return base.Location; }
            set { throw new ArgumentException("The Location of a LogiFrame.Components.Book.Page cannot be changed."); }
        }

        /// <summary>
        ///     Gets or sets the <see cref="Size" /> of this <see cref="Component" />.
        /// </summary>
        /// <exception cref="System.ArgumentException">The Size of a LogiFrame.Components.Book.Page cannot be changed.</exception>
        public override Size Size
        {
            get { return base.Size; }
            set { throw new ArgumentException("The Size of a LogiFrame.Components.Book.Page cannot be changed."); }
        }

        /// <summary>
        ///     Gets the icon.
        /// </summary>
        public abstract PageIcon Icon { get; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance is browsable.
        /// </summary>
        public virtual bool IsBrowsable
        {
            get { return true; }
        }

        /// <summary>
        ///     Occurs when a button has been pressed.
        /// </summary>
        public event EventHandler<ButtonEventArgs> ButtonPressed;

        /// <summary>
        ///     Occurs when a button has been released.
        /// </summary>
        public event EventHandler<ButtonEventArgs> ButtonReleased;

        /// <summary>
        ///     Occurs when shown.
        /// </summary>
        public event EventHandler Show;

        /// <summary>
        ///     Occurs when hidden.
        /// </summary>
        public event EventHandler Hide;

        /// <summary>
        ///     Raises the <see cref="ButtonPressed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ButtonEventArgs" /> instance containing the event data.</param>
        public virtual void OnButtonPressed(ButtonEventArgs e)
        {
            if (ButtonPressed != null)
                ButtonPressed(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="ButtonReleased" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ButtonEventArgs" /> instance containing the event data.</param>
        public virtual void OnButtonReleased(ButtonEventArgs e)
        {
            if (ButtonReleased != null)
                ButtonReleased(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="Show" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        public virtual void OnShow(EventArgs e)
        {
            if (Show != null)
                Show(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="Hide" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        public virtual void OnHide(EventArgs e)
        {
            if (Hide != null)
                Hide(this, e);
        }
    }
}