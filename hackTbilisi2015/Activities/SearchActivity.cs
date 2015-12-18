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
	[Activity (Label = "SearchActivity")]
	public class SearchActivity : Activity,IBeaconConsumer
	{
		IBeaconManager _iBeaconManager;
		RangeNotifier _rangeNotifier;
		Region _rangingRegion;
		private List<iBeacon> _beacons;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			var json = Intent.GetStringExtra ("Beacons");
			_beacons = JsonConvert.DeserializeObject<List<iBeacon>> (json);
		
			_iBeaconManager = IBeaconManager.GetInstanceForApplication (this);

			_iBeaconManager.Bind (this);

			_rangeNotifier.DidRangeBeaconsInRegionComplete += DidRange;
		}

		public void OnIBeaconServiceConnect ()
		{
			foreach (var beacon in _beacons) {
				_rangeNotifier = new RangeNotifier ();
				_rangingRegion = new Region (beacon.Name, beacon.UUID, (Java.Lang.Integer)beacon.Major, (Java.Lang.Integer)beacon.Minor);
				_iBeaconManager.SetRangeNotifier (_rangeNotifier);
				_iBeaconManager.StartRangingBeaconsInRegion (_rangingRegion);
			}
		}

		void DidRange (object sender, RangeEventArgs e)
		{
			if (e.Beacons.Count > 0) {
				var beacon = e.Beacons.FirstOrDefault ();

				switch ((ProximityType)beacon.Proximity) {
				case ProximityType.Immediate:
					RunOnUiThread (() => Toast.MakeText (this, "You found the monkey!", ToastLength.Short).Show ());
					break;
				case ProximityType.Near:
					RunOnUiThread (() => Toast.MakeText (this, "You're getting warmer", ToastLength.Short).Show ());
					break;
				case ProximityType.Far:
					RunOnUiThread (() => Toast.MakeText (this, "You're freezing cold", ToastLength.Short).Show ());
					break;
				case ProximityType.Unknown:
					RunOnUiThread (() => Toast.MakeText (this, "I'm not sure how close you are to the monkey", ToastLength.Short).Show ());
					break;
				}
			}
		}
	}
}