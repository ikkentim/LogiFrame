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
using LogiFrame.Drawing;
using LogiFrame.Internal;

namespace LogiFrame
{
    /// <summary>
    /// </summary>
    public class Frame : ContainerFrameControl
    {
        private readonly LgLcd.ConnectContext _connection;
        private int _oldButtons;
        private readonly int _device;

        public static Size DefaultSize { get; } = new Size((int) LgLcd.BitmapWidth, (int) LgLcd.BitmapHeight);

        public Frame(string name, bool canAutoStart, bool isPersistent, bool allowConfiguration)
        {
            UpdatePriority = UpdatePriority.Normal;

            _connection.AppFriendlyName = name;
            _connection.IsAutostartable = canAutoStart;
            _connection.IsPersistent = isPersistent;

            if (allowConfiguration)
                _connection.OnConfigure.ConfigCallback = (connection, pContext) => 1;

            UnmanagedLibrariesLoader.Load();
            LgLcd.Init();

            var connectionResponse = LgLcd.Connect(ref _connection);

            if (connectionResponse != 0)
                throw new ConnectionException(connectionResponse);

            var openContext = new LgLcd.OpenContext
            {
                Connection = _connection.Connection,
                Index = 0,
                OnSoftButtonsChanged =
                {
                    Callback = (device, buttons, context) =>
                    {
                        for (var button = 0; button < 4; button++)
                        {
                            var buttonIdentifier = 1 << button;

                            if ((buttons & buttonIdentifier) > (_oldButtons & buttonIdentifier))
                                OnButtonDown(new ButtonEventArgs(button));
                            else if ((buttons & buttonIdentifier) < (_oldButtons & buttonIdentifier))
                                OnButtonUp(new ButtonEventArgs(button));
                        }
                        _oldButtons = buttons;
                        return 1;
                    }
                },
            };
            
            LgLcd.Open(ref openContext);
            _device = openContext.Device;

            SetBounds(0, 0, DefaultSize.Width, DefaultSize.Height);
            InitLayout();
        }

        public UpdatePriority UpdatePriority { get; set; }
        public event EventHandler Configure;
        public event EventHandler<ButtonEventArgs> ButtonDown;
        public event EventHandler<ButtonEventArgs> ButtonUp;
        public event EventHandler<RenderedEventArgs> Rendered;
        #region Overrides of FrameControl

        public override void Invalidate()
        {
            base.Invalidate();
            PerformLayout();
        }

        #endregion

        #region Overrides of ContainerFrameControl

        protected override void OnPaint(FramePaintEventArgs e)
        {
            base.OnPaint(e);
            Push(e.Bitmap);
        }

        #endregion

        protected virtual void OnConfigure()
        {
            Configure?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnButtonDown(ButtonEventArgs e)
        {
            ButtonDown?.Invoke(this, e);
        }

        protected virtual void OnButtonUp(ButtonEventArgs e)
        {
            ButtonUp?.Invoke(this, e);
        }

        private void Push(MonochromeBitmap bitmap)
        {
            if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));

            var render = new MonochromeBitmap(bitmap, (int) LgLcd.BitmapWidth, (int) LgLcd.BitmapHeight);
            var lgBitmap = new LgLcd.Bitmap160X43X1
            {
                hdr = {Format = LgLcd.BitmapFormat160X43X1},
                pixels = render.Data
            };

            LgLcd.UpdateBitmap(_device, ref lgBitmap, (uint) UpdatePriority);
            OnRendered(new RenderedEventArgs(render));
        }

        protected virtual void OnRendered(RenderedEventArgs e)
        {
            Rendered?.Invoke(this, e);
        }

        public virtual bool IsButtonDown(int key)
        {
            return (_oldButtons & (1 << key)) != 0;
        }
    }
}