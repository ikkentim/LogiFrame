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

using System.Linq;

namespace SanAndreas.SAInfo
{
    public class Vehicles
    {
        #region Data

        private static readonly string[] VehicleNames =
        {
            "Landstalker",
            "Bravura",
            "Buffalo",
            "Linerunner",
            "Perrenial",
            "Sentinel",
            "Dumper",
            "Firetruck",
            "Trashmaster",
            "Stretch",
            "Manana",
            "Infernus",
            "Voodoo",
            "Pony",
            "Mule",
            "Cheetah",
            "Ambulance",
            "Leviathan",
            "Moonbeam",
            "Esperanto",
            "Taxi",
            "Washington",
            "Bobcat",
            "Mr Whoopee",
            "BF Injection",
            "Hunter",
            "Premier",
            "Enforcer",
            "Securicar",
            "Banshee",
            "Predator",
            "Bus",
            "Rhino",
            "Barracks",
            "Hotknife",
            "Trailer 1",
            "Previon",
            "Coach",
            "Cabbie",
            "Stallion",
            "Rumpo",
            "RC Bandit",
            "Romero",
            "Packer",
            "Monster",
            "Admiral",
            "Squalo",
            "Sparrow",
            "Pizzaboy",
            "Tram",
            "Trailer 2",
            "Turismo",
            "Speeder",
            "Reefer",
            "Tropic",
            "Flatbed",
            "Yankee",
            "Caddy",
            "Solair",
            "Berkley's RC Van",
            "Skimmer",
            "PCJ-600",
            "Faggio",
            "Freeway",
            "RC Baron",
            "RC Raider",
            "Glendale",
            "Oceanic",
            "Sanchez",
            "Seasparrow",
            "Patriot",
            "Quad",
            "Coastguard",
            "Dinghy",
            "Hermes",
            "Sabre",
            "Rustler",
            "ZR-350",
            "Walton",
            "Regina",
            "Comet",
            "BMX",
            "Burrito",
            "Camper",
            "Marquis",
            "Baggage",
            "Dozer",
            "Maverick",
            "News Maverick",
            "Rancher",
            "FBI Rancher",
            "Virgo",
            "Greenwood",
            "Jetmax",
            "Hotring",
            "Sandking",
            "Blista Compact",
            "Police Maverick",
            "Boxville",
            "Benson",
            "Mesa",
            "RC Goblin",
            "Hotring Racer A",
            "Hotring Racer B",
            "Bloodring Banger",
            "Rancher",
            "Super GT",
            "Elegant",
            "Journey",
            "Bike",
            "Mountain Bike",
            "Beagle",
            "Cropdust",
            "Stunt",
            "Tanker",
            "Roadtrain",
            "Nebula",
            "Majestic",
            "Buccaneer",
            "Shamal",
            "Hydra",
            "FCR-900",
            "NRG-500",
            "HPV1000",
            "Cement Truck",
            "Tow Truck",
            "Fortune",
            "Cadrona",
            "FBI Truck",
            "Willard",
            "Forklift",
            "Tractor",
            "Combine",
            "Feltzer",
            "Remington",
            "Slamvan",
            "Blade",
            "Freight",
            "Brownstreak",
            "Vortex",
            "Vincent",
            "Bullet",
            "Clover",
            "Sadler",
            "Ladder Firetruck",
            "Hustler",
            "Intruder",
            "Primo",
            "Cargobob",
            "Tampa",
            "Sunrise",
            "Merit",
            "Utility",
            "Nevada",
            "Yosemite",
            "Windsor",
            "Monster A",
            "Monster B",
            "Uranus",
            "Jester",
            "Sultan",
            "Stratum",
            "Elegy",
            "Raindance",
            "RC Tiger",
            "Flash",
            "Tahoma",
            "Savanna",
            "Bandito",
            "Freight Flat",
            "Streak Carriage",
            "Kart",
            "Mower",
            "Dune",
            "Sweeper",
            "Broadway",
            "Tornado",
            "AT-400",
            "DFT-30",
            "Huntley",
            "Stafford",
            "BF-400",
            "Newsvan",
            "Tug",
            "Trailer 3",
            "Emperor",
            "Wayfarer",
            "Euros",
            "Hotdog",
            "Club",
            "Freight Carriage",
            "Trailer 4",
            "Andromada",
            "Dodo",
            "RC Cam",
            "Launch",
            "Police Car (LSPD)",
            "Police Car (SFPD)",
            "Police Car (LVPD)",
            "Police Ranger",
            "Picador",
            "S.W.A.T. Van",
            "Alpha",
            "Phoenix",
            "Old Glendale",
            "Old Sadler",
            "Luggage Trailer A",
            "Luggage Trailer B",
            "Stair Trailer",
            "Boxville",
            "Farm Plow",
            "Utility Trailer"
        };

        private static readonly int[] PlaneIds =
        {
            417, 425, 447, 460, 469, 476, 487, 488, 497, 511, 512, 513, 519, 520,
            548, 553, 563, 577, 592, 593
        };

        private static readonly int[] VehicleSeats =
        {
            4, 2, 2, 2, 4, 4, 1, 2, 2, 4, 2, 2, 2, 4, 2, 2, 4, 2, 4, 2, 4, 4, 2, 2, 2, 1, 4, 4, 4, 2, 1, 10, 1, 2, 2, 0,
            2, 10, 4, 2, 4, 1, 2, 2, 2, 4, 1, 2,
            1, 0, 0, 2, 1, 1, 1, 2, 2, 2, 4, 4, 2, 2, 2, 2, 1, 1, 4, 4, 2, 2, 4, 2, 1, 1, 2, 2, 1, 2, 2, 4, 2, 1, 4, 3,
            1, 1, 1, 4, 2, 2, 4, 2, 4, 1, 2, 2, 2, 4,
            4, 2, 2, 1, 2, 2, 2, 2, 2, 4, 2, 1, 1, 2, 1, 1, 2, 2, 4, 2, 2, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 4, 1, 1, 1, 2,
            2, 2, 2, 10, 10, 1, 4, 2, 2, 2, 2, 2,
            4, 4, 2, 2, 4, 4, 2, 1, 2, 2, 2, 2, 2, 2, 4, 4, 2, 2, 1, 2, 4, 4, 1, 0, 0, 1, 1, 2, 1, 2, 2, 1, 2, 4, 4, 2,
            4, 1, 0, 4, 2, 2, 2, 2, 0, 0, 10, 2, 2,
            1, 4, 4, 4, 2, 2, 2, 2, 2, 4, 2, 0, 0, 0, 4, 0, 0
        };

        #endregion

        public static string GetVehicleName(int vehicleid)
        {
            return (vehicleid < 400 || vehicleid - 400 >= VehicleNames.Length)
                ? "Unknown"
                : VehicleNames[vehicleid - 400];
        }

        public static bool IsVehiclePlane(int vehicleid)
        {
            return PlaneIds.Contains(vehicleid);
        }

        public static int GetVehicleSeats(int vehicleid)
        {
            return (vehicleid < 400 || vehicleid - 400 >= VehicleSeats.Length)
                ? 0
                : VehicleSeats[vehicleid - 400];
        }
    }
}