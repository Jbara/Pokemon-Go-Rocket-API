﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Window
{
    public class LocationManager
    {
        private Client client;
        private double kilometersPerMillisecond;

        public LocationManager(Client client, double speed)
        {
            this.client = client;
            this.kilometersPerMillisecond = speed / 3600000;
        }

        public double getDistance(double lat, double lng)
        {
            Coordinate currentLoc = new Coordinate(client.getCurrentLat(), client.getCurrentLong());
            return currentLoc.distanceFrom(new Coordinate(lat, lng));
        }

        public async Task update(double lat, double lng)
        {
            double waitTime = getDistance(lat, lng)/this.kilometersPerMillisecond;
            await Task.Delay((int)Math.Ceiling(waitTime));
            await client.UpdatePlayerLocation(lat, lng);
        }

        public struct Coordinate
        {

            public Coordinate(double lat, double lng)
            {
                this.latitude = lat;
                this.longitude = lng;
            }
            public double latitude;
            public double longitude;

            //returns distance in kilometers 
            public double distanceFrom(Coordinate c2)
            {
                double R = 6371;
                Func<double, double> toRad = x => x * (Math.PI / 180);

                double lat1 = toRad(this.latitude);
                double lat2 = toRad(c2.latitude);
                double dLng = toRad(c2.longitude - c2.longitude);

                return Math.Acos(Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(dLng)) * R;
            }
        }
    }

}
