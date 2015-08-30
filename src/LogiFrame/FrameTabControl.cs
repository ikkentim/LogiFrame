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
using System.Drawing;
using System.Linq;

namespace LogiFrame
{
    public class FrameTabControl : FrameControl
    {
        private FrameTabMenuControl _menu;
        private FrameTabPage _selectedTab;

        public FrameTabControl()
        {
            TabPages.ItemRemoved += TabPages_ItemRemoved;

            Menu = new FrameTabMenuControl(this);
            Size = Frame.DefaultSize;
        }

        public FrameTabPageCollection TabPages { get; } = new FrameTabPageCollection();

        public FrameTabMenuControl Menu
        {
            get { return _menu; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _menu = value;
                value.AssignParent(this);
            }
        }

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

        public FrameTabPage SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if (value != null && !TabPages.Contains(value))
                    throw new ArgumentException("value not a member of TabPages", nameof(value));

                SuspendLayout();
                _selectedTab?.AssignParent(null);
                value?.AssignParent(this);
                _selectedTab = value;
                OnSelectedTabChanged();
                Invalidate();
                ResumeLayout();
            }
        }

        public event EventHandler SelectedTabChanged;

        public void ShowMenu()
        {
            Menu.Visible = true;
        }

        public void HideMenu()
        {
            Menu.Visible = false;
        }

        private void TabPages_ItemRemoved(object sender, ValueEventArgs<FrameTabPage> e)
        {
            if (e.Value == SelectedTab)
                SelectedTab = null;
        }

        protected virtual void OnSelectedTabChanged()
        {
            SelectedTabChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Overrides of FrameControl

        protected override void OnPaint(FramePaintEventArgs e)
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

        protected override void OnButtonDown(ButtonEventArgs e)
        {
            if (Menu.Visible && (e.PreventPropagation = Menu.HandleButtonDown(e.Button)))
                return;

            if (TabPages.Any(tab => (tab.HandleButtonDown(e.Button))))
            {
                e.PreventPropagation = true;
                return;
            }
            base.OnButtonDown(e);
        }

        protected override void OnButtonUp(ButtonEventArgs e)
        {
            if (Menu.Visible && (e.PreventPropagation = Menu.HandleButtonUp(e.Button)))
                return;

            if (TabPages.Any(tab => (tab.HandleButtonUp(e.Button))))
            {
                e.PreventPropagation = true;
                return;
            }
            base.OnButtonUp(e);
        }

        #endregion
    }
}