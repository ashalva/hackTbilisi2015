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
using Newtonsoft.Json;
using hackTbilisi2015.Core.BusinessObjects;

namespace hackTbilisi2015.Activities
{
    [Activity(Label = "SearchActivity")]
    public class SearchActivity : Activity
    {
        View _view;
        IBeaconManager _iBeaconManager;
        MonitorNotifier _monitorNotifier;
        Region _monitoringRegion;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var json = Intent.GetStringExtra("Beacons");
            var beacons = JsonConvert.DeserializeObject<List<iBeacon>>(json);
            // Create your application here
        }


    }
}