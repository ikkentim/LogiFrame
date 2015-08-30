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
using LogiFrame.Drawing;

namespace LogiFrame
{
    public class FrameTabMenuControl : FrameControl
    {
        private const int Margin = 2;
        private readonly ContainerFrameControl _container = new ContainerFrameControl();
        private readonly FrameLine _line;

        public FrameTabMenuControl(FrameTabControl tabControl)
        {
            if (tabControl == null) throw new ArgumentNullException(nameof(tabControl));

            TabControl = tabControl;
            tabControl.SelectedTabChanged += TabControl_SelectedTabChanged;
            tabControl.TabPages.ItemAdded += TabPages_ItemAdded;
            tabControl.TabPages.ItemRemoved += TabPages_ItemRemoved;

            Visible = false;

            _line = new FrameLine
            {
                Start = new Point(0, 0),
                End = new Point(Width - 1, 0)
            };

            _container.Size = base.Size = new Size(Frame.DefaultSize.Width, 10);
            _container.AssignParent(this);
        }

        public FrameTabControl TabControl { get; }
        public int ButtonLeft { get; set; } = 0;
        public int ButtonRight { get; set; } = 1;
        public int ButtonClose { get; set; } = 2;

        public FrameTabMenuButtonTask Button0Task { get; set; } = FrameTabMenuButtonTask.Previous;
        public FrameTabMenuButtonTask Button1Task { get; set; } = FrameTabMenuButtonTask.Next;
        public FrameTabMenuButtonTask Button2Task { get; set; } = FrameTabMenuButtonTask.Close;
        public FrameTabMenuButtonTask Button3Task { get; set; } = FrameTabMenuButtonTask.None;
        public void SelectPrevious()
        {
            var index = TabControl.SelectedIndex;
            switch (TabControl.TabPages.Count)
            {
                case 0:
                    index = -1;
                    break;
                case 1:
                    index = 0;
                    break;
                default:
                    if (index == -1)
                        index = 0;
                    else
                        index = index == 0 ? TabControl.TabPages.Count - 1 : index - 1;
                    break;
            }

            TabControl.SelectedIndex = index;
        }

        public void SelectNext()
        {
            var index = TabControl.SelectedIndex;
            switch (TabControl.TabPages.Count)
            {
                case 0:
                    index = -1;
                    break;
                case 1:
                    index = 0;
                    break;
                default:
                    if (index == -1 || index == TabControl.TabPages.Count - 1)
                        index = 0;
                    else
                        index++;
                    break;
            }

            TabControl.SelectedIndex = index;
        }

        private void TabControl_SelectedTabChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                SuspendLayout();
                ResetControls();
                PlaceControls();
                Invalidate();
                ResumeLayout();
            }
        }

        private void TabPages_ItemRemoved(object sender, ValueEventArgs<FrameTabPage> e)
        {
            if (Visible)
                Invalidate();
        }

        private void TabPages_ItemAdded(object sender, ValueEventArgs<FrameTabPage> e)
        {
            if (Visible)
                Invalidate();
        }

        #region Overrides of FrameControl

        public override Size Size
        {
            get { return base.Size; }
            set
            {
                _container.Size = value;
                base.Size = value;
            }
        }

        private void PlaceControls()
        {
            SuspendLayout();

            // Recalculate size based on line, margins and largest icon.
            var iconWidth = TabControl.TabPages.Max(t => t.Icon?.Width ?? 0);
            var iconHeight = TabControl.TabPages.Max(t => t.Icon?.Height ?? 0);
            var iconCount = TabControl.TabPages.Count;
            var marginsBetweenIcons = Math.Max(iconCount - 1, 0);
            var iconBarWidthSum = iconCount*iconWidth + marginsBetweenIcons*Margin;

            Size = new Size(Frame.DefaultSize.Width, 1 + Margin*2 + iconHeight);

            _line.Start = new Point(0, 0);
            _line.End = new Point(Width - 1, 0);

            // Re-add all controls.
            _container.Controls.Clear();
            _container.Controls.Add(_line);

            var x = Width/2 - iconBarWidthSum/2;
            foreach (var tab in TabControl.TabPages)
            {
                if (tab == TabControl.SelectedTab)
                {
                    var selectionBox = new FrameRectangle
                    {
                        Location = new Point(x - 1, (Height - iconHeight - 1)/2),
                        Size = new Size(2 + iconWidth, 2 + iconHeight),
                        Style = RectangleStyle.Filled
                    };
                    _container.Controls.Add(selectionBox);
                }

                var icon = tab.Icon;
                if (icon == null) continue;

                icon.Location = new Point(x, (Height - icon.Height - 1)/2 + 1);
                icon.MergeMethod = MergeMethods.Invert;

                _container.Controls.Add(icon);

                x += Margin + iconWidth;
            }

            Invalidate();
            ResumeLayout();
        }

        private void ResetControls()
        {
            _container.Controls.Clear();
        }

        protected override void OnVisibleChanged()
        {
            if (Visible)
            {
                PlaceControls();
            }
            else
            {
                ResetControls();
            }
            base.OnVisibleChanged();
        }

        protected override void OnPaint(FramePaintEventArgs e)
        {
            _container.PerformLayout();
            e.Bitmap.MergeOverride(_container.Bitmap, new Point(0, 0));
            base.OnPaint(e);
        }

        protected override void OnButtonDown(ButtonEventArgs e)
        {
            var task = FrameTabMenuButtonTask.None;
            switch (e.Button)
            {
                case 0:
                    task = Button0Task;
                    break;
                case 1:
                    task = Button1Task;
                    break;
                case 2:
                    task = Button2Task;
                    break;
                case 3:
                    task = Button3Task;
                    break;
            }

            switch (task)
            {
                case FrameTabMenuButtonTask.Close:
                    Hide();
                    e.PreventPropagation = true;
                    return;
                case FrameTabMenuButtonTask.Previous:
                    SelectPrevious();
                    e.PreventPropagation = true;
                    return;
                case FrameTabMenuButtonTask.Next:
                    SelectNext();
                    e.PreventPropagation = true;
                    return;
            }

            base.OnButtonDown(e);
        }

        #endregion
    }
}