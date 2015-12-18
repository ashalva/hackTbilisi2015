using Android.App;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Content;
using System;
using System.Collections.Generic;
using hackTbilisi2015.Core.BusinessObjects;
using System.Linq;
using System.Timers;
using Newtonsoft.Json;

namespace hackTbilisi2015.Activities
{
	[Activity (Label = "hackTbilisi2015", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity, BluetoothAdapter.ILeScanCallback
	{
		private List<iBeacon> _beacons;
        int beaconCount = 4;
        BluetoothAdapter adapter;
        DateTime startDate;
        Timer timer;
        int elapsedSeconds;
        TextView text;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Main);
            text = FindViewById<TextView>(Resource.Id.textView1);
            text.Text = "";
            _beacons = new List<iBeacon>();
            adapter = BluetoothAdapter.DefaultAdapter;
            startDate = DateTime.Now;
            timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;
            elapsedSeconds = 0;
            timer.Start();
			adapter.StartLeScan (this);

           
		}

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            elapsedSeconds++;
            if (elapsedSeconds > 10)
            {
                timer.Stop();
                adapter.StopLeScan(this);
                elapsedSeconds = 0;
            }
        }

        public void OnLeScan (BluetoothDevice device, int rssi, byte[] scanRecord)
		{
			int startByte = 2;
			var patternFound = false;
			while (startByte <= 5) {
				if (((int)scanRecord [startByte + 2] & 0xff) == 0x02 &&
				    ((int)scanRecord [startByte + 3] & 0xff) == 0x15) { //Identifies correct data length
					patternFound = true;
					break;
				}
				startByte++;
			}

			if (patternFound) {
				//Convert to hex String
				byte[] uuidBytes = new byte[16];
				Array.Copy (scanRecord, startByte + 4, uuidBytes, 0, 16);
				var hexString = BitConverter.ToString (uuidBytes).Replace ("-", "");

				//Here is your UUID
				var uuid = hexString.Substring (0, 8) + "-" +
				           hexString.Substring (8, 4) + "-" +
				           hexString.Substring (12, 4) + "-" +
				           hexString.Substring (16, 4) + "-" +
				           hexString.Substring (20, 12);

				//Here is your Major value
				int major = (scanRecord [startByte + 20] & 0xff) * 0x100 + (scanRecord [startByte + 21] & 0xff);

				//Here is your Minor value
				int minor = (scanRecord [startByte + 22] & 0xff) * 0x100 + (scanRecord [startByte + 23] & 0xff);

				if (!_beacons.Any (b => b.UUID == uuid && b.Major == major && b.Minor == minor))
					_beacons.Add (new iBeacon () { 
						UUID = uuid,
						Minor = Convert.ToUInt16(minor),
						Major = Convert.ToUInt16(major),
						Name = device.Name,
						MacAddress = device.Address
					});
			}
            if (_beacons.Count==beaconCount)
            {
                Toast.MakeText(this, "ვიპოვე 4 ცალი.", ToastLength.Long).Show();
                adapter.StopLeScan(this);
                timer.Stop();
                elapsedSeconds = 0;
                foreach (var beacon in _beacons)
                {
                    text.Text += beacon.Name;
                }

                var intent = new Intent(this,typeof(SearchActivity));
                var serializedBeacons = JsonConvert.SerializeObject(_beacons);
                intent.PutExtra("Beacons", serializedBeacons);
                StartActivity(intent);
            }
		}
     
	}
}
