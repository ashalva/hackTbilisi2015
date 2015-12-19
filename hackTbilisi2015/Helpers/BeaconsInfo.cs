using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RadiusNetworks.IBeaconAndroid;
using hackTbilisi2015.Core.BusinessObjects;

namespace hackTbilisi2015.Helpers
{
    public class BeaconsInfo
    {
        public iBeacon Beacon { get; set; }
        public ProximityType ProximityType { get; set; }
        public bool WasFound { get; set; }
    }
}