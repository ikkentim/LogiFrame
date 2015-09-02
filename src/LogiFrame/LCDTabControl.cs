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
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace LogiFrame
{
    /// <summary>
    ///     Represents a tab control.
    /// </summary>
    public class LCDTabControl : LCDControl
    {
        private LCDTabMenuControl _menu;
        private LCDTabPage _selectedTab;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LCDTabControl" /> class.
        /// </summary>
        public LCDTabControl()
        {
            TabPages.ItemRemoved += TabPages_ItemRemoved;

            Menu = new LCDTabMenuControl(this);
            Size = LCDApp.DefaultSize;
        }

        /// <summary>
        ///     Gets the collection of tab pages represented by the tab control.
        /// </summary>
        public LCDTabPageCollection TabPages { get; } = new LCDTabPageCollection();

        /// <summary>
        ///     Gets or sets the menu.
        /// </summary>
        public LCDTabMenuControl Menu
        {
            get { return _menu; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _menu = value;
                value.AssignParent(this);
            }
        }

        /// <summary>
        ///     Gets or sets the index of the selected tab.
        /// </summary>
        public int SelectedIndex
        {
            get { return TabPages.IndexOf(SelectedTab); }
            set
            {
                if (value < -1 || value >= TabPages.Count)
                    throw new ArgumentOutOfRangeException(nameof(value));

                SelectedTab = value == -1 ? null : TabPages[value];
            }
        }

        /// <summary>
        ///     Gets or sets the selected tab.
        /// </summary>
        public LCDTabPage SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (value != null && !TabPages.Contains(value))
                    throw new ArgumentException("value not a member of TabPages", nameof(value));

                SuspendLayout();
                if (_selectedTab != null)
                {
                    _selectedTab.AssignParent(null);
                    _selectedTab.Visible = false;
                }
                _selectedTab = value;
                if (value != null)
                {
                    value.AssignParent(this);
                    value.Visible = true;
                }
                OnSelectedTabChanged();
                Invalidate();
                ResumeLayout();
            }
        }

        /// <summary>
        ///     Occurs when the <see cref="P:System.Windows.Forms.TabControl.SelectedTab" /> property has changed.
        /// </summary>
        public event EventHandler SelectedTabChanged;

        /// <summary>
        ///     Shows the menu.
        /// </summary>
        public void ShowMenu()
        {
            ThrowIfDisposed();

            Menu.Visible = true;
        }

        /// <summary>
        ///     Hides the menu.
        /// </summary>
        public void HideMenu()
        {
            ThrowIfDisposed();

            Menu.Visible = false;
        }

        private void TabPages_ItemRemoved(object sender, LCDTabPageEventArgs e)
        {
            if (e.TabPage == SelectedTab)
                SelectedTab = null;
        }

        /// <summary>
        ///     Raises the <see cref="E:SelectedTab" /> event.
        /// </summary>
        protected virtual void OnSelectedTabChanged()
        {
            SelectedTabChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Overrides of LCDControl

        /// <summary>
        ///     Raises the <see cref="E:Paint" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LogiFrame.LCDPaintEventArgs" /> instance containing the event data.</param>
        protected override void OnPaint(LCDPaintEventArgs e)
        {
            SelectedTab?.PerformLayout();
            e.Bitmap.MergeOverride(SelectedTab?.Bitmap, new Point(0, 0));

            if (Menu.Visible)
            {
                Menu.PerformLayout();
                e.Bitmap.MergeOverride(Menu.Bitmap, new Point(0, Height - Menu.Height));
            }
            base.OnPaint(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:ButtonDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LogiFrame.ButtonEventArgs" /> instance containing the event data.</param>
        protected override void OnButtonDown(ButtonEventArgs e)
        {
            if (e.PreventPropagation)
                return;

            if (Menu.Visible &&  Menu.HandleButtonDown(e.Button))
            {
                e.PreventPropagation = true;
                return;
            }

            if (!Menu.Visible && SelectedTab.HandleButtonDown(e.Button))
            {
                e.PreventPropagation = true;
                return;
            }
            base.OnButtonDown(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:ButtonUp" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LogiFrame.ButtonEventArgs" /> instance containing the event data.</param>
        protected override void OnButtonUp(ButtonEventArgs e)
        {
            if (e.PreventPropagation)
                return;

            if (Menu.Visible && Menu.HandleButtonUp(e.Button))
            {
                e.PreventPropagation = true;
                return;
            }

            if (!Menu.Visible && SelectedTab.HandleButtonUp(e.Button))
            {
                e.PreventPropagation = true;
                return;
            }
            base.OnButtonUp(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:ButtonPress" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LogiFrame.ButtonEventArgs" /> instance containing the event data.</param>
        protected override void OnButtonPress(ButtonEventArgs e)
        {
            if (e.PreventPropagation)
                return;

            if (Menu.Visible && Menu.HandleButtonPress(e.Button))
            {
                e.PreventPropagation = true;
                return;
            }

            if (!Menu.Visible && SelectedTab.HandleButtonPress(e.Button))
            {
                e.PreventPropagation = true;
                return;
            }
            base.OnButtonPress(e);
        }

        #endregion
    }
}