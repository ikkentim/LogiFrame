// LogiFrame rendering library.
// Copyright (C) 2014 Tim Potze
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>. 

using System;
using System.Drawing;
using LogiFrame;
using LogiFrame.Components;
using LogiFrame.Components.Book;
using Size = LogiFrame.Size;

namespace SanAndreas.Pages
{
    internal class OnFoot : Page
    {
        private readonly Label _ammoLabel;
        private readonly ProgressBar _armorBar;
        private readonly ProgressBar _healthBar;
        private readonly Label _locationLabel;
        private readonly Label _moneylabel;
        private readonly Label _timeLabel;
        private readonly Timer _timer;
        private readonly Picture _weaponPicture;

        private int _currentWeapon;

        public OnFoot()
        {
            Components.Add(_weaponPicture = new Picture
            {
                Location = new Location(0, 0),
                AutoSize = true,
                Image = Properties.Resources.Weapon0
            }); 
            Components.Add(_healthBar = new ProgressBar
            {
                Location = new Location(35, 15),
                Size = new Size(50, 10),
                ProgressBarStyle = ProgressBarStyle.WhiteSpacedBorder,
                Horizontal = true
            });
            Components.Add(_armorBar = new ProgressBar
            {
                Location = new Location(35, 3),
                Size = new Size(50, 10),
                ProgressBarStyle = ProgressBarStyle.WhiteSpacedBorder,
                MaximumValue = 100,
                Visible = false,
                Horizontal = true
            });
            _ammoLabel = new Label
            {
                Location = new Location(125, 32),
                Font = new Font("Arial", 7),
                AutoSize = true,
                Text = ""
            };
            Components.Add(_locationLabel = new Label
            {
                Location = new Location(0, 30),
                Font = new Font("Arial", 8),
                AutoSize = true,
                Text = "Unknown",
            });
            Components.Add(_moneylabel = new Label
            {
                Location = new Location(94, 0),
                Font = new Font("Arial", 10),
                AutoSize = true,
                Text = "$00000000"
            });
            Components.Add(_timeLabel = new Label
            {
                Location = new Location(125, 13),
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true,
                Text = "00:00"
            });
            Components.Add(_timer = new Timer
            {
                Interval = 250
            });
            _timer.Tick += (sender, args) => Update();
        }

        public override void OnShow(EventArgs e)
        {
            Update();
            _timer.Enabled = true;
        }

        public override void OnHide(EventArgs e)
        {
            _timer.Enabled = false;
        }

        private void Update()
        {
            if (!GTA.IsRunning)
            {
                var book = GetParentComponent<Book>();
                if (book == null) return;
                book.SwitchTo<Detector>();
                return;
            }
            //Globals
            var vehicle = GTA.Memory ^ 0xBA18FC;
            var money = GTA.Memory ^ 0xB7CE50;
            var hours = GTA.Memory ^ 0xB70153;
            var minutes = GTA.Memory ^ 0xB70152;

            //Pools
            var player = GTA.Memory ^ 0xB6F5F0;
            var position = ~player + 0x14;

            //Playerinfo
            var health = ~player + 0x540;
            var maxhealth = ~player + 0x544;
            var armor = ~player + 0x548;

            var weaponslot = ~player + 0x718;
            var weaponslotid = weaponslot.AsByte();

            var weaponid = (~player + (0x5A0 + 0x1C*weaponslotid + 0x0)).AsShort();
            var clip = (~player + (0x5A0 + 0x1C*weaponslotid + 0x8)).AsShort();
            var remaining = (~player + (0x5A0 + 0x1C*weaponslotid + 0xC)).AsShort();

            //Positioninfo
            var x = ~position + 0x30;
            var y = ~position + 0x34;
            //var z = ~position + 0x38;

            _moneylabel.Text = "$" + money.AsInteger().ToString("00000000");
            _timeLabel.Text = hours.AsByte().ToString("00") + ":" + minutes.AsByte().ToString("00");
            _locationLabel.Text = SAInfo.Zones.GetLocationName(x.AsFloat(), y.AsFloat());

            _ammoLabel.Visible = clip + remaining > 0;
            _ammoLabel.Text = (remaining - clip) + "-" + clip;

            if (_currentWeapon != weaponid)
            {
                _currentWeapon = weaponid;
                _weaponPicture.Image =
                    (Bitmap) Properties.Resources.ResourceManager.GetObject("Weapon" + weaponid);
            }
            _healthBar.Value = health.AsFloat();
            _healthBar.MaximumValue = maxhealth.AsFloat();

            _armorBar.Visible = armor.AsFloat() >= 1;
            _armorBar.Value = armor.AsFloat();
        }

        protected override PageIcon GetPageIcon()
        {
            return new PageIcon(new Component[]
            {
                new Picture
                {
                    AutoSize = true,
                    ConversionMethod = ConversionMethod.QuarterByte,
                    Image = Properties.Resources.Footstep
                }
            });
        }

        public override string GetName()
        {
            return "On Foot";
        }
    }
}