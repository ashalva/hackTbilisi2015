using Android.App;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Content;
using System;

namespace hackTbilisi2015
{
    [Activity(Label = "hackTbilisi2015", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity, BluetoothAdapter.ILeScanCallback
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            var adapter = BluetoothAdapter.DefaultAdapter;

            adapter.StartLeScan(this);
          
        }

        public void OnLeScan(BluetoothDevice device, int rssi, byte[] scanRecord)
        {
            //device.CreateBond();
            var res = device.FetchUuidsWithSdp();
            var uids = device.GetUuids();

            int startByte = 2;
            var patternFound = false;
            while (startByte <= 5)
            {
                if (((int)scanRecord[startByte + 2] & 0xff) == 0x02 && //Identifies an iBeacon
                        ((int)scanRecord[startByte + 3] & 0xff) == 0x15)
                { //Identifies correct data length
                    patternFound = true;
                    break;
                }
                startByte++;
            }

            if (patternFound)
            {
                //Convert to hex String
                byte[] uuidBytes = new byte[16];
                //System.arraycopy(scanRecord, startByte + 4, uuidBytes, 0, 16);
                Array.Copy(scanRecord, startByte + 4, uuidBytes, 0, 16);
                var hexString = BitConverter.ToString(uuidBytes).Replace("-","");

                //Here is your UUID
                var uuid = hexString.Substring(0, 8) + "-" +
                        hexString.Substring(8, 4) + "-" +
                        hexString.Substring(12, 4) + "-" +
                        hexString.Substring(16, 4) + "-" +
                        hexString.Substring(20, 12);

                //Here is your Major value
                int major = (scanRecord[startByte + 20] & 0xff) * 0x100 + (scanRecord[startByte + 21] & 0xff);

                //Here is your Minor value
                int minor = (scanRecord[startByte + 22] & 0xff) * 0x100 + (scanRecord[startByte + 23] & 0xff);
            }
        }

     
    }
}
