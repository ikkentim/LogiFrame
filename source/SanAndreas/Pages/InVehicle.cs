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
using SanAndreas.Components;

namespace SanAndreas.Pages
{
    internal class InVehicle : Page
    {
        private readonly ProgressBar _healthBar;
        private readonly Label _locationLabel;
        private readonly Label _nameLabel;
        private readonly NitroInfo _nitroInfo;
        private readonly Label _notInVehicleLabel;
        private readonly Label _radioLabel;
        private readonly Label _speedLabel;
        private readonly Timer _timer;

        public InVehicle()
        {
            Components.Add(_notInVehicleLabel = new Label
            {
                Location = new Location(1, 1),
                AutoSize = true,
                Font = new Font("Arial", 10f, FontStyle.Bold),
                Text = "You are currently\n not in a vehicle!"
            });
            Components.Add(_nameLabel = new Label
            {
                Location = new Location(0, 16),
                AutoSize = true,
                Font = new Font("Arial", 8f),
                Text = "Burito"
            });
            Components.Add(_locationLabel = new Label
            {
                Location = new Location(0, 30),
                AutoSize = true,
                Font = new Font("Arial", 8),
                Text = "Unknown"
            });
            Components.Add(_speedLabel = new Label
            {
                Location = new Location(108, 28),
                AutoSize = true,
                Font = new Font("Arial", 10f, FontStyle.Bold),
                Text = "00",
                UseCache = true,
                HorizontalAlignment = Alignment.Right
            });
            Components.Add(_radioLabel = new Label
            {
                Location = new Location(Frame.LCDSize.Width/2, 1),
                AutoSize = true,
                Font = new Font("Arial", 12f, FontStyle.Bold),
                Text = "No Radio",
                Visible = false,
                UseCache = true,
                Transparent = true,
                TopEffect = true,
                HorizontalAlignment = Alignment.Center
            });
            Components.Add(_healthBar = new ProgressBar
            {
                Location = new Location(109, 32),
                Size = new LogiFrame.Size(50, 10),
                MaximumValue = 750,
                ProgressBarStyle = ProgressBarStyle.WhiteSpacedBorder
            });
            Components.Add(_nitroInfo = new NitroInfo
            {
                Location = new Location(129, 2)
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

        private void ToggleVehicle(bool toggle)
        {
            _notInVehicleLabel.Visible = !toggle;
            _nameLabel.Visible = toggle;
            _locationLabel.Visible = toggle;
            _speedLabel.Visible = toggle;
            _healthBar.Visible = toggle;
            //_vehicleInfo.Visible = toggle;
            _nitroInfo.Visible = toggle;
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

            if (!vehicle.IsPointing)
            {
                ToggleVehicle(false);
                return;
            }

            ToggleVehicle(true);

            //General memory
            var radioid = GTA.Memory ^ 0x8CB7A5; //AsShort();

            //Vehicle memory
            var type = ~vehicle + 0x590; //ReadByte();
            var model = ~vehicle + 0x22; //.ReadShort();

            var health = ~vehicle + 0x4C0; //.ReadFloat();

            var nos = ~vehicle + 0x48A; //).ReadByte();
            var nosStatus = ~vehicle + 0x8A4; //.ReadFloat();

            var speedx = (~vehicle + 68).AsFloat(); //.ReadFloat();
            var speedy = (~vehicle + 72).AsFloat(); //.ReadFloat();
            var speedz = (~vehicle + 76).AsFloat(); //.ReadFloat();
            var speed = (int) Math.Round(Math.Sqrt(((speedx*speedx) + (speedy*speedy)) + (speedz*speedz))*136.6666667);

            //Position memory
            var position = ~vehicle + 0x14;
            var x = ~position + 0x30;
            var y = ~position + 0x34;
            var z = ~position + 0x38;

            var zPos = z.AsFloat();


            //Seats
            var seats = new bool[SAInfo.Vehicles.GetVehicleSeats(model.AsShort())];
            for (int i = 0; i < SAInfo.Vehicles.GetVehicleSeats(model.AsShort()); i++)
            {
                var passengerPointer = ~vehicle + (0x460 + i*0x4);
                seats[i] = passengerPointer.IsPointing;
            }

            //_radioLabel.Text = .RadioStations.GetRadioName(radioid);
            _nameLabel.Text = SAInfo.Vehicles.GetVehicleName(model.AsShort());
            _locationLabel.Text = SAInfo.Zones.GetLocationName(x.AsFloat(), y.AsFloat());

            //vehicleInfo.Visible = !SanAndreas.Vehicles.IsVehiclePlane(model);
            //vehicleInfo.IsBike = type == (int)SanAndreas.Vehicle.Bike;
            /*
            if (type == (int)SanAndreas.Vehicle.PlaneCar && !SanAndreas.Vehicles.IsVehiclePlane(model))
            {
                vehicleInfo.SetTire(0, vehicle.GetOffset(0x5A5).ReadByte() == 2);
                vehicleInfo.SetTire(1, vehicle.GetOffset(0x5A6).ReadByte() == 2);
                vehicleInfo.SetTire(2, vehicle.GetOffset(0x5A7).ReadByte() == 2);
                vehicleInfo.SetTire(3, vehicle.GetOffset(0x5A8).ReadByte() == 2);
            }
            else if (type == (int)SanAndreas.Vehicle.Bike)
            {
                vehicleInfo.SetTire(0, vehicle.GetOffset(0x65C).ReadByte() == 2);
                vehicleInfo.SetTire(1, vehicle.GetOffset(0x65D).ReadByte() == 2);
            }
            else if (type == (int)SanAndreas.Vehicle.PlaneCar && SanAndreas.Vehicles.IsVehiclePlane(model))
            {
                //TODO: Draw altitude
                //Drawing.Progressbar.Draw(e.graphics, new Rectangle(145, 8, 8, 32), Direction.Up, (z % 250) / 5, true);
            }
            */

            //vehicleInfo.Seats = SanAndreas.Vehicles.GetVehicleSeats(model);
            //for (int i = 0; i < vehicleInfo.Seats; i++)
            //    vehicleInfo.SetSeat(i, seats[i]);

            _healthBar.Value = (750 - (health.AsFloat() - 250));

            _nitroInfo.Count = nos.AsByte();
            _nitroInfo.Status = nosStatus.AsFloat();
            _speedLabel.Text = speed.ToString("00");
        }

        protected override PageIcon GetPageIcon()
        {
            return new PageIcon(new Component[]
            {
                new Picture
                {
                    Location = new Location(1, 0),
                    AutoSize = true,
                    ConversionMethod = ConversionMethod.QuarterByte,
                    Image = Properties.Resources.CarIcon
                }
            });
        }

        public override string GetName()
        {
            return "In Vehicle";
        }
    }
}