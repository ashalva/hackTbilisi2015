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
using hackTbilisi2015.Helpers;

namespace hackTbilisi2015.Activities
{
	[Activity (Label = "SearchActivity")]
	public class SearchActivity : Activity, IBeaconConsumer
	{
		IBeaconManager _iBeaconManager;
		RangeNotifier _rangeNotifier;
		List<Region> _rangingRegion;
		List<BeaconsInfo> _beaconsInfo;
		private List<iBeacon> _beacons;
		int foundBeaconsCount = 0;
		TextView foundText;
		TextView proximityText;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Search);
			foundText = FindViewById<TextView> (Resource.Id.found);
			foundText.Text = $"ნაპოვნია: {foundBeaconsCount}";
			proximityText = FindViewById<TextView> (Resource.Id.proximity);
			proximityText.Text = "";
			var json = Intent.GetStringExtra ("Beacons");
			_beacons = JsonConvert.DeserializeObject<List<iBeacon>> (json);
			_beaconsInfo = new List<BeaconsInfo> ();
			_iBeaconManager = IBeaconManager.GetInstanceForApplication (this);

			_iBeaconManager.Bind (this);
			_rangingRegion = new List<Region> ();
			_rangeNotifier = new RangeNotifier ();
			_rangeNotifier.DidRangeBeaconsInRegionComplete += DidRange;

			foreach (var beacon in _beacons) {
				_rangingRegion.Add (new Region (beacon.Name, beacon.UUID, null, null));
				_beaconsInfo.Add (new BeaconsInfo {
					Beacon = beacon,
					WasFound = false,
					ProximityType = ProximityType.Unknown
				});
			}
		}

		public void OnIBeaconServiceConnect ()
		{
			_iBeaconManager.SetRangeNotifier (_rangeNotifier);
			foreach (var region in _rangingRegion) {
				_iBeaconManager.RangedRegions.Add (region);
				_iBeaconManager.StartRangingBeaconsInRegion (region);
			}
		}

		void DidRange (object sender, RangeEventArgs e)
		{
			if (e.Beacons.Count > 0) {
				foreach (var beacon in e.Beacons) {
					var currentBeaconInfo = _beaconsInfo.FirstOrDefault (x => x.Beacon.UUID.ToLower () == beacon.ProximityUuid && x.Beacon.Major == beacon.Major && x.Beacon.Minor == beacon.Minor);
					if (!(currentBeaconInfo == null) && !currentBeaconInfo.WasFound)
						switch ((ProximityType)beacon.Proximity) {
						case ProximityType.Immediate:
							currentBeaconInfo.ProximityType = ProximityType.Immediate;
							break;
						case ProximityType.Near:
							currentBeaconInfo.ProximityType = ProximityType.Near;
							break;
						case ProximityType.Far:
							currentBeaconInfo.ProximityType = ProximityType.Far;
							break;
						case ProximityType.Unknown:
							currentBeaconInfo.ProximityType = ProximityType.Unknown;
							break;
						}
				}
				var nearest = _beaconsInfo.Where (x => !x.WasFound && !(x.ProximityType == ProximityType.Unknown)).OrderBy (x => (int)x.ProximityType).FirstOrDefault ();
				if (nearest?.ProximityType == ProximityType.Immediate) {
					nearest.WasFound = true;
					foundBeaconsCount++;
					RunOnUiThread (() => foundText.Text = $"ნაპოვნია: {foundBeaconsCount}");
				}

				if (nearest != null)
					RunOnUiThread (() => proximityText.Text = nearest.ProximityType.ToString ());

				if (foundBeaconsCount == _beacons.Count) {
					RunOnUiThread (() => proximityText.Text = "You found all beacons! Game over!");
				}
			}
			Console.WriteLine ();
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();

			_rangeNotifier.DidRangeBeaconsInRegionComplete -= DidRange;

			foreach (var region in _rangingRegion)
				_iBeaconManager.StopRangingBeaconsInRegion (region);
			_iBeaconManager.UnBind (this);
		}
	}
}