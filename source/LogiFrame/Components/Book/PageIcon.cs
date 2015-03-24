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
using System.Collections.Generic;

namespace LogiFrame.Components.Book
{
    /// <summary>
    ///     Represents a drawable icon for a <see cref="Page" />.
    /// </summary>
    public class PageIcon : Container
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PageIcon" /> class.
        /// </summary>
        public PageIcon()
        {
            base.Size = new Size(16, 16);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="PageIcon" /> class.
        /// </summary>
        /// <param name="components">The components.</param>
        public PageIcon(IEnumerable<Component> components) : this()
        {
            foreach (Component c in components)
                Components.Add(c);
        }

        /// <summary>
        ///     Gets or sets the <see cref="Location" /> this <see cref="Component" /> should be rendered at within the parent
        ///     <see cref="Container" />.
        /// </summary>
        /// <exception cref="System.ArgumentException">The Location of a LogiFrame.Components.Book.PageIcon cannot be changed.</exception>
        public override Location Location
        {
            get { return base.Location; }
            set
            {
                throw new ArgumentException("The Location of a LogiFrame.Components.Book.PageIcon cannot be changed.");
            }
        }

        /// <summary>
        ///     Gets or sets the <see cref="Size" /> of this <see cref="Component" />.
        /// </summary>
        /// <exception cref="System.ArgumentException">The Size of a LogiFrame.Components.Book.PageIcon cannot be changed.</exception>
        public override Size Size
        {
            get { return base.Size; }
            set { throw new ArgumentException("The Size of a LogiFrame.Components.Book.PageIcon cannot be changed."); }
        }
    }
}